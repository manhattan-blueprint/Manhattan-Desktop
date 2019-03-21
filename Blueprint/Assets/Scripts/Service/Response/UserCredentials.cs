using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Service.Response {
    [Serializable]
    public class UserCredentials {
        private string username;
        private string password;

        // Server tokens
        private string access;
        private string refresh;
        private string accountType;

        public UserCredentials(string username, string password) {
            this.username = username;
            this.password = password;
        }

        public UserCredentials(string username, string password, string access, string refresh, string accountType) {
            this.username = username;
            this.password = password;
            this.access = access;
            this.refresh = refresh;
            this.accountType = accountType;
        }
        
        // Getters
        public string GetUsername() {
            return username;
        }
        
        public string GetPassword() {
            return password;
        }
        
        public string GetAccessToken() {
            return access;
        }
        
        public string GetRefreshToken() {
            return refresh;
        }
        
        public string GetAccountType() {
            return accountType;
        }
    }
}
