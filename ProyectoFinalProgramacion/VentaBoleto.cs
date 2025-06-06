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
    public partial class VentaBoleto : MaterialForm
    {
        private Eventos eventoSeleccionado { get; set; }
        private List<Zonas> localidades { get; set; } = new List<Zonas>();
        private MainForm mainForm { get; set; }
        public VentaBoleto(Eventos eventoSeleccionado,MainForm main)
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
            this.eventoSeleccionado = eventoSeleccionado;
            this.mainForm = main;
            setMapConfig();
        }
        private void setMapConfig()
        {
            gMapControl1.MapProvider = GMapProviders.OpenStreetMap;
            gMapControl1.Position = new PointLatLng(eventoSeleccionado.estadio.Ubicacion.Latitud, eventoSeleccionado.estadio.Ubicacion.Longitud); 
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 18;
            gMapControl1.Zoom = 10;
            gMapControl1.ShowCenter = false;
            gMapControl1.DragButton = MouseButtons.Right;
            gMapControl1.CanDragMap = true;

            // Habilita que se pueda arrastrar el mapa con el mouse
            gMapControl1.CanDragMap = false;

            // Crear una capa de marcadores
            GMapOverlay markersOverlay = new GMapOverlay("markers");

            // Crear el marcador
            PointLatLng point = new PointLatLng(eventoSeleccionado.estadio.Ubicacion.Latitud, eventoSeleccionado.estadio.Ubicacion.Longitud);
            GMarkerGoogle marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);

            // Agregar el marcador a la capa
            markersOverlay.Markers.Add(marker);

            // Agregar la capa al mapa
            gMapControl1.Overlays.Add(markersOverlay);
        }

        private void VentaBoleto_Load(object sender, EventArgs e)
        {
            localidades = eventoSeleccionado.estadio.Localidades;
            materialLabel2.Text= "Evento: " + eventoSeleccionado.nombreEvento;
            materialLabel3.Text = "Estadio: " + eventoSeleccionado.estadio.NombreEstadio;
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
                    }
                }
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var localidad in localidades)
            {
                if (localidad.rectangulo.Contains(e.Location))  // Verificar si el clic fue dentro del rectángulo
                {
                    mainForm.AbrirFormularioEnPanel(new seleccionarAsientos(mainForm,localidad));
                    break;
                }
            }
        }
    }
}
