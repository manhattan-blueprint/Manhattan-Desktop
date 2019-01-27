using System;
using Service.Response;

namespace Service.Request {
	public class PayloadAuthenticate {
		public string username;
		public string password;

		public PayloadAuthenticate(UserCredentials user) {
			this.username = user.GetUsername();
			this.password = user.GetPassword();
		}

		public PayloadAuthenticate(string username, string password) {
			this.username = username;
			this.password = password;
		}
	}
}
