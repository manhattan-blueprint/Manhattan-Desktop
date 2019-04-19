using System;
using UnityEngine;

namespace Service.Response {
    [Serializable]
    public class AccessToken {
        [SerializeField] private string access; 
        [SerializeField] private string refresh;
        [SerializeField] private string account_type;

        public string GetAccess() {
            return access;
        }

        public string GetRefresh() {
            return refresh;
        }
    }
}

