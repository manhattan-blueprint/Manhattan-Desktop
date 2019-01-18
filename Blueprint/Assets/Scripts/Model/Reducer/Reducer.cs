namespace Model.Reducer {
    public interface Reducer<S, A> {
        S Reduce(S current, A action);
    }
}