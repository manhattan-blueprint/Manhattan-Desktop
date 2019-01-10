
namespace Model.Redux {
    public interface Subscriber<S> {
        void StateDidUpdate(S state);
    }
}