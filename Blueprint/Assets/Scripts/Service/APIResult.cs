/* example
 
 if (APIResult.isSuccess()) {
    UserCredentials user = ApiResult.GetSuccess();
 } else {
    string errorJson = ApiResult.GetError();
 }
 
 */
 
namespace Service {
    public class APIResult<S, T> {
        private S success;
        private T error;
        
        public static APIResult<S, T> Success(S value) {
            return new APIResult<S, T> { success = value };
        }
        
        public static APIResult<S, T> Error(T value) {
            return new APIResult<S, T> { error = value };
        }

        private APIResult() { }
        
        public bool isSuccess() {
            return success != null;
        }

        public S GetSuccess() {
            return success;
        }

        public T GetError() {
            return error;
        }
    }
}
