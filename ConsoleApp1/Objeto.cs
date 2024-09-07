using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    internal class Objeto
    {
        // Cambiar listaDePartes de List a Dictionary
        private Dictionary<string, Parte> listaDePartes;
        private Punto centroDeMasa;

        public Objeto()
        {
            // Inicializar el diccionario en lugar de la lista
            listaDePartes = new Dictionary<string, Parte>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f); // Inicialmente en el origen
        }

        // Cambiar AddParte para recibir un nombre de parte
        public void AddParte(string nombreParte, Parte parte)
        {
            listaDePartes[nombreParte] = parte; // Añadir o actualizar la parte
        }

        public void Dibujar()
        {
            // Aplicar la traslación al centro de masa antes de dibujar cada parte
            GL.PushMatrix();
            GL.Translate(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);

            foreach (var parte in listaDePartes.Values)
            {
                parte.Dibujar();
            }

            GL.PopMatrix();
        }

        public void SetCentroDeMasa(Punto nuevoCentro)
        {
            centroDeMasa = nuevoCentro;
        }

        // Obtener una parte por su nombre
        public Parte GetParte(string nombreParte)
        {
            return listaDePartes.ContainsKey(nombreParte) ? listaDePartes[nombreParte] : null;
        }

        // Eliminar una parte por su nombre (Delete)
        public bool RemoveParte(string nombreParte)
        {
            if (listaDePartes.ContainsKey(nombreParte))
            {
                listaDePartes.Remove(nombreParte);
                return true;
            }
            return false;
        }

        // Obtener todas las partes
        public Dictionary<string, Parte> GetPartes()
        {
            return listaDePartes;
        }
    }
}
