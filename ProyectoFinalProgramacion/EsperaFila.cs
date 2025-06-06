using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ProyectoFinalProgramacion
{
    public class EsperaFila
    {
        public static List<EsperaClass> listaEspera =new List<EsperaClass>();
        

        public static void addToList(EsperaClass agregar)
        {
            listaEspera.Add(agregar);
            guardarJson();
        }
        public static void setStatusActive(EsperaClass objeto)
        {
            objeto.status = 1;
        }
        public static void deleteFirst()
        {
            for(int i = 0; i < listaEspera.Count-1; i++)
            {
                listaEspera[i]=listaEspera[i + 1];
            }
            listaEspera.RemoveAt(listaEspera.Count - 1);

        }
        public static bool guardarJson()
        {
            try
            {
                string json = JsonSerializer.Serialize(listaEspera, new JsonSerializerOptions { WriteIndented = true });
                string ruta = "../../JSON/Espera.json";

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
        public static bool cargarJson()
        {
            try
            {
                string ruta = "../../JSON/Espera.json";
                if (File.Exists(ruta))
                {
                    string json = File.ReadAllText(ruta);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    listaEspera = JsonSerializer.Deserialize<List<EsperaClass>>(json, options);
                    return true;
                }
                else
                {
                    MessageBox.Show("El archivo JSON no existe.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el archivo JSON: " + ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public static void asignarPrimerTurno()
        {
            if (listaEspera.Count > 0 && listaEspera.All(e => e.status != 1))
            {
                listaEspera[0].status = 1;
                guardarJson(); // opcional: guardar el cambio si lo necesitas persistente
            }
        }
        public static EsperaClass getPrimerTurno()
        {
            if (listaEspera.Count > 0 && listaEspera.All(e => e.status == 1))
            {
                return listaEspera[0];
            }
            else
            {
                return null;
            }
        }
        public static void deleteFromJson(EsperaClass espera)
        {
            // Buscar el objeto en la lista
            EsperaClass objetoAEliminar = listaEspera.FirstOrDefault(e =>
                e.usuario.Username == espera.usuario.Username &&
                e.status == espera.status); // Puedes comparar más campos si es necesario

            if (objetoAEliminar != null)
            {
                listaEspera.Remove(objetoAEliminar);

                //Actualizamos el status de la siguiente persona en querer comprar.
                if (listaEspera.Count > 0)
                {
                    setStatusActive(listaEspera[0]);
                }
                guardarJson(); // Guardar la lista actualizada en el archivo
            }
        }
        public static void clearList()
        {
            listaEspera.Clear();
        }
        public static int getCount()
        {
            return listaEspera.Count;
        }
        public static int getPlaceQueue(Usuario usuario)
        {
            int place = 1;
            foreach (EsperaClass compra in listaEspera)
            {
                if (compra.usuario.Username == usuario.Username)
                {
                    return place;
                }
                place++;
            }

            return 0;
        }
        public static bool canPay(EsperaClass compra)
        {
            int statusActual = 0;
            foreach(EsperaClass peticion in listaEspera)
            {
                if (peticion.idEspera == compra.idEspera)
                {
                    statusActual = peticion.status;
                }
            }

            if (statusActual == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
