using bytePassion.Lib.MathLib;
using bytePassion.Lib.Types;
using System;


namespace bytePassion.Lib.GeometryLib.Base
{

    public class Radians : ValueType<double>
    {
        public Radians(double value)
            : base(value)
        {            
        }

        protected override Func<double, double, bool> EqualsFunc => GeometryLibUtils.DoubleEquals;
        protected override string String => $"{GeometryLibUtils.DoubleFormat(Value)} rad";
    }
}