using System.Collections.Generic;
using ConsoleApp1.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    internal class Objeto:IGraphics
    {
        // Cambiar listaDePartes de List a Dictionary:
        private Dictionary<string, Parte> partes;
        public Punto centro { get; set; }

        //Punto IGraphics.centro { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Objeto()
        { 
            this.centro = new Punto();
            partes = new Dictionary<string, Parte>();
        }

        public Objeto(Punto punto)
        {
            this.centro = punto;
            partes = new Dictionary<string, Parte>();
        }

        public Objeto(Punto punto, Dictionary<string, Parte>() partes)
        {
            this.centro = punto;
            this.partes = partes;
        }

        public void addParte(string key, Parte value) 
        {
            this.partes.Add(key, value);
            this.centro = calcularCentroMasa();               
        }
        public Punto calcularCentroMasa()
        {
            if (partes.Count==0)
            {
                return new Punto(0.0f,0.0f,0.0f);
            }
            else 
            {
                float ejeX = 0;
                float ejeY = 0;
                float ejeZ = 0;

                foreach (var valor in partes) 
                { 
                    Parte parte = valor.Value;
                    ejeX += parte.calcularCentroMasa().Y;
                    ejeY += parte.calcularCentroMasa().Y;
                    ejeZ += parte.calcularCentroMasa().Z;
                }

                int numPartes = partes.Count;
                float promedioEjeX = ejeX/numPartes;
                float promedioEjeY = ejeY/numPartes;
                float promedioEjeZ = ejeZ/numPartes;

                return new Punto(promedioEjeX, promedioEjeY, promedioEjeZ);
            }
        }

        public void dibujar()
        {
            foreach (var valor in partes)
            {
                Parte parte = valor.Value;
                parte.dibujar();
            }
        }

        public void escalar(float factor)
        {
            foreach(var valor in partes)
            {
                Parte parte = valor.Value;
                parte.setCentro(this.centro);
                parte.escalar(factor);
            }

            this.centro = calcularCentroMasa();
        }

        public void rotar(Punto angulo)
        {
            foreach (var valor in partes)
            {
                Parte parte = valor.Value;
                parte.setCentro(this.centro);
                parte.rotar(angulo);
            }
            this.centro = calcularCentroMasa();
        }

        public void setCentro(Punto newCentro)
        {
            foreach (var valor in partes)
            {
                Parte parte = valor.Value;
                parte.setCentro(newCentro);
            }
        }

        public void trasladar(Punto valorTrasladar)
        {
            foreach (var valor in partes)
            {
                Parte parte = valor.Value;
                parte.setCentro(this.centro);
                parte.trasladar(valorTrasladar);
            }
            this.centro = calcularCentroMasa();
        }
    }
}
