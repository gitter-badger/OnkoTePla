﻿using System;
using System.Globalization;
using System.Windows;


namespace bytePassion.Lib.GeometryLib.Utils
{
	public static class GeometryLibUtils
    {
        private static readonly IFormatProvider Numberformat = CultureInfo.CreateSpecificCulture("en-US");

        public static Func<double, double>  GetLinearFunction(Point p1, Point p2)
		{
			var m = (p2.Y - p1.Y) / (p2.X - p1.X);
			var t = p1.Y - (m * p1.X);

			return x => m * x + t;
		}

	    public static bool DoubleEquals(double d1, double d2)
	    {
		    return DoubleEquals(d1, d2, Constants.DoubleEqualityPrecision);
	    }

        public static bool DoubleEquals(double d1, double d2, double precision)
        {
            return Math.Abs(d1 - d2) < precision;
        }

        public static string DoubleFormat(double d)
        {
            return DoubleFormat(d, Constants.StandardOutputPrecision);
        }

        public static string DoubleFormat(double d, byte precision)
        {
            switch (precision)
            {
                case  1: return string.Format(Numberformat, "{0:0.0}", d);
                case  2: return string.Format(Numberformat, "{0:0.00}", d);
                case  3: return string.Format(Numberformat, "{0:0.000}", d);
                case  4: return string.Format(Numberformat, "{0:0.0000}", d);
                case  5: return string.Format(Numberformat, "{0:0.00000}", d);
                case  6: return string.Format(Numberformat, "{0:0.000000}", d);
                case  7: return string.Format(Numberformat, "{0:0.0000000}", d);
                case  8: return string.Format(Numberformat, "{0:0.00000000}", d);
                case  9: return string.Format(Numberformat, "{0:0.000000000}", d);
                case 10: return string.Format(Numberformat, "{0:0.0000000000}", d);
                case 11: return string.Format(Numberformat, "{0:0.00000000000}", d);
                case 12: return string.Format(Numberformat, "{0:0.000000000000}", d);
                case 13: return string.Format(Numberformat, "{0:0.0000000000000}", d);
                case 14: return string.Format(Numberformat, "{0:0.00000000000000}", d);
                case 15: return string.Format(Numberformat, "{0:0.000000000000000}", d);
                case 16: return string.Format(Numberformat, "{0:0.0000000000000000}", d);
                case 17: return string.Format(Numberformat, "{0:0.00000000000000000}", d);
                case 18: return string.Format(Numberformat, "{0:0.000000000000000000}", d);
                case 19: return string.Format(Numberformat, "{0:0.0000000000000000000}", d);
                case 20: return string.Format(Numberformat, "{0:0.00000000000000000000}", d);

                default: throw new ArgumentException("precision has to be between 1 and 20");
            }
            
        }
    }
}
