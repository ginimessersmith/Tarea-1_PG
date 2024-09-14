using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Punto
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Punto(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Punto()
        {
            X = Y = Z = 0.0f;
        }
        public Punto(float v)
        {
            X = Y = Z = v;
        }
    
        public Punto(Punto p)
        {
            X = p.X; Y = p.Y; Z = p.Z;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
