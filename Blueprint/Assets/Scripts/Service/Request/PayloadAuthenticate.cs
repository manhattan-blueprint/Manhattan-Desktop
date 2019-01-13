using System;
using Service.Response;

namespace Service.Request {
	public class PayloadAuthenticate {
		private string username;
		private string password;

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
