    using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1
    {
        internal class Parte:IGraphics
        {
        public Dictionary<string, Poligono> poligonos { get; set; }

        public Punto centro { get; set; }

        public Parte()
        { 
            this.centro = new Punto(0.0f, 0.0f, 0.0f); // Inicialmente en el origen
            poligonos = new Dictionary<string, Poligono>();
        }

        public Parte(Punto centro)
        {
            this.centro = centro;
            poligonos = new Dictionary<string, Poligono>();
        }

        public void addPoligono(string key, Poligono poligono)
        { 
            poligonos.Add(key, poligono);
            this.centro = this.calcularCentroMasa();
        }

        public void dibujar()
        {
            foreach (var valor in poligonos) 
            { 
                Poligono poligono = valor.Value;
                poligono.dibujar();
            }
        }

        public void setCentro(Punto newCentro)
        {
            foreach (var valor in poligonos) 
            {
                Poligono poligono = valor.Value;
                poligono.setCentro(newCentro);
            }
        }

        public Punto getCentro() 
        { 
            return this.centro; 
        }
        public void rotar(Punto angulo)
        {
            foreach (var valor in poligonos)
            {
                Poligono poligono = valor.Value;
                poligono.rotar(angulo);
            }
           // this.centro = calcularCentroMasa();
        }

        public void escalar(float factor)
        {
           foreach (var valor in poligonos)
            {
                Poligono poligono = valor.Value;
                poligono.setCentro(this.centro);
                poligono.escalar(factor);
            }
            this.centro = calcularCentroMasa();
        }

        public void trasladar(Punto valorTrasladar)
        {
            foreach (var valor in poligonos)
            {
                Poligono poligono = valor.Value;
                poligono.setCentro(this.centro);
                poligono.trasladar(valorTrasladar);
            }

            this.centro = calcularCentroMasa();
        }

        public Punto calcularCentroMasa()
        {
            if (poligonos.Count == 0) {
                return new Punto(0.0f, 0.0f, 0.0f);
            }
            else { 

                float ejeX = 0.0f;
                float ejeY = 0.0f;
                float ejeZ = 0.0f;

                foreach (var valor in poligonos) 
                {
                    string key = valor.Key;
                    Poligono poligono = valor.Value;
                    
                    Punto centroPoligono = poligono.calcularCentroMasa();

                    ejeX += centroPoligono.X;
                    ejeY += centroPoligono.Y;
                    ejeZ += centroPoligono.Z;
                }

                int numeroPoligonos = poligonos.Count;
                float promedioEjeX = ejeX / numeroPoligonos;
                float promedioEjeY = ejeY / numeroPoligonos;
                float promedioEjeZ = ejeZ / numeroPoligonos;

                return new Punto(promedioEjeX, promedioEjeY, promedioEjeZ);
            }
        }
    }
    }
