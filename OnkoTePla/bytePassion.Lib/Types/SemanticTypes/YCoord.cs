﻿using bytePassion.Lib.Types.SemanticTypes.Base;


namespace bytePassion.Lib.Types.SemanticTypes
{

    public class YCoord : SimpleDoubleSemanticType
    {
        public YCoord(double value)
            : base(value)
        {
        }

        public static YCoord operator +(YCoord y1, YCoord y2) => new YCoord(y1.Value + y2.Value);
        public static YCoord operator -(YCoord y1, YCoord y2) => new YCoord(y1.Value - y2.Value);
        public static YCoord operator *(YCoord y1, YCoord y2) => new YCoord(y1.Value * y2.Value);
        public static YCoord operator /(YCoord y1, YCoord y2) => new YCoord(y1.Value / y2.Value);
        public static YCoord operator -(YCoord y)             => new YCoord(-y.Value);
    }
}