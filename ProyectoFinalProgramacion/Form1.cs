using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace ProyectoFinalProgramacion
{
    public partial class Form1 : MaterialForm
    {
        private List<Usuario> usuarios=new List<Usuario>();
        public Form1()
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
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            string username = materialTextBox1.Text;
            string contra = materialTextBox2.Text;
            if (validarUsuario(username, contra))
            {
                MainForm mainForm=new MainForm(username,returnStatus(username));
                mainForm.Show();
                Hide();
            }
            else
                MessageBox.Show("Inicio de sesion incorrecto");
        }

        private bool validarUsuario(string username,string contra)
        {
            foreach(Usuario usario in usuarios)
            {
                if (username == usario.Username)
                    return Hasher.VerificarContrasena(contra, Convert.FromBase64String(usario.sal), usario.Password);
            }

            return false;
        }
        private int returnStatus(string username)
        {
            foreach(Usuario usuario in usuarios)
            {
                if (username == usuario.Username)
                    return usuario.Status;
            }

            return 0;
        }

        private void obtenerUsuarios()
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            obtenerUsuarios();
        }
    }
}
