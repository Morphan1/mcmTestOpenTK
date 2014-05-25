using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared
{
    public struct Location: IEquatable<Location>
    {
        /// <summary>
        /// A Location of (0, 0, 0).
        /// </summary>
        public static Location Zero { get { return new Location(0); } }

        /// <summary>
        /// A Location of (1, 1, 1).
        /// </summary>
        public static Location One { get { return new Location(1); } }

        /// <summary>
        /// A location of (1, 0, 0).
        /// </summary>
        public static Location UnitX { get { return new Location(1, 0, 0); } }

        /// <summary>
        /// A location of (0, 1, 0).
        /// </summary>
        public static Location UnitY { get { return new Location(0, 1, 0); } }

        /// <summary>
        /// A location of (0, 0, 1).
        /// </summary>
        public static Location UnitZ { get { return new Location(0, 0, 1); } }

        /// <summary>
        /// The X coordinate of this location.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y coordinate of this location.
        /// </summary>
        public float Y;

        /// <summary>
        /// The Z coordinate of this location.
        /// </summary>
        public float Z;

        public Location(float _X, float _Y, float _Z)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
        }

        public Location(float _Point)
        {
            X = _Point;
            Y = _Point;
            Z = _Point;
        }

        /// <summary>
        /// Returns the full linear length of the vector location, squared for efficiency.
        /// </summary>
        /// <returns>The squared length</returns>
        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Returns the full linear length of the vector location, which goes through a square-root operation (inefficient).
        /// </summary>
        /// <returns>The square-rooted length</returns>
        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        /// <summary>
        /// Returns the location as a string in the form: X, Y, Z
        /// Inverts .FromString()
        /// </summary>
        /// <returns>The location string</returns>
        public string ToSimpleString()
        {
            return X + ", " + Y + ", " + Z;
        }

        /// <summary>
        /// Converts the Location to a simple byte[] representation.
        /// Inverts .FromBytes()
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            byte[] toret = new byte[12];
            BitConverter.GetBytes(X).CopyTo(toret, 0);
            BitConverter.GetBytes(Y).CopyTo(toret, 4);
            BitConverter.GetBytes(Z).CopyTo(toret, 8);
            return toret;
        }

        public override bool Equals(object obj)
        {
            return (obj is Location) && this == (Location)obj;
        }

        public bool Equals(Location v)
        {
            return this == v;
        }

        public static bool operator ==(Location v1, Location v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Location v1, Location v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
        }

        public override int GetHashCode()
        {
            return (int)(X + Y + Z);
        }

        public static Location operator +(Location v1, Location v2)
        {
            return new Location(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Location operator -(Location v)
        {
            return new Location(-v.X, -v.Y, -v.Z);
        }

        public static Location operator -(Location v1, Location v2)
        {
            return new Location(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Location operator *(Location v, float scale)
        {
            return new Location(v.X * scale, v.Y * scale, v.Z * scale);
        }

        public static Location operator *(float scale, Location v)
        {
            return new Location(v.X * scale, v.Y * scale, v.Z * scale);
        }

        public static Location operator /(Location v, float scale)
        {
            return new Location(v.X / scale, v.Y / scale, v.Z / scale);
        }

        /// <summary>
        /// Converts a string representation of a location to a Location object.
        /// Inverts .ToString(), .ToSimpleString()
        /// </summary>
        /// <param name="input">The location string</param>
        /// <returns>the location object</returns>
        public static Location FromString(string input)
        {
            string[] data = input.Replace('(',' ').Replace(')', ' ').Replace(" ", "").Split(',');
            if (data.Length != 3)
            {
                return new Location(0);
            }
            return new Location(Utilities.StringToFloat(data[0]), Utilities.StringToFloat(data[1]), Utilities.StringToFloat(data[2]));
        }

        /// <summary>
        /// Reads the byte array to a Location object.
        /// Inverts .ToBytes()
        /// </summary>
        /// <param name="bytes">The bytes to read</param>
        /// <param name="index">The index to start at</param>
        /// <returns>the location object</returns>
        public static Location FromBytes(byte[] bytes, int index)
        {
            if (bytes.Length - index < 12)
            {
                return new Location(0);
            }
            float X = BitConverter.ToSingle(bytes, index);
            float Y = BitConverter.ToSingle(bytes, index + 4);
            float Z = BitConverter.ToSingle(bytes, index + 8);
            return new Location(X, Y, Z);
        }
    }
}
