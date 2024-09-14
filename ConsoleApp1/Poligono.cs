using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    internal class Poligono
    {
       //public Punto ini { get; set; } = new Punto();
        private List<Punto> puntos;
        private float[] color;
        private Punto centroDeMasa;
        //public PrimitiveType primitiveType;

        public Poligono(List<Punto> puntos, float r, float g, float b)
        {
            this.puntos = new List<Punto>(puntos);
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f);
            this.color = new float[] { r, g, b };
         
        }

        public void Dibujar()
        {
            GL.Begin(PrimitiveType.Polygon); 
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

        public float[] GetColor()
        {
            return color;
        }

        public void AjustarCentroDeMasa(Punto nuevoCentro)
        {
            for (int i = 0; i < puntos.Count; i++)
            {
                puntos[i] = new Punto(
                    puntos[i].X + nuevoCentro.X,
                    puntos[i].Y + nuevoCentro.Y,
                    puntos[i].Z + nuevoCentro.Z
                );
            }
        }

        public void ApplyTransformation(Matrix4 transformation)
        {
            for (int i = 0; i < puntos.Count; i++)
            {
                puntos[i] = TransformPoint(puntos[i], transformation);
            }
        }

        private Punto TransformPoint(Punto punto, Matrix4 transformation)
        {
            Vector4 transformed = Vector4.Transform(new Vector4(punto.X, punto.Y, punto.Z, 1.0f), transformation);
            return new Punto(transformed.X, transformed.Y, transformed.Z);
        }

    }
}
