using System;
using Service.Response;

namespace Service.Request {
	public class RequestAuthenticate {
		public string username;
		public string password;

		public RequestAuthenticate(string username, string password) {
			this.username = username;
			this.password = password;
		}
	}
}
