using System;
using System.Collections;
using System.Text;
using FullSerializer;
using Model;
using Service.Request;
using Service.Response;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

namespace Service {
    public class BlueprintAPI {
        
        public delegate void Callback<T>(APIResult<T, string> result);
        private const string baseURL = "http://smithwjv.ddns.net";
        private const string endpointAuthenticate = baseURL + ":8000/api/v1/authenticate";
        private const string endpointInventory = baseURL + ":8001/api/v1/inventory";
        private const string endpointProgress = baseURL + ":8003/api/v1/progress";
        private const string endpointSchema = baseURL + ":8003/api/v1/item-schema";
        private static readonly RestHandler restHandler = new RestHandler(endpointAuthenticate + "/refresh");
      
        // Login an existing user and generate an access token
        public static IEnumerator Login(string username, string password, Callback<AccessToken> callback) {
            string json = JsonUtility.ToJson(new RequestAuthenticate(username, password));
            return restHandler.requestPOST(endpointAuthenticate, json, handleResponse(callback));
        }

        // Register a new user and generate an access token
        public static IEnumerator Register(string username, string password, Callback<AccessToken> callback) {
            string json = JsonUtility.ToJson(new RequestAuthenticate(username, password));
            return restHandler.requestPOST(endpointAuthenticate + "/register", json, handleResponse(callback)); 
        }

        // Get the existing game state, given a user's access token
        public static IEnumerator GetGameState(AccessToken accessToken, Callback<GameState> callback) {
            return restHandler.requestAuthorizedGET(endpointProgress + "/desktop-state", accessToken, handleResponse(callback));
        }
        
        // Update the current game state, given a user's access token
        public static IEnumerator SaveGameState(AccessToken accessToken, GameState gameState, Callback<bool> callback) {
            fsData data;
            new fsSerializer().TrySerialize(gameState, out data).AssertSuccessWithoutWarnings();
            string json = fsJsonPrinter.CompressedJson(data);
            return restHandler.requestAuthorizedPOST(endpointProgress + "/desktop-state", json, accessToken, handleEmptyResponse(callback));
        }

        // Get the inventory, given a user's access token
        public static IEnumerator GetInventory(AccessToken accessToken, Callback<ResponseGetInventory> callback) {
            return restHandler.requestAuthorizedGET(endpointInventory, accessToken, handleResponse(callback));
        }

        // Clear the inventory, given a user's access token
        public static IEnumerator DeleteInventory(AccessToken accessToken, Callback<bool> callback) {
            return restHandler.requestAuthorizedDELETE(endpointInventory, accessToken, handleEmptyResponse(callback));
        }

        // Add a completed blueprint, given a user's access token
        public static IEnumerator AddCompletedBlueprints(AccessToken accessToken, RequestCompletedBlueprint request, Callback<bool> callback) {
            string json = JsonUtility.ToJson(request);
            return restHandler.requestAuthorizedPOST(endpointProgress, json, accessToken, handleEmptyResponse(callback));
        }
        
        // Get completed blueprints, given a user's access token
        public static IEnumerator GetCompletedBlueprints(AccessToken accessToken, Callback<ResponseGetCompletedBlueprints> callback) {
            return restHandler.requestAuthorizedGET(endpointProgress, accessToken, handleResponse(callback));
        }

        // Get the schema for the game
        public static IEnumerator GetSchema(Callback<SchemaItems> callback) {
            return restHandler.requestGET(endpointSchema, handleResponse(callback));
        }

        // Generic response handler that will attempt to decode a response into JSON
        private static RestHandler.Callback handleResponse<T>(Callback<T> then) {
            return result => {
                if (result.isSuccess()) {
                    T decoded = default(T);
                    fsData data = fsJsonParser.Parse(result.GetSuccess());
                    new fsSerializer().TryDeserialize(data, ref decoded).AssertSuccessWithoutWarnings();
                    then(APIResult<T, string>.Success(decoded));
                } else {
                    ResponseError error = JsonUtility.FromJson<ResponseError>(result.GetError());
                    then(APIResult<T, string>.Error(error.error));
                }
            };
        }

        // Generic response handler that will accept any successful request
        private static RestHandler.Callback handleEmptyResponse(Callback<bool> then) {
            return result => {
                if (result.isSuccess()) {
                    then(APIResult<bool, string>.Success(true));
                } else {
                    ResponseError error = JsonUtility.FromJson<ResponseError>(result.GetError());
                    then(APIResult<bool, string>.Error(error.error));
                }
            };
        }
        
    }
}