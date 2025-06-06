using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace ProyectoFinalProgramacion
{
    public class Estadio
    {
        public double BoletosVendidos { get; set; }
        public string NombreEstadio {  get; set; }
        public Ubicacion Ubicacion {  get; set; }
        public List<Zonas> Localidades {  get; set; }
        public static List<Estadio> estadios=new List<Estadio>();

        public Estadio(string nombreEstadio, Ubicacion ubicacion)
        {
            BoletosVendidos = 0;
            NombreEstadio = nombreEstadio;
            Ubicacion = ubicacion;
            Localidades = null;
        }
        public Estadio() { }
        public void setBoletosVendidos(int cantidad)
        {
            this.BoletosVendidos += cantidad;
        }
        public void setLocalidades(List<Zonas> localidades)
        {
            Localidades = localidades;
            saveEstadios();
        }
        public List<Zonas> returnLocalidades()
        {
            //Luego tengo que hacer que se obtengan los datos de un JSON si es que no los hay
            return Localidades;
        }
        public static void addStadium(Estadio estadio)
        {
            estadios.Add(estadio);
            saveEstadios();
        }
        public static List<Estadio> returnEstadios()
        {
            return estadios;
        }

        public static void loadEstadios()
        {
            // Cargar los estadios desde un archivo JSON o base de datos
            // Aquí puedes implementar la lógica para cargar los estadios
            // Por ejemplo, leer un archivo JSON y deserializarlo en objetos Estadio
            //Cargar los usuarios desde un JSON
            try
            {
                string ruta = "../../JSON/Estadios.json";

                //Verificacion del JSON
                if (File.Exists(ruta))
                {
                    string json = File.ReadAllText(ruta);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        estadios = JsonSerializer.Deserialize<List<Estadio>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar el archivo JSON: " + ex.Message);
            }
        }
        public static bool saveEstadios()
        {
            // Guardar los estadios en un archivo JSON o base de datos
            // Aquí puedes implementar la lógica para guardar los estadios
            // Por ejemplo, serializar la lista de estadios en un archivo JSON

            try
            {
                string json = JsonSerializer.Serialize(estadios, new JsonSerializerOptions { WriteIndented = true });
                string ruta = "../../JSON/Estadios.json";

                if (!File.Exists(ruta))
                {
                    File.Create(ruta).Close();
                }

                File.WriteAllText(ruta, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el archivo JSON: " + ex.Message);
                return false;
            }
        }
    }
}
