using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ProyectoFinalProgramacion
{
    public class BoletosJSON
    {
        public static List<Boleto> boletos { get; set; } = new List<Boleto>();

        public static void agregarBoleto(Boleto boleto)
        {
            boletos.Add(boleto);
        }

        public static void eliminarBoleto(Boleto boleto)
        {
            boletos.Remove(boleto);
        }

        public static void guardarJson()
        {
            try
            {
                string ruta = "../../JSON/boletos.json";
                if (!File.Exists(ruta))
                {
                    File.Create(ruta).Close();
                }

                string json = JsonSerializer.Serialize(boletos, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ruta, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void desealizeJson()
        {
            try
            {
                string ruta = "../../JSON/boletos.json";
                if (File.Exists(ruta))
                {
                    string json = File.ReadAllText(ruta);
                    boletos = JsonSerializer.Deserialize<List<Boleto>>(json) ?? new List<Boleto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static List<Boleto> returnList()
        {
            if (boletos.Count != 0)
            {
                return boletos;
            }

            desealizeJson();
            return boletos;
        }
    }
}
