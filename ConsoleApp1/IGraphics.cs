using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    internal interface IGraphics
    {
        public Punto centro { get; set; }

        public void dibujar();

        public void setCentro(Punto newCentro);

        public void rotar(Punto angulo);

        public void escalar(float factor);

        public void trasladar(Punto valorTrasladar);

        public Punto calcularCentroMasa();


    }
}
