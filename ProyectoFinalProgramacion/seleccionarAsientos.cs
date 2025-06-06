using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace ProyectoFinalProgramacion
{
    public partial class seleccionarAsientos : MaterialForm
    {
        MainForm mainForm { get; set; }
        Zonas zona { get; set; }
        List<Asiento> asientosSeleccionados = new List<Asiento>();
        public seleccionarAsientos(MainForm mainForm,Zonas zonaSeleccionada)
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
            this.mainForm = mainForm;
            this.zona = zonaSeleccionada;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var fila in zona.Asientos)
            {
                foreach (var asiento in fila)
                {
                    Brush brush = asiento.Ocupado ? Brushes.Gray :
                                  asiento.Seleccionado ? Brushes.Green :
                                  Brushes.White;

                    int size = 20;
                    int spacing = 5;

                    int x = zona.X + (asiento.Columna * (size + spacing));
                    int y = zona.Y + (asiento.Fila * (size + spacing));
                    asiento.rect = new Rectangle(x, y, size, size);

                    g.FillRectangle(brush, asiento.rect);
                    g.DrawRectangle(Pens.Black, asiento.rect);
                }
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var fila in zona.Asientos)
            {
                foreach (var asiento in fila)
                {
                    if (asiento.rect.Contains(e.Location))
                    {
                        if (asiento.Ocupado) return;

                        // Validar reglas aquí:
                        if (!EsSeleccionValida(fila, asiento)) return;

                        asiento.Seleccionado = !asiento.Seleccionado;
                        asientosSeleccionados.Add(asiento);
                        panel1.Invalidate(); // Redibuja
                        return;
                    }
                }
            }
        }
        private bool EsSeleccionValida(List<Asiento> fila, Asiento asiento)
        {
            int col = asiento.Columna;

            // Si el asiento ya estaba seleccionado, permitir deselección
            if (asiento.Seleccionado)
                return true;

            int seleccionados = fila.Count(a => a.Seleccionado);
            int totalSeleccionados = seleccionados + 1;

            // Límite de máximo 5 asientos seleccionados por fila
            if (totalSeleccionados > 5)
                return false;

            // Si con este clic se selecciona toda la fila (y la fila tiene 5 asientos), permitirlo
            if (totalSeleccionados == fila.Count && fila.Count <= 5)
                return true;

            // No dejar esquinas solas
            if (col == 0 && !fila[col + 1].Seleccionado) return false;
            if (col == fila.Count - 1 && !fila[col - 1].Seleccionado) return false;

            // Regla para el centro: solo 2 seleccionados permitidos
            int mitad = fila.Count / 2;
            if (Math.Abs(col - mitad) <= 1)
            {
                int seleccionadosCentro = fila.Count(a => a.Seleccionado && Math.Abs(a.Columna - mitad) <= 1);
                if (seleccionadosCentro >= 2)
                    return false;
            }

            return true;
        }


        private void seleccionarAsientos_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Seleccionar asientos");
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            EsperaClass espera = new EsperaClass(asientosSeleccionados, Usuario.returnUsuario(mainForm.username), 1);
            EsperaFila.addToList(espera);
            mainForm.AbrirFormularioEnPanel(new ColaFormulario(mainForm,zona));
        }
    }
}
