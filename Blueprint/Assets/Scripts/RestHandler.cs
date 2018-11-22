using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class RestHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print("Hello");
		performGET("http://adamcfox.ddns.net:8000");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public string performGET(string url)
	{
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(url));
		HttpWebResponse response = (HttpWebResponse) request.GetResponse();

//		print(response.StatusDescription);
		Stream stream = response.GetResponseStream();
		StreamReader reader = new StreamReader(stream);

		string str_response = reader.ReadToEnd();
		print(str_response);
		
		reader.Close();
		response.Close();
		return str_response;
	}
}


