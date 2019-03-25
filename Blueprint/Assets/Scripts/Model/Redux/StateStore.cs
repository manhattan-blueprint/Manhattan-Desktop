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
            subscribers.ForEach(x => x.StateDidUpdate(this.state));
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

        public void ConfigureState(S state) {
            this.state = state;
        }
    }
}