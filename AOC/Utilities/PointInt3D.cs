using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.Utilities
{
    public class PointInt3D
    {
        public PointInt3D(int x, int y, int z)
        {
            Value = (x, y, z);
        }
        public PointInt3D(int x, int y) : this(x, y, 0) { }


        protected (int x, int y, int z) Value { get; }

        public int X => Value.x;
        public int Y => Value.y;
        public int Z => Value.z;

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }

        public override bool Equals(object obj)
        {
            var other = (PointInt3D)obj;
            return this.Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public PointInt3D Abs()
        {
            return new PointInt3D(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
        }
        public PointInt3D Sign()
        {
            return new PointInt3D(Math.Sign(X), Math.Sign(Y), Math.Sign(Z));
        }
        public PointInt3D MoveOneCloserPreferDiagonal(PointInt3D other)
        {
            return this + (other - this).Sign();
        }

        public static PointInt3D operator +(PointInt3D a) => a;
        public static PointInt3D operator -(PointInt3D a) => new PointInt3D(-a.X, -a.Y, -a.Z);
        public static PointInt3D operator +(PointInt3D a, PointInt3D b) => new PointInt3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static PointInt3D operator -(PointInt3D a, PointInt3D b) => new PointInt3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static bool operator ==(PointInt3D a, PointInt3D b) => a.Value == b.Value;
        public static bool operator !=(PointInt3D a, PointInt3D b) => a.Value != b.Value;
    }
}
