using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class Escenario
    {

        private Dictionary<string, Objeto> listaDeObjetos;

        public Escenario()
        {
            listaDeObjetos = new Dictionary<string, Objeto>();
        }

        public void AddObjeto(string nombreObjeto, Objeto objeto)
        {
            listaDeObjetos[nombreObjeto] = objeto;
        }

        public void DibujarEscenario()
        {
            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.Dibujar();
            }
        }

        public Objeto GetObjeto(string nombreObjeto)
        {
            return listaDeObjetos.ContainsKey(nombreObjeto) ? listaDeObjetos[nombreObjeto] : null;
        }

        public bool RemoveObjeto(string nombreObjeto)
        {
            if (listaDeObjetos.ContainsKey(nombreObjeto))
            {
                listaDeObjetos.Remove(nombreObjeto);
                return true;
            }
            return false;
        }

        public void UpdateObjeto(string nombreObjeto, Objeto objetoActualizado)
        {
            if (listaDeObjetos.ContainsKey(nombreObjeto))
            {
                listaDeObjetos[nombreObjeto] = objetoActualizado;
            }
        }

        public Dictionary<string, Objeto> GetObjetos()
        {
            return listaDeObjetos;
        }
    }
}
