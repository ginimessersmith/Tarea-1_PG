using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class ListaPuntos
    {
        private List<Punto> listaDePuntos;

        public ListaPuntos()
        {
            listaDePuntos = new List<Punto>();
        }

        public void AddPunto(Punto punto)
        {
            listaDePuntos.Add(punto);
        }

        public void RemovePunto(int index)
        {
            if(index >= 0 && index < listaDePuntos.Count)
            {
                listaDePuntos.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Índice fuera de rango");
            }
        }

        public List<Punto> GetPuntos()
        {
            return listaDePuntos;
        }
    }
}
