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

namespace ProyectoFinalProgramacion
{
    public partial class MainForm : MaterialForm
    {
        public string username;
        public int status;
        public MainForm(string username, int status)
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
            this.username = username;
            this.status = status;
            this.WindowState = FormWindowState.Maximized;

            //Cargamos los estadios y los eventos
            Estadio.loadEstadios();
            Eventos.loadEventos();
        }

        public void AbrirFormularioEnPanel(Form formulario)
        {
            panelContenedor.Controls.Clear(); // Limpia el contenido actual

            formulario.TopLevel = false;         // El formulario no es de nivel superior
            formulario.FormBorderStyle = FormBorderStyle.None; // Sin bordes
            formulario.Dock = DockStyle.Fill;    // Ocupa todo el panel

            panelContenedor.Controls.Add(formulario);
            panelContenedor.Tag = formulario;

            formulario.Show();
            formulario.BringToFront(); // Asegura que se muestre sobre todo

            //Vamos a forzar el renderizado
            panelContenedor.Refresh();
            panelContenedor.Invalidate();
            formulario.Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            checkUserStatus();
            AbrirFormularioEnPanel(new EventosForm(this));
        }
        private void checkUserStatus()
        {
            if (status != 0)
            {
                materialButton2.Visible = false;
                materialButton3.Visible = false;
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new CrearEventos());
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new Configs(username,status,this));
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new EventosForm(this));
        }
    }
}
