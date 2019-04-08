using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Service.Request;
using Service.Response;
using UnityEngine;
using UnityEngine.Networking;

namespace Service {
    public class RestHandler {
        struct ContentType {
            public const string json = "application/json";
        }
        public delegate void Callback(APIResult<string, string> result);
        public delegate void RefreshCallback(APIResult<AccessToken, string> result);
        private readonly string refreshURL;

        public RestHandler(string refreshURL) {
            this.refreshURL = refreshURL; 
        }
       
        // Perform a GET request to the provided URL
        public IEnumerator requestGET(string url, Callback callback) {
            return performGET(url, new Dictionary<string, string>(), callback);
        }

        // Perform a GET request to the provided URL, using the access token for authentication
        public IEnumerator requestAuthorizedGET(string url, AccessToken accessToken, Callback callback) {
            Dictionary<string, string> headers = new Dictionary<string, string> {
                {"Authorization", "Bearer " + accessToken.GetAccess()}
            };
            return performGET(url, headers, callback);
        }

        // Perform a POST request to the provided URL
        public IEnumerator requestPOST(string url, string json, Callback callback) {
            return performPOST(url, json, new Dictionary<string, string>(), callback);
        }

        // Perform a POST request to the provided URL, using the access token for authentication
        public IEnumerator requestAuthorizedPOST(string url, string json, AccessToken accessToken, Callback callback) {
            Dictionary<string, string> headers = new Dictionary<string, string> {
                {"Authorization", "Bearer " + accessToken.GetAccess()}
            };
            return performPOST(url, json, headers, callback);
        }

        // Perform a DELETE request to the provided URL, using the access token for authentication
        public IEnumerator requestAuthorizedDELETE(string url, AccessToken accessToken, Callback callback) {
            Dictionary<string, string> headers = new Dictionary<string, string> {
                {"Authorization", "Bearer " + accessToken.GetAccess()}
            };
            return performDELETE(url, headers, callback);
        }

        // Internal method for performing a GET request
        private IEnumerator performGET(string url, Dictionary<string, string> headers, Callback callback) {
            using (UnityWebRequest w = UnityWebRequest.Get(url)) {
                foreach (KeyValuePair<string, string> keyValuePair in headers) {
                    w.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
                }

                yield return w.SendWebRequest();

                if (w.isNetworkError || w.isHttpError) {
                    callback(APIResult<string, string>.Error(w.downloadHandler.text));
                } else if (w.responseCode == (int) HttpStatusCode.Unauthorized) {
                    // If unauthorized, refresh tokens and reattempt using new token
                    yield return refreshTokens(result => {
                        if (result.isSuccess()) {
                            headers["Authorization"] = "Bearer " + result.GetSuccess().GetAccess();
                            performGET(url, headers, callback);
                        } else {
                            callback(APIResult<string, string>.Error("Invalid access & refresh token pair")); 
                        }
                    });
                } else {
                    callback(APIResult<string, string>.Success(w.downloadHandler.text));
                }
            }
        }

        // Internal method for performing a POST request
        private IEnumerator performPOST(string url, string body, Dictionary<string, string> headers, Callback callback) {
            DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
            byte[] rawJSON = Encoding.UTF8.GetBytes(body);
            UploadHandlerRaw uh = new UploadHandlerRaw(rawJSON) {
                contentType = ContentType.json
            };

            using (UnityWebRequest w = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST, dh, uh)) {
                foreach (KeyValuePair<string, string> keyValuePair in headers) {
                    w.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
                }

                yield return w.SendWebRequest();

                if (w.isNetworkError || w.isHttpError) {
                    callback(APIResult<string, string>.Error(w.downloadHandler.text));
                } else if (w.responseCode == (int) HttpStatusCode.Unauthorized) {
                    // If unauthorized, refresh tokens and reattempt using new token
                    yield return refreshTokens(result => {
                        if (result.isSuccess()) {
                            headers["Authorization"] = "Bearer " + result.GetSuccess().GetAccess();
                            performPOST(url, body, headers, callback);
                        } else {
                            callback(APIResult<string, string>.Error("Invalid access & refresh token pair")); 
                        }
                    });
                } else {
                    callback(APIResult<string, string>.Success(w.downloadHandler.text));
                }
            }
        }

        // Internal method for performing a DELETE request
        private IEnumerator performDELETE(string url, Dictionary<string, string> headers, Callback callback) {
            using (UnityWebRequest w = UnityWebRequest.Delete(url)) {
                foreach (KeyValuePair<string, string> keyValuePair in headers) {
                    w.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
                }

                yield return w.SendWebRequest();

                if (w.isNetworkError || w.isHttpError) {
                    callback(APIResult<string, string>.Error(w.downloadHandler.text));
                } else if (w.responseCode == (int) HttpStatusCode.Unauthorized) {
                    // If unauthorized, refresh tokens and reattempt using new token
                    yield return refreshTokens(result => {
                        if (result.isSuccess()) {
                            headers["Authorization"] = "Bearer " + result.GetSuccess().GetAccess();
                            performDELETE(url, headers, callback);
                        } else {
                            callback(APIResult<string, string>.Error("Invalid access & refresh token pair")); 
                        }
                    });
                } else {
                    callback(APIResult<string, string>.Success(""));
                }
            }
        }

        // Internal method for refreshing access tokens
        private IEnumerator refreshTokens(RefreshCallback callback) {
            AccessToken current = GameManager.Instance().GetAccessToken();
            RequestRefreshToken request = new RequestRefreshToken(current.GetRefresh());
            string json = JsonUtility.ToJson(request);
            
            DownloadHandlerBuffer dh = new DownloadHandlerBuffer();
            byte[] rawJSON = Encoding.UTF8.GetBytes(json);
            UploadHandlerRaw uh = new UploadHandlerRaw(rawJSON) {
                contentType = ContentType.json
            };

            using (UnityWebRequest w = new UnityWebRequest(refreshURL, UnityWebRequest.kHttpVerbPOST, dh, uh)) {
                yield return w.SendWebRequest();
                if (w.isNetworkError || w.isHttpError || w.responseCode == (int) HttpStatusCode.Unauthorized) {
                    callback(APIResult<AccessToken, string>.Error("Could not refresh tokens"));
                } else {
                    string response = w.downloadHandler.text;
                    AccessToken token = JsonUtility.FromJson<AccessToken>(response);
                    GameManager.Instance().SetAccessToken(token);
                    callback(APIResult<AccessToken, string>.Success(token));
                }
            }
        }
    }
}
