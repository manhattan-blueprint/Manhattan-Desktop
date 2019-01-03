using System;

[Serializable]
public class PayloadAuthenticate {
	public string username;
	public string password;

	public PayloadAuthenticate(UserCredentials user) {
		this.username = user.getUsername();
		this.password = user.getPassword();
	}

	public PayloadAuthenticate(string username, string password) {
		this.username = username;
		this.password = password;
	}
}
