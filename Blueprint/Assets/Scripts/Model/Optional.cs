using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

namespace Model {
    [Serializable]
    public class Optional<T> where T : class {
        [SerializeField] private T value;
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

        public Optional<S> Map<S>(Func<T, S> f) where S : class {
            if (IsPresent()) {
                return Optional<S>.Of(f(Get()));
            }

            return Optional<S>.Empty();
        }

        public override bool Equals(object obj) {
            if (obj is Optional<T>) {
                Optional<T> other = (Optional<T>) obj;
                if (this.IsPresent() && other.IsPresent()) {
                    return this.Get() == other.Get();
                } else if (!this.IsPresent() && !other.IsPresent()) {
                    return true;
                } else {
                    return false;
                }
            }

            return false;
        }
    }

    public class WrappedInt {
        public int i;

        public WrappedInt(int i) {
            this.i = i;
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            if (this.GetType() != obj.GetType())
                return false;

            WrappedInt w = (WrappedInt) obj;
            return (this.i == w.i);
        } 
    }
}