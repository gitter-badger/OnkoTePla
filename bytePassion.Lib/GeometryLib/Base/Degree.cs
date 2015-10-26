using bytePassion.Lib.Types;
using System;
using static bytePassion.Lib.MathLib.GeometryLibUtils;


namespace bytePassion.Lib.GeometryLib.Base
{
    public class Degree : ValueType<double>
    {        
        public Degree(double value)
            : base(value)
        {            
        }

        protected override Func<double, double, bool> EqualsFunc => DoubleEquals;
        protected override string String => $"{DoubleFormat(Value)} deg";
    }
}
