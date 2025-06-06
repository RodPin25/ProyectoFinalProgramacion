using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ProyectoFinalProgramacion
{
    public class Usuario
    {
        public string Username {  get; set; }
        public string Password { get; set; }
        public int Status {  get; set; }
        public string sal {  get; set; }
        public static List<Usuario> usuarios= new List<Usuario>();

        public Usuario(string username, string password, int status, string sal)
        {
            Username = username;
            Password = password;
            Status = status;
            this.sal = sal;
        }
        public Usuario() { }

        public static Usuario returnUsuario(string username)
        {
            loadUsuarios();
            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Username == username)
                {
                    return usuario;
                }
            }
            return null;
        }
        public static void loadUsuarios()
        {
            //Cargar los usuarios desde un JSON
            try
            {
                string ruta = "../../JSON/Usuarios.json";

                //Verificacion del JSON
                if (File.Exists(ruta))
                {
                    string json = File.ReadAllText(ruta);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        usuarios = JsonSerializer.Deserialize<List<Usuario>>(json);
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Error al cargar el archivo JSON: " + ex.Message);
            }
        }
    }
}
