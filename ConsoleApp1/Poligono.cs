using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    internal class Poligono:IGraphics
    {
 
        private List<Punto> puntos;
        private float[] color;
        public Punto centro { get; set; } = new Punto();
        public PrimitiveType primitiveType { get; set; } = new PrimitiveType();


        public Poligono(List<Punto> puntos, float r, float g, float b)
        {
            this.puntos = new List<Punto>(puntos);
            this.centro = new Punto();
            this.color = new float[] { r, g, b };
         
        }

        public Poligono(Punto centro)
        {
            this.centro = centro;
            this.puntos = new List<Punto>();
            this.primitiveType = PrimitiveType.LineLoop;

        }

        public Poligono() 
        {
            this.centro = new Punto();
            this.puntos = new List<Punto>();
            this.primitiveType = PrimitiveType.LineLoop;
            this.color = new float[] { 0.1f, 0.1f, 0.1f };
        }

        public List<Punto> GetPuntos()
        {
            return puntos;
        }

        public float[] GetColor()
        {
            return color;
        }

        public void dibujar()
        {
            GL.Color4(this.color);
            GL.Begin(PrimitiveType.LineLoop);
            foreach (var punto in puntos) {
                GL.Vertex3(punto.X, punto.Y, punto.Z);
            }

            GL.End();
            GL.Flush();
        }

        public void setCentro(Punto newCentro)
        {
           this.centro = newCentro;
        }

        public Punto getCentro() 
        {
            return this.calcularCentroMasa();
        }

        public void rotar(Punto angulo)
        {
            Punto centro = this.centro;

            Matrix4 matrixX = Matrix4.CreateRotationX(MathHelper.RadiansToDegrees(angulo.X));
            Matrix4 matrixY = Matrix4.CreateRotationY(MathHelper.RadiansToDegrees(angulo.Y));
            Matrix4 matrixZ = Matrix4.CreateRotationZ(MathHelper.RadiansToDegrees(angulo.Z));

            Matrix4 rotacion = matrixZ * matrixY * matrixX ;

            foreach (var punto in puntos)
            { 
                Vector4 vector = new Vector4(punto.X - this.centro.X, punto.Y - this.centro.Y, punto.Z - this.centro.Z,1f);

                Vector4 resultado = rotacion * vector;
                
                punto.X = resultado.X + this.centro.X;
                punto.Y = resultado.Y + this.centro.Y;
                punto.Z = resultado.Z + this.centro.Z;

                Punto nuevoCentro = this.calcularCentroMasa();
                this.setCentro(nuevoCentro);

            }

            

        }

        public void escalar(float factor)
        {
            Matrix4 load = Matrix4.CreateScale(factor);
            Vector4 resultado;
            foreach (var punto in puntos) 
            { 
                Vector4 vector = new Vector4(
                    punto.X - this.centro.X,
                    punto.Y - this.centro.Y,
                    punto.Z - this.centro.Z,
                    1);             
                resultado = vector * load;
                punto.X = resultado.X + this.centro.X;
                punto .Y = resultado.Y + this.centro.Y; 
                punto.Z = resultado.Z + this.centro.Z;

                Punto nuevoCentro = this.calcularCentroMasa();
                this.setCentro(nuevoCentro);
            }

          

        }

        public void trasladar(Punto valorTrasladar)
        {
            Matrix4 load = Matrix4.CreateTranslation(valorTrasladar.X,valorTrasladar.Y, valorTrasladar.Z);
            Vector4 resultado;
            foreach (var punto in puntos) 
            {
                Vector4 vector = new Vector4(
                    punto.X,
                    punto.Y,
                    punto.Z,
                    1
                    );
                resultado = vector * load;
                punto.X = resultado.X;
                punto.Y = resultado.Y;
                punto.Z = resultado.Z;

                Punto nuevoCentro = this.calcularCentroMasa();
                this.setCentro(nuevoCentro);
            }

            
        }

        public Punto calcularCentroMasa()
        {
            if (puntos.Count == 0)
            {
                return new Punto(0,0,0);
            }
            else 
            {
                float minX = puntos.Min(p=>p.X);
                float maxX = puntos.Max(p => p.X);

                float minY = puntos.Min(p=>p.Y);
                float maxY = puntos.Max(p =>p.Y);
                
                float minZ = puntos.Min(p=> p.Z);
                float maxZ = puntos.Max(p=>p.Z);

                float nuevoCentroX = (minX + maxX) / 2;
                float nuevoCentroY = (minY + maxY) / 2;
                float nuevoCentroZ = (minZ + maxZ) / 2;

                return new Punto(nuevoCentroX,nuevoCentroY,nuevoCentroZ);
            }
        }
    }
}
