using System;
using System.Windows;


namespace bytePassion.Lib.Math
{
    public static class MathLibUtils
    {
	    
		public static Func<double, double>  GetLinearFunction(Point p1, Point p2)
		{
			var m = (p2.Y - p1.Y) / (p2.X - p1.X);
			var t = p1.Y - (m * p1.X);

			return x => m * x + t;
		}

	    public static bool DoubleEquals(double d1, double d2)
	    {
		    return System.Math.Abs(d1 - d2) < Constants.DoubleEqualityPrecision;
	    }
    }
}
