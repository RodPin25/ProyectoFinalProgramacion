using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ProyectoFinalProgramacion
{
    public class Eventos
    {
        public string nombreEvento { get; set; }
        public DateTime FechaEvento { get; set; }
        public Estadio estadio { get; set; }
        public string categoria { get; set; }

        public static List<Eventos> eventosDisponibles = new List<Eventos>();

        public Eventos(string nombreEvento, DateTime fechaEvento, Estadio estadio, string categoria)
        {
            this.nombreEvento = nombreEvento;
            FechaEvento = fechaEvento;
            this.estadio = estadio;
            this.categoria = categoria;
        }
        public Eventos() { }

        public static List<Eventos> GetEventos()
        {
            if(eventosDisponibles!=null)
                return eventosDisponibles;

            return null;
        }
        public static void addEvento(Eventos eventoAgregar)
        {
            eventosDisponibles.Add(eventoAgregar);
            saveEventos();
        }
        public static void loadEventos()
        {
            try
            {
                string ruta = "../../JSON/Eventos.json";

                //Verificacion del JSON
                if (File.Exists(ruta))
                {
                    string json = File.ReadAllText(ruta);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        eventosDisponibles = JsonSerializer.Deserialize<List<Eventos>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar el archivo JSON: " + ex.Message);
            }
        }
        public static bool saveEventos()
        {
            try
            {
                string json = JsonSerializer.Serialize(eventosDisponibles, new JsonSerializerOptions { WriteIndented = true });
                string ruta = "../../JSON/Eventos.json";

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
