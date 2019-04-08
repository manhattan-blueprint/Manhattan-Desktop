using System;
using Service.Response;

namespace Service.Request {
	[Serializable]
	public class RequestRefreshToken {
		public string refresh_token;

		public RequestRefreshToken(string refresh) {
			this.refresh_token = refresh;
		}
	}
}
