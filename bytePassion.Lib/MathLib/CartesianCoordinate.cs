﻿using bytePassion.Lib.FrameworkExtensions;
using System;


namespace bytePassion.Lib.MathLib
{
    public class CartesianCoordinate
    {
        public static readonly CartesianCoordinate Origin = new CartesianCoordinate(0, 0, 0);

        public CartesianCoordinate(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
      
        public double X { get; }
        public double Y { get; }
        public double Z { get; }             

        public override bool Equals(object obj)
        {           
            return this.Equals(obj, (c1, c2) =>  MathLibUtils.DoubleEquals(c1.X, c2.X) &&
                                                 MathLibUtils.DoubleEquals(c1.Y, c2.Y) &&
                                                 MathLibUtils.DoubleEquals(c1.Z, c2.Z));
        }

        public override string ToString()
        {
            return $"CC({MathLibUtils.DoubleFormat(X)}, {MathLibUtils.DoubleFormat(Y)}, {MathLibUtils.DoubleFormat(Z)})";
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }      

        public static bool operator ==(CartesianCoordinate v1, CartesianCoordinate v2) => EqualsExtension.EqualsForEqualityOperator(v1, v2);
        public static bool operator !=(CartesianCoordinate v1, CartesianCoordinate v2) => !(v1 == v2);

        public static CartesianCoordinate operator +(CartesianCoordinate c, Vec3 v) => new CartesianCoordinate(c.X + v.X, c.Y + v.Y, c.Z + v.Z);
        public static CartesianCoordinate operator -(CartesianCoordinate c, Vec3 v) => new CartesianCoordinate(c.X - v.X, c.Y - v.Y, c.Z - v.Z);

        public static Vec3 operator -(CartesianCoordinate c1, CartesianCoordinate c2)
        {
            return new Vec3(c1.X - c2.X, c1.Y - c2.Y, c1.Z - c2.Z);
        }

        public static explicit operator Vec3(CartesianCoordinate c)
        {
            return new Vec3(c.X, c.Y, c.Z);
        }

        public static implicit operator PolarCoordinate(CartesianCoordinate c)
        {
            var radius = Math.Sqrt(c.X * c.X + c.Y * c.Y + c.Z * c.Z);

            var theta = new Angle( (2 * Math.PI) - ((Math.Atan2(c.Z, c.X) + 2 * Math.PI) % (2 * Math.PI)), AngleUnit.Radians);
            var phi = new Angle(Math.Asin(c.Y / radius), AngleUnit.Radians);

            return new PolarCoordinate(radius, theta, phi);
        }        
    }

}
