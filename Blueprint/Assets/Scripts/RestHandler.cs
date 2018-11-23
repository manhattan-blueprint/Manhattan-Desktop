using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class RestHandler
{
	private string _base_url;

	public RestHandler(string baseUrl)
	{
		_base_url = baseUrl;
	}

	private string GetBaseURL()
		{
			return _base_url;
		}

	public string performGET(string endpoint)
	{
		string request_url = string.Concat(GetBaseURL(), endpoint);
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(request_url));
		
		HttpWebResponse response = (HttpWebResponse) request.GetResponse();
		Stream stream = response.GetResponseStream();
		StreamReader reader = new StreamReader(stream);
	
		string str_response = reader.ReadToEnd();
			
		reader.Close();
		response.Close();
		return str_response;
	}
	
}


