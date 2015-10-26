using bytePassion.Lib.FrameworkExtensions;
using System;


namespace bytePassion.Lib.Types
{

    public abstract class ValueType<T>
    {
        protected ValueType(T value)
        {
            Value = value;
        }

        public T Value { get; }

        protected abstract Func<T,T,bool> EqualsFunc { get; }
        protected abstract string String { get; }
        
        public override bool   Equals(object obj) => this.Equals(obj, (vt1, vt2) => EqualsFunc(vt1.Value, vt2.Value));
        public override int    GetHashCode()      => Value.GetHashCode();
        public override string ToString()         => String;

        public static bool operator ==(ValueType<T> v1, ValueType<T> v2) => EqualsExtension.EqualsForEqualityOperator(v1, v2);
        public static bool operator !=(ValueType<T> v1, ValueType<T> v2) => !(v1 == v2);
    }
}