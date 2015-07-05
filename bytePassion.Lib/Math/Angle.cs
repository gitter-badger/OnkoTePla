using System;
using System.Globalization;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.Math
{
	public struct Angle
	{
		// TODO get culture from framework
		private static readonly IFormatProvider Numberformat = CultureInfo.CreateSpecificCulture("en-US");
		
		public static readonly Angle Zero = new Angle(0);

		private readonly double value;

		public Angle (double angle, AngleUnit representation = AngleUnit.Degree)
		{
			if (representation == AngleUnit.Radians)
				angle = (180.0/System.Math.PI)*angle;

			value = angle%360.0;

			valueAsRad = Double.NaN;
			sin = Double.NaN;
			cos = Double.NaN;
			tan = Double.NaN;
		}

		#region properties (Value/ValueAsRad/PosValue/Sin/Cos)

		public double Value { get { return value; } }

		private double valueAsRad;
		public double ValueAsRad
		{
			get { return valueAsRad = Double.IsNaN(valueAsRad) ? (System.Math.PI/180.0)*Value : valueAsRad; }
		}

		public Angle PosValue
		{
			get
			{
				return Value >= 0 ? this : new Angle(360 + Value);
			}
		}

		private double sin;
		public double Sin
		{
			get { return sin = Double.IsNaN(sin) ? System.Math.Sin(ValueAsRad) : sin; }
		}

		private double cos;
		public double Cos
		{
			get { return cos = Double.IsNaN(cos) ? System.Math.Cos(ValueAsRad) : cos; }
		}

		private double tan;
		public double Tan
		{
			get { return tan = Double.IsNaN(cos) ? System.Math.Tan(ValueAsRad) : tan; }
		}

		#endregion

		#region Equals,ToString,HashCode

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (angle1, angle2) => System.Math.Abs(angle1.PosValue.Value - angle2.PosValue.Value) < Constants.AngleEqualityPrecision);			
		}

		public override int GetHashCode ()
		{
			return Value.GetHashCode();
		}

		public override string ToString ()
		{
			return String.Format(Numberformat, "{0:0.000000} deg", Value);
		}

		#endregion

		#region operators ( > >= < <= == != + - * / ) 

		public static bool operator > (Angle a1, Angle a2)
		{
			return (a1 != a2) &&
			       (a1.Value - a2.Value) > Constants.AngleEqualityPrecision;
		}

		public static bool operator >= (Angle a1, Angle a2)
		{
			return a1 > a2 || a1 == a2;
		}

		public static bool operator < (Angle a1, Angle a2)
		{
			return !(a1 >= a2);
		}

		public static bool operator <= (Angle a1, Angle a2)
		{
			return !(a1 > a2);
		}

		public static bool operator == (Angle a1, Angle a2)
		{			
			return a1.Equals(a2);
		}

		public static bool operator != (Angle a1, Angle a2)
		{
			return !(a1 == a2);
		}

		public static Angle operator + (Angle a1, Angle a2)
		{
			return new Angle(a1.Value + a2.Value);
		}

		public static Angle operator - (Angle a1, Angle a2)
		{
			return a1 + (-a2);
		}

		public static Angle operator - (Angle a)
		{
			return new Angle(-a.Value);
		}

		public static Angle operator * (Angle a, double d)
		{
			return new Angle(a.Value*d);
		}

		public static Angle operator * (double d, Angle a)
		{
			return a*d;
		}

		public static Angle operator / (Angle a, double d)
		{
			return a*(1.0/d);
		}

		public static double operator / (Angle a1, Angle a2)
		{
			return a1.Value/a2.Value;
		}

		#endregion

		#region operations

		public static Angle MinimalAngleBetween (Angle a1, Angle a2)
		{
			var interval = new AngleInterval(a1, a2);

			return (interval.AbsolutAngleValue.Value > 180) 
				? new Angle(360 - interval.AbsolutAngleValue.Value) 
				: interval.AbsolutAngleValue;
		}

		#endregion
	}
}
