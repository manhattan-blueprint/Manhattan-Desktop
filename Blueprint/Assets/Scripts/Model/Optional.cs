using System;

namespace Model {
    
    public class Optional<T> where T : class {
        private T value;
        private Optional() { }
        
        public static Optional<T> Of(T value) {
            Optional<T> v = new Optional<T>();
            v.value = value;
            return v;
        }

        public static Optional<T> Empty() {
            return Of(null);
        }

        public bool IsPresent() {
            return value != null;
        }

        public T Get() {
            return value;
        }
    }
}