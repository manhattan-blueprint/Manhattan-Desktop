using System.Collections.Generic;
using Model.Reducer;
using UnityEngine;

namespace Model.Redux {
    public class StateStore<S, A> {
        private Reducer<S, A> reducer;
        private List<Subscriber<S>> subscribers;
        private S state;

        public StateStore(Reducer<S, A> reducer, S initial) {
            this.reducer = reducer;
            this.subscribers = new List<Subscriber<S>>();
            this.state = initial;
        }

        public void Dispatch(A action) {
            this.state = reducer.Reduce(state, action);
            // Create a copy of subscribers to avoid side effects of manipulating subscribers
            // during any `StateDidUpdate` calls
            foreach (Subscriber<S> subscriber in subscribers.ToArray()) {
                subscriber.StateDidUpdate(state);
            }
        }

        public void Subscribe(Subscriber<S> subscriber) {
            this.subscribers.Add(subscriber);
            // Notify subscriber of current state
            subscriber.StateDidUpdate(state);
        }
        
        public void Unsubscribe(Subscriber<S> subscriber) {
            this.subscribers.Remove(subscriber);
        }

        public S GetState() {
            return state;
        }

        public void SetState(S state) {
            this.state = state;
            // Create a copy of subscribers to avoid side effects of manipulating subscribers
            // during any `StateDidUpdate` calls
            foreach (Subscriber<S> subscriber in subscribers.ToArray()) {
                subscriber.StateDidUpdate(this.state);
            }
        }
    }
}