using System;

namespace Model {
    
    public class Optional<T> where T : class {
        private T value;
        
        public static Optional<T> of(T value) {
            Optional<T> v = new Optional<T>();
            v.value = value;
            return v;
        }

        public static Optional<T> empty() {
            return of(null);
        }

        public bool isPresent() {
            return value != null;
        }

        public T get() {
            return value;
        }
    }
}