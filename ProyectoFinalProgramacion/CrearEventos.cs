using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using MaterialSkin;
using MaterialSkin.Controls;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;

namespace ProyectoFinalProgramacion
{
    public partial class CrearEventos : MaterialForm
    {
        private double latitud {  get; set; }
        private double longitud { get; set; }
        private Estadio estadioSeleccionado;
        public CrearEventos()
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

        private void gMapControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CrearEventos_Load(object sender, EventArgs e)
        {
            List<Estadio> listaEstadios= Estadio.returnEstadios();

            if (listaEstadios != null)
            {
                foreach (Estadio estadio in listaEstadios)
                {
                    materialComboBox1.Items.Add(estadio.NombreEstadio);
                }
            }
            else
            {
                materialComboBox1.Text= "No hay estadios disponibles";
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (materialComboBox1.Text == "No hay estadios disponibles")
                {
                    MessageBox.Show("No hay estadios disponibles");
                }
                else
                {
                    string nombreEvento = materialTextBox1.Text;
                    int estadio = materialComboBox1.SelectedIndex;
                    DateTime fechaEvento = dateTimePicker1.Value;
                    string categoria = materialTextBox2.Text;
                    List<Estadio> estadios = Estadio.returnEstadios();

                    //Obtener el estadio seleccionado
                    estadioSeleccionado = estadios[estadio];
                    // Crear un nuevo evento
                    Eventos evento = new Eventos(nombreEvento, fechaEvento, estadioSeleccionado, categoria);
                    Eventos.addEvento(evento);
                    materialLabel3.Text = "Se creo correctamente el evento ✅";
                    materialLabel3.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el evento: " + ex.Message);
            }
        }
    }
}
