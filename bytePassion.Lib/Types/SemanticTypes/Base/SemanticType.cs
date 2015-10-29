using bytePassion.Lib.FrameworkExtensions;
using System;


namespace bytePassion.Lib.Types.SemanticTypes.Base
{

    public abstract class SemanticType<T>
    {
        protected SemanticType(T value, string unit = "")
        {
            Value = value;
            Unit = unit;
        }

        public T Value { get; }
        protected string Unit { get; }

        protected abstract Func<T, T, bool> EqualsFunc { get; }
        protected abstract string String { get; }

        public override bool Equals(object obj) => this.Equals(obj, (vt1, vt2) => EqualsFunc(vt1.Value, vt2.Value));
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => String.IsNullOrEmpty(Unit) ? String : String + " " + Unit;

        public static bool operator ==(SemanticType<T> v1, SemanticType<T> v2) => EqualsExtension.EqualsForEqualityOperator(v1, v2);
        public static bool operator !=(SemanticType<T> v1, SemanticType<T> v2) => !(v1 == v2);
    }
}