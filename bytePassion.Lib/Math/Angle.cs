using System;
using System.Globalization;
using bytePassion.Lib.FrameworkExtensions;

using SysMath = System.Math;

namespace bytePassion.Lib.Math
{
	public struct Angle
	{
		// TODO get culture from framework
		private static readonly IFormatProvider Numberformat = CultureInfo.CreateSpecificCulture("en-US");

		public static readonly Angle Zero = new Angle(0);

		private readonly double value;

		private double valueAsRad;		//
		private double sin;				//	variables to avoid
		private double cos;				//  redundant computation
		private double tan;				//

		public Angle (double angle, AngleUnit representation = AngleUnit.Degree)
		{
			if (representation == AngleUnit.Radians)
				angle = (180.0/SysMath.PI)*angle;

			value = angle%360.0;

			valueAsRad = Double.NaN;
			sin = Double.NaN;
			cos = Double.NaN;
			tan = Double.NaN;
		}

		#region properties (Value/ValueAsRad/PosValue/Sin/Cos)

		public double Value      { get { return value; } }
		public double ValueAsRad { get { return valueAsRad = Double.IsNaN(valueAsRad) ? (SysMath.PI/180.0)*Value : valueAsRad; } }

		public Angle PosValue { get { return Value >= 0 ? this : new Angle(360 + Value); } }
		public Angle Inverted { get { return new Angle(-Value); } }

		public double Sin { get { return sin = Double.IsNaN(sin) ? SysMath.Sin(ValueAsRad) : sin; } }
		public double Cos { get { return cos = Double.IsNaN(cos) ? SysMath.Cos(ValueAsRad) : cos; } }
		public double Tan { get { return tan = Double.IsNaN(cos) ? SysMath.Tan(ValueAsRad) : tan; } }

		#endregion

		#region Equals,ToString,HashCode

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (angle1, angle2) => MathLibUtils.DoubleEquals(angle1.PosValue.Value, angle2.PosValue.Value));
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

		public static bool operator >  (Angle a1, Angle a2) { return a1.Value > a2.Value; }
		public static bool operator >= (Angle a1, Angle a2) { return a1 > a2 || a1 == a2; }
		public static bool operator <  (Angle a1, Angle a2) { return !(a1 >= a2);         }
		public static bool operator <= (Angle a1, Angle a2) { return !(a1 > a2);          }
		public static bool operator == (Angle a1, Angle a2) { return a1.Equals(a2);       }
		public static bool operator != (Angle a1, Angle a2) { return !(a1 == a2);         }

		public static Angle  operator + (Angle a1, Angle a2) { return new Angle(a1.Value + a2.Value); }
		public static Angle  operator - (Angle a1, Angle a2) { return a1 + (-a2);                     }
		public static Angle  operator - (Angle a)            { return new Angle(-a.Value);            }
		public static Angle  operator * (Angle a, double d)  { return new Angle(a.Value*d);           }
		public static Angle  operator * (double d, Angle a)  { return a*d;                            }
		public static Angle  operator / (Angle a, double d)  { return a*(1.0/d);                      }
		public static double operator / (Angle a1, Angle a2) { return a1.Value/a2.Value;              }

		#endregion

		#region operations (MinimalAngleBetween)

		public static Angle MinimalAngleBetween (Angle a1, Angle a2)
		{
			var interval = new AngleInterval(a1, a2);

			return (interval.AbsolutAngleValue.Value > 180) 
				? new Angle(360 - interval.AbsolutAngleValue.Value) 
				: interval.AbsolutAngleValue;
		}

		#endregion

		#region static: Parse

		public static Angle Parse (string s)
		{
			var parts = s.Split(' ');

			if (parts.Length != 2)
				throw new ArgumentException(s + " cannot be converted to an Angle");

			var number = Double.Parse(parts[0].Trim(), Numberformat);
			var unit = parts[1].Trim() == "deg" ? AngleUnit.Degree : AngleUnit.Radians;

			return new Angle(number, unit);
		}

		#endregion
	}
}
