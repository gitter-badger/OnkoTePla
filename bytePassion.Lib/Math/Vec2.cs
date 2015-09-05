using System;
using System.Globalization;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Math;

// ReSharper disable once CheckNamespace


namespace funkwerk.phx.smp.map
{		
	public struct Vec2
	{
		// TODO get culture from framework
		private static readonly IFormatProvider Numberformat = CultureInfo.CreateSpecificCulture("en-US");

		public static readonly Vec2 Zero = new Vec2(0.0, 0.0);

		public Vec2(Point from, Point to) 
			: this(to.X-from.X, to.Y-from.Y)
		{			
		}

		public Vec2(Vec2 v)
			: this(v.X, v.Y)
		{			
		}

		public Vec2(double x, double y)
		{
			X = x;
			Y = y;
		}

		#region properties

		public double X { get; }
		public double Y { get; }

		public double Length        => Math.Sqrt(X*X + Y*Y);
		public double SquaredLength =>           X*X + Y*Y;

		#endregion

		#region Equals,ToString,HashCode

		public override bool Equals(object obj)
		{			
			return this.Equals(obj, (v1, v2) => MathLibUtils.DoubleEquals(v1.X, v2.X) &&
												MathLibUtils.DoubleEquals(v1.Y, v2.Y));					
		}

		public override string ToString()
		{
			return "Vec2(" + String.Format(Numberformat, "{0:0.000000}", X) +"|" + 
							 String.Format(Numberformat, "{0:0.000000}", Y) + ")";
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		#endregion

		#region operators

		public static bool operator > (Vec2 v1, Vec2 v2) => v1.Length > v2.Length;
		public static bool operator < (Vec2 v1, Vec2 v2) => v1.Length < v2.Length;
		public static bool operator >=(Vec2 v1, Vec2 v2) => (v1 == v2) || (v1 > v2);
		public static bool operator <=(Vec2 v1, Vec2 v2) => (v1 == v2) || (v1 < v2);
		public static bool operator ==(Vec2 v1, Vec2 v2) => v1.Equals(v2);
		public static bool operator !=(Vec2 v1, Vec2 v2) => !(v1 == v2);

		public static Vec2   operator +(Vec2 v1, Vec2 v2) => new Vec2(v1.X + v2.X, v1.Y + v2.Y);
		public static Vec2   operator -(Vec2 v1)          => new Vec2(-v1.X, -v1.Y);
		public static Vec2   operator -(Vec2 v1, Vec2 v2) => v1 + (-v2);
		public static double operator *(Vec2 v1, Vec2 v2) => v1.X * v2.X + v1.Y * v2.Y;
		public static Vec2   operator *(Vec2 v, double d) => new Vec2(v.X * d, v.Y * d);
		public static Vec2   operator /(Vec2 v, double d) => v * (1.0 / d);
		public static Vec2   operator *(double d, Vec2 v) => v * d;

		#endregion

		#region operations

		public Vec2 Normalize() => this / Length;

		public Vec2 ProjectTo(Vec2 v)
		{
			return (new Vec2(v)) * ((this * v) / (v.Length * v.Length));
		}

		public Angle GetAngleTo(Vec2 v)
		{			
			// TODO: test this method

			var v1 = this;
			var v2 = v;

			var a1 = new Angle(Math.Atan2(v1.Y, v1.X), AngleUnit.Radians);
			var a2 = new Angle(Math.Atan2(v2.Y, v2.X), AngleUnit.Radians);

			var res = a2 - a1;

//			if (res.Value < -180.0)
//				res += new Angle(360.0);
//
//			if (res.Value > 180.0)
//				res -= new Angle(-360.0);

			return res;
		}

		public static Angle GetAngleBetween(Vec2 v1, Vec2 v2)
		{
			return v1.GetAngleTo(v2);
		}

		#endregion
	}

}
