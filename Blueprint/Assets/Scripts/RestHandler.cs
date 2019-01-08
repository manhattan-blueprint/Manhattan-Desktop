using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Boo.Lang;
using UnityEngine;

public class RestHandler {
    private string baseUrl;
    private string passwordRegex = "(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{5,16}";

    // Const
    private const string httpPost = "POST";
    private const string httpGet = "GET";
    private const string httpDelete = "DELETE";
    private const string JsonContentType = "application/json";
    
    // Constructor
    public RestHandler(string baseUrl) {
        this.baseUrl = baseUrl;
    }

    public bool CheckPasswordValid(string password) {
        Regex rgx = new Regex(passwordRegex);
        return rgx.IsMatch(password);
    }

    public string PerformGET(string endpoint) {
        string requestUrl = string.Concat(baseUrl, endpoint);
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(requestUrl));

        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);

        string strResponse = reader.ReadToEnd();

        reader.Close();
        response.Close();
        return strResponse;
    }

    public async Task<string> PerformAsyncGet(string endpoint, string accessToken) {
        var content = new MemoryStream();
        var webReq = (HttpWebRequest) WebRequest.Create(string.Concat(baseUrl, endpoint));

        webReq.Method = httpGet;
        webReq.ContentType = JsonContentType;
        webReq.Headers.Add("Authorization", "Bearer " + accessToken);

        using (WebResponse response = await webReq.GetResponseAsync()) {
            using (Stream responseStream = response.GetResponseStream()) {
                await responseStream.CopyToAsync(content);
            }
        }

        return Encoding.Default.GetString(content.ToArray());  
    }

    public async Task<string> PerformAsyncPost(string endpoint, string postData) {
        var content = new MemoryStream();
        var webReq = (HttpWebRequest) WebRequest.Create(string.Concat(baseUrl, endpoint));
        byte[] payload = Encoding.ASCII.GetBytes(postData);

        webReq.Method = httpPost;
        webReq.ContentType = JsonContentType;
        webReq.ContentLength = payload.Length;

        using (var stream = webReq.GetRequestStream()) {
            stream.Write(payload, 0, payload.Length);
        }

        using (WebResponse response = await webReq.GetResponseAsync()) {
            using (Stream responseStream = response.GetResponseStream()) {
                await responseStream.CopyToAsync(content);
            }
        }

        return Encoding.Default.GetString(content.ToArray());
    }

    public async Task<string> PerformAsyncPost(string endpoint, string postData, string accessToken) {
        var content = new MemoryStream();
        var webReq = (HttpWebRequest) WebRequest.Create(string.Concat(baseUrl, endpoint));
        byte[] payload = Encoding.ASCII.GetBytes(postData);

        webReq.Method = httpPost;
        webReq.ContentType = JsonContentType;
        webReq.ContentLength = payload.Length;
        webReq.Headers.Add("Authorization", "Bearer " + accessToken);

        using (var stream = webReq.GetRequestStream()) {
            stream.Write(payload, 0, payload.Length);
        }

        using (WebResponse response = await webReq.GetResponseAsync()) {
            using (Stream responseStream = response.GetResponseStream()) {
                await responseStream.CopyToAsync(content);
            }
        }

        return Encoding.Default.GetString(content.ToArray());
    }

    public async Task<string> PerformAsyncDelete(string endpoint, string accessToken) {
        var content = new MemoryStream();
        var webReq = (HttpWebRequest) WebRequest.Create(string.Concat(baseUrl, endpoint));

        webReq.Method = httpDelete;
        webReq.ContentType = JsonContentType;
        webReq.Headers.Add("Authorization", "Bearer " + accessToken);

        using (WebResponse response = await webReq.GetResponseAsync()) {
            using (Stream responseStream = response.GetResponseStream()) {
                await responseStream.CopyToAsync(content);
            }
        }

        return Encoding.Default.GetString(content.ToArray());  
    }

}
