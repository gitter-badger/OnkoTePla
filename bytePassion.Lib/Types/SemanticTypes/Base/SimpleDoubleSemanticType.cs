using bytePassion.Lib.MathLib;
using System;


namespace bytePassion.Lib.Types.SemanticTypes.Base
{
    public abstract class SimpleDoubleSemanticType : SemanticType<double>
    {        
        protected SimpleDoubleSemanticType(double value, string unit = "")
            : base(value, unit)
        {            
        }
        
        protected override Func<double, double, bool> EqualsFunc => GeometryLibUtils.DoubleEquals;
        protected override string String => $"{GeometryLibUtils.DoubleFormat(Value)}";    
    }
}
