using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Gravity {
    internal class Vector {
        public double x, y;
        public Vector(double x, double y) {
            this.x = x;
            this.y = y;
        }
        public Vector(Point p) { 
            this.x = p.X;
            this.y = p.Y;
        }

        public double Length() => Math.Sqrt(x * x + y * y);

        public static implicit operator Point(Vector a) => new Point((int)Math.Round(a.x), (int)Math.Round(a.y));

        public static Vector operator +(Vector a, Vector b) => new Vector(a.x + b.x, a.y + b.y);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.x - b.x, a.y - b.y);
        public static Vector operator /(Vector a, double d) => new Vector(a.x / d, a.y / d);
        public static Vector operator *(Vector a, double d) => new Vector(a.x * d, a.y * d);
    }
}
