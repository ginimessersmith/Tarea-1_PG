using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    internal class AdministradorObjetos
    {
        public void GuardarObjetos(Escenario escenario, string archivo)
        {
          
            EscenarioData escenarioData = ConvertirEscenarioAData(escenario);

            string jsonData = JsonSerializer.Serialize(escenarioData, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(archivo, jsonData);
        }

        // Método para cargar los objetos desde un archivo JSON
        public Escenario CargarObjetos(string archivo)
        {
       
            string jsonData = File.ReadAllText(archivo);
     
            EscenarioData escenarioData = JsonSerializer.Deserialize<EscenarioData>(jsonData);

            return ConvertirDataAEscenario(escenarioData);
        }

        private EscenarioData ConvertirEscenarioAData(Escenario escenario)
        {
            EscenarioData escenarioData = new EscenarioData();
            escenarioData.Objetos = new Dictionary<string, ObjetoData>();

            foreach (var kvp in escenario.GetObjetos())
            {
                string nombreObjeto = kvp.Key;
                var objeto = kvp.Value;

                ObjetoData objetoData = new ObjetoData();
                objetoData.Partes = new Dictionary<string, ParteData>();

                foreach (var kvpParte in objeto.GetPartes())
                {
                    string nombreParte = kvpParte.Key;
                    var parte = kvpParte.Value;

                    ParteData parteData = new ParteData();
                    parteData.Poligonos = new List<PoligonoData>();

                    foreach (var poligono in parte.GetPoligonos())
                    {
                        PoligonoData poligonoData = new PoligonoData();
                        poligonoData.Color = poligono.GetColor(); 
                        poligonoData.Puntos = new List<float[]>();

                        foreach (var punto in poligono.GetPuntos())
                        {
                            poligonoData.Puntos.Add(new float[] { punto.X, punto.Y, punto.Z });
                        }

                        parteData.Poligonos.Add(poligonoData);
                    }

                    objetoData.Partes[nombreParte] = parteData;
                }

                escenarioData.Objetos[nombreObjeto] = objetoData;
            }

            return escenarioData;
        }

        private Escenario ConvertirDataAEscenario(EscenarioData escenarioData)
        {
            Escenario escenario = new Escenario();

            foreach (var kvpObjeto in escenarioData.Objetos)
            {
                string nombreObjeto = kvpObjeto.Key;
                var objetoData = kvpObjeto.Value;

                Objeto objeto = new Objeto();

                foreach (var kvpParte in objetoData.Partes)
                {
                    string nombreParte = kvpParte.Key;
                    var parteData = kvpParte.Value;

                    Parte parte = new Parte();

                    foreach (var poligonoData in parteData.Poligonos)
                    {
                        List<Punto> puntos = new List<Punto>();
                        foreach (var puntoArray in poligonoData.Puntos)
                        {
                            puntos.Add(new Punto(puntoArray[0], puntoArray[1], puntoArray[2]));
                        }

                        Poligono poligono = new Poligono(puntos, poligonoData.Color[0], poligonoData.Color[1], poligonoData.Color[2]);
                        parte.AddPoligono(poligono);
                    }

                    objeto.AddParte(nombreParte, parte);
                }

                escenario.AddObjeto(nombreObjeto, objeto);
            }

            return escenario;
        }
    }

    public class EscenarioData
    {
        public Dictionary<string, ObjetoData> Objetos { get; set; }
    }

    public class ObjetoData
    {
        public Dictionary<string, ParteData> Partes { get; set; }
    }

    public class ParteData
    {
        public List<PoligonoData> Poligonos { get; set; }
    }

    public class PoligonoData
    {
        public float[] Color { get; set; }
        public List<float[]> Puntos { get; set; }
    }
}
