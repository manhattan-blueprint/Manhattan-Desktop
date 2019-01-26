public class APIResult<S, T> {
    private S success;
    private T error;

    public APIResult(S success) {
        this.success = success;
    }

    public APIResult(T error) {
        this.error = error;
    }

    public bool isSuccess() {
        return success != null;
    }

    public S GetSuccess() {
        return success;
    }

    public T GetError() {
        return error;
    }
    
    /* example
     
     if (APIResult.isSuccess) {
        UserCredentials user = ApiResult.GetSuccess();
     }
     else {
        string errorJson = ApiResult.GetError();
     }
     
     */
}
