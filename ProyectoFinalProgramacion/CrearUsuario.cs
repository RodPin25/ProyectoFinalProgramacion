using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;

namespace ProyectoFinalProgramacion
{
    public partial class CrearUsuario : MaterialForm
    {
        public CrearUsuario()
        {
            InitializeComponent();
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);

            // Tema claro u oscuro
            skinManager.Theme = MaterialSkinManager.Themes.DARK;

            // Paleta de colores
            skinManager.ColorScheme = new ColorScheme(
                Primary.Grey800,        // color primario
                Primary.Grey900,        // color oscuro
                Primary.Indigo100,        // color claro
                Accent.Cyan100,     // acento
                TextShade.WHITE           // color del texto
            );

            this.FormBorderStyle = FormBorderStyle.None;
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            this.Text = string.Empty;
            this.ControlBox = false; // Oculta los botones cerrar/minimizar/maximizar

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = materialTextBox1.Text;
                string pass = materialTextBox2.Text;
                string role = materialComboBox1.Text;
                int status = 0;

                if (role == "VIP")
                    status = 1;
                else if (role == "Normal")
                    status = 2;

                // Hashear la contraseña
                byte[] sal;
                string hash = Hasher.HashearContrasena(pass, out sal);

                // Crear el nuevo usuario
                Usuario nuevoUsuario = new Usuario(username, hash, status, Convert.ToBase64String(sal));

                // Ruta del archivo
                string ruta = "../../JSON/Usuarios.json";
                List<Usuario> usuarios = new List<Usuario>();

                // Leer usuarios existentes
                if (File.Exists(ruta))
                {
                    string contenido = File.ReadAllText(ruta);
                    if (!string.IsNullOrWhiteSpace(contenido))
                    {
                        try
                        {
                            usuarios = JsonSerializer.Deserialize<List<Usuario>>(contenido);
                        }
                        catch (JsonException)
                        {
                            MessageBox.Show("El archivo JSON está corrupto. Se iniciará una nueva lista.");
                            usuarios = new List<Usuario>();
                        }
                    }
                }

                // Agregar el nuevo usuario
                usuarios.Add(nuevoUsuario);

                // Serializar la lista completa y sobrescribir el archivo
                string nuevoJson = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ruta, nuevoJson);

                MessageBox.Show("Se ha creado el usuario exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ingrese datos correctos al formulario", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
        }

    }
}
