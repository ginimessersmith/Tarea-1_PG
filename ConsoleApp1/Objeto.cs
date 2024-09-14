using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    internal class Objeto
    {
        // Cambiar listaDePartes de List a Dictionary
        private Dictionary<string, Parte> listaDePartes;
        private Punto centroDeMasa;
        private float anguloRotacion;
        private float ejeX, ejeY, ejeZ;
        private Matrix4 transformationMatrix;
        private Vector3 ejeRotacion;


        public Objeto()
        {
    
            listaDePartes = new Dictionary<string, Parte>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f); // Inicialmente en el origen
            anguloRotacion = 0.0f;
            ejeX = 0.0f;
            ejeY = 0.0f;
            ejeZ = 0.0f;
            transformationMatrix = Matrix4.Identity;
            ejeRotacion = Vector3.UnitY;
        }

        // Cambiar AddParte para recibir un nombre de parte
        public void AddParte(string nombreParte, Parte parte)
        {
            listaDePartes[nombreParte] = parte; // Añadir o actualizar la parte
        }

        public void Rotar(Vector3 axis, float angleInDegrees)
        {
            ejeRotacion = axis;
            anguloRotacion += angleInDegrees;
        }

        public void Dibujar()
        {
            GL.PushMatrix();

            // Mover al centro de masa
            GL.Translate(-centroDeMasa.X, -centroDeMasa.Y, -centroDeMasa.Z);

            // Aplicar la rotación
            GL.Rotate(anguloRotacion, ejeRotacion.X, ejeRotacion.Y, ejeRotacion.Z);

            // Dibujar cada parte
            foreach (var parte in listaDePartes.Values)
            {
                parte.Dibujar();
            }

            GL.PopMatrix();
        }

        public void SetCentroDeMasa(Punto nuevoCentro)
        {
            centroDeMasa = nuevoCentro;
            AjustarCentroDeMasa();
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
        private void AjustarCentroDeMasa()
        {
            foreach (var parte in listaDePartes.Values)
            {
                parte.AjustarCentroDeMasa(centroDeMasa);
            }
        }

        public Punto GetCentroDeMasa()
        {
            return centroDeMasa;
        }

        public void setListaPartes(Dictionary<string, Parte> nuevaListaDePartes) { 
        this.listaDePartes = nuevaListaDePartes;
        }
    }
}
