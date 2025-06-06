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
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace ProyectoFinalProgramacion
{
    public partial class ZonasEstadio : MaterialForm
    {
        private System.Drawing.Color color;
        private List<Zonas> localidades=new List<Zonas> ();
        private int contador = 1;  // Para dar nombre a las localidades
        private List<Estadio> listaEstadios = new List<Estadio>();
        private int contadorBoletos = 0;
        private Estadio estadioSeleccionado { get; set; } = new Estadio();
        public ZonasEstadio()
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
            this.panel1.Paint += new PaintEventHandler(panel1_Paint);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog1.Color;
                materialLabel2.Text = $"Color seleccionado {color.ToString()}";
                materialLabel2.Visible = true;
            }
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = materialTextBox1.Text.Trim();
                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("Por favor ingrese un nombre para la localidad.");
                    return;
                }

                int asientos = int.Parse(materialTextBox2.Text);
                int width = 100;
                int height = (int)asientos/10;
                int x = (panel1.Width - width) / 2;
                int y = 20 + localidades.Sum(l => l.rectangulo.Height + 10);

                Rectangle rect = new Rectangle(x, y, width, height);
                Zonas localidad = new Zonas(materialTextBox1.Text, asientos, int.Parse(materialTextBox3.Text), color, rect,int.Parse(materialTextBox4.Text));
                contadorBoletos += asientos;
                localidades.Add(localidad);

                contador++;
                panel1.Invalidate();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var loc in localidades)
            {
                using (Brush brush = new SolidBrush(loc.color))
                {
                    e.Graphics.FillRectangle(brush, loc.rectangulo);
                    e.Graphics.DrawRectangle(Pens.Black, loc.rectangulo);

                    // Dibujar el texto centrado
                    using (Font font = new Font("Segoe UI", 10))
                    {
                        SizeF textSize = e.Graphics.MeasureString(loc.nombreLocalidad, font);
                        float textX = loc.rectangulo.X + (loc.rectangulo.Width - textSize.Width) / 2;
                        float textY = loc.rectangulo.Y + (loc.rectangulo.Height - textSize.Height) / 2;

                        e.Graphics.DrawString(loc.nombreLocalidad, font, Brushes.White, textX, textY);
                        panel1.AutoSize = true; // Ajustar el tamaño del panel al contenido
                    }
                }
            }
        }

        private void ZonasEstadio_Load(object sender, EventArgs e)
        {
            listaEstadios = Estadio.returnEstadios();
            if (listaEstadios != null)
            {
                foreach (Estadio estadio in listaEstadios)
                {
                    materialComboBox1.Items.Add(estadio.NombreEstadio);
                }
            }
            else
            {
                materialComboBox1.Text = "No hay estadios disponibles";
            }
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                int estadioSeleccionadoIndex = materialComboBox1.SelectedIndex;
                if (estadioSeleccionadoIndex == -1)
                {
                    MessageBox.Show("Por favor seleccione un estadio.");
                    return;
                }

                this.estadioSeleccionado = listaEstadios[estadioSeleccionadoIndex];
                estadioSeleccionado.setLocalidades(localidades);
                estadioSeleccionado.setBoletosVendidos(contadorBoletos);
                materialLabel3.Text = $"Se han agregado {localidades.Count} localidades al estadio {estadioSeleccionado.NombreEstadio}.";
                materialLabel3.Visible = true;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
