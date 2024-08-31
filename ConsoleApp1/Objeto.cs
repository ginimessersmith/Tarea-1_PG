using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;


namespace ConsoleApp1
{
    internal class Objeto
    {
        private List<Poligono> listaDePoligonos;
        private Punto centroDeMasa;


        public Objeto()
        {
            listaDePoligonos = new List<Poligono>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f); // Inicialmente en el origen
        }

        public void AddPoligono(Poligono poligono)
        {
            listaDePoligonos.Add(poligono);
        }

        public void Dibujar()
        {
            // Aplicar la traslación al centro de masa antes de dibujar cada polígono
            GL.PushMatrix();
            GL.Translate(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);

            foreach (var poligono in listaDePoligonos)
            {
                poligono.Dibujar();
            }

            GL.PopMatrix();
        }
        public void SetCentroDeMasa(Punto nuevoCentro)
        {
            centroDeMasa = nuevoCentro;
        }
        public List<Poligono> GetPoligonos()
        {
            return listaDePoligonos;
        }
    }
}
