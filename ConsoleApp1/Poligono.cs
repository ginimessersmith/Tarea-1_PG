using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    internal class Poligono
    {
        private List<Punto> puntos;
        private float[] color; // Arreglo para almacenar los valores RGB
        //public PrimitiveType primitiveType;

        public Poligono(List<Punto> puntos, float r, float g, float b)
        {
            this.puntos = puntos;
            this.color = new float[] { r, g, b };
            //this.primitiveType.LineLoop
        }

        public void Dibujar()
        {
            GL.Begin(PrimitiveType.Quads); // Cambiado de Polygon a Quads
            GL.Color3(color[0], color[1], color[2]);

            foreach (var punto in puntos)
            {
                GL.Vertex3(punto.X, punto.Y, punto.Z);
            }

            GL.End();
        }

        public List<Punto> GetPuntos()
        {
            return puntos;
        }
    }
}
