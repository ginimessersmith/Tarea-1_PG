using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    internal class AdministradorObjetos
    {
        public void GuardarObjetos(Objeto objeto, string nombreArchivo)
        {
            // Convertir el objeto a DTO
            ObjetoDTO objetoDTO = objeto.ToDTO();

            // Serializar a JSON
            string json = JsonSerializer.Serialize(objetoDTO, new JsonSerializerOptions { WriteIndented = true });

            // Obtener la ruta del directorio del proyecto
            string directorioProyecto = AppDomain.CurrentDomain.BaseDirectory;

            // Crear la ruta completa del archivo
            string rutaArchivo = Path.Combine(directorioProyecto, nombreArchivo);
            Console.WriteLine(rutaArchivo);

            // Guardar en archivo
            File.WriteAllText(rutaArchivo, json);
        }

        public Objeto CargarDesdeJSON(string nombreArchivo)
        {
            // Obtener la ruta del directorio del proyecto
            string directorioProyecto = AppDomain.CurrentDomain.BaseDirectory;

            // Crear la ruta completa del archivo
            string rutaArchivo = Path.Combine(directorioProyecto, nombreArchivo);

            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(rutaArchivo);

            // Deserializar el JSON a un objeto DTO
            ObjetoDTO objetoDTO = JsonSerializer.Deserialize<ObjetoDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Convertir de DTO a objeto
            return objetoDTO.ToObjeto();
        }

    }

    public class PuntoDTO
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class PoligonoDTO
    {
        public List<PuntoDTO> Puntos { get; set; }
        public float[] Color { get; set; }
    }

    public class ParteDTO
    {
        public List<PoligonoDTO> Poligonos { get; set; }
        public string Nombre { get; set; }
        public PuntoDTO CentroDeMasa { get; set; }
    }

    public class ObjetoDTO
    {
        public Dictionary<string, ParteDTO> Partes { get; set; }
        public PuntoDTO CentroDeMasa { get; set; }
    }

    internal static class Convertidor
    {
        public static PuntoDTO ToDTO(this Punto punto)
        {
            return new PuntoDTO { X = punto.X, Y = punto.Y, Z = punto.Z };
        }

        public static PoligonoDTO ToDTO(this Poligono poligono)
        {
            return new PoligonoDTO
            {
                Puntos = poligono.GetPuntos().ConvertAll(p => p.ToDTO()),
                Color = poligono.GetColor()
            };
        }

        public static ParteDTO ToDTO(this Parte parte)
        {
            return new ParteDTO
            {

                Poligonos = parte.GetPoligonos().ConvertAll(p => p.ToDTO()),
                CentroDeMasa = parte.GetCentroDeMasa().ToDTO()
            };
        }

        public static ObjetoDTO ToDTO(this Objeto objeto)
        {
            return new ObjetoDTO
            {
                Partes = objeto.GetPartes().ToDictionary(
            kvp => kvp.Key, // La clave es el nombre de la parte
            kvp => new ParteDTO
            {
                Nombre = kvp.Key,
                Poligonos = kvp.Value.GetPoligonos().ConvertAll(p => p.ToDTO()),
                CentroDeMasa = kvp.Value.GetCentroDeMasa().ToDTO()
            }
        ),
                CentroDeMasa = objeto.GetCentroDeMasa().ToDTO()
            };
        }

        public static Punto ToPunto(this PuntoDTO puntoDTO)
        {
            return new Punto(puntoDTO.X, puntoDTO.Y, puntoDTO.Z);
        }

        public static Poligono ToPoligono(this PoligonoDTO poligonoDTO)
        {
            List<Punto> puntos = poligonoDTO.Puntos.Select(p => p.ToPunto()).ToList();

            // Asegúrate de que el DTO incluya valores separados para r, g, b, si es necesario.
            // Aquí asumimos que `Color` es un array con tres elementos (r, g, b).
            if (poligonoDTO.Color.Length != 3)
            {
                throw new ArgumentException("El array de color debe tener exactamente 3 elementos.");
            }

            return new Poligono(puntos, poligonoDTO.Color[0], poligonoDTO.Color[1], poligonoDTO.Color[2]);
        }

        public static Parte ToParte(this ParteDTO parteDTO)
        {
            List<Poligono> poligonos = parteDTO.Poligonos.Select(p => p.ToPoligono()).ToList();
            Punto centroDeMasa = parteDTO.CentroDeMasa.ToPunto();
            Parte parte = new Parte();
            parte.setListaPoligono( poligonos );
            parte.SetCentroDeMasa( centroDeMasa );
            return parte;
        }

        public static Objeto ToObjeto(this ObjetoDTO objetoDTO)
        {
            // Convertir el diccionario de partes del DTO a un diccionario de partes del objeto
            Dictionary<string, Parte> partes = objetoDTO.Partes.ToDictionary(
                kvp => kvp.Key, // La clave es el nombre de la parte
                kvp => kvp.Value.ToParte() // Convertir cada ParteDTO a Parte
            );

            // Convertir el centro de masa del DTO a un Punto
            Punto centroDeMasa = objetoDTO.CentroDeMasa.ToPunto();

            // Crear un nuevo Objeto con el diccionario de partes y el centro de masa
            Objeto objeto = new Objeto();
            objeto.setListaPartes(partes);
            objeto.SetCentroDeMasa(centroDeMasa);

            return objeto;
        }

    }

}


