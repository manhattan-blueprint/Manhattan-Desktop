using System;

namespace Service.Response {
	[Serializable]
	public class ResponseAuthenticate {
		public string access;
		public string refresh;
		public string account_type;
	}
}
