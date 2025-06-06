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
    public partial class Configs : MaterialForm
    {
        private string username {  get; set; }
        private int status {  get; set; }
        private MainForm MainForm { get; set; }
        public Configs(string username, int status, MainForm mainForm)
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
            this.username = username;
            this.status = status;
            MainForm = mainForm;
        }

        private void materialFloatingActionButton1_Click(object sender, EventArgs e)
        {
            MainForm.AbrirFormularioEnPanel(new Estadios());
        }

        private void materialFloatingActionButton2_Click(object sender, EventArgs e)
        {
            MainForm.AbrirFormularioEnPanel(new ZonasEstadio());
        }
    }
}
