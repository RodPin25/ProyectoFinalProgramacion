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
    public partial class Estadios : MaterialForm
    {
        private double latitud {  get; set; }
        private double longitud {  get; set; }
        private Estadio listaEstadio=new Estadio();

        public Estadios()
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
            setMapConfig();
            materialLabel2.Visible = false;
        }
        private void setMapConfig()
        {
            gMapControl1.MapProvider = GMapProviders.OpenStreetMap;
            gMapControl1.Position = new PointLatLng(14.6349, -90.5069); // Coordenadas de ejemplo (Guatemala)
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 18;
            gMapControl1.Zoom = 10;
            gMapControl1.ShowCenter = false;

            // Habilita que se pueda arrastrar el mapa con el mouse
            gMapControl1.DragButton = MouseButtons.Right;
            gMapControl1.CanDragMap = true;

            // Evento para capturar clics en el mapa
            //gMapControl1.MouseClick += Gmap_MouseClick;
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Convierte el punto clicado en coordenadas geográficas
                PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

                latitud = point.Lat;
                longitud = point.Lng;

                // Mostrar marcador
                var markers = new GMapOverlay("markers");
                var marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
                markers.Markers.Add(marker);
                gMapControl1.Overlays.Clear();
                gMapControl1.Overlays.Add(markers);
                gMapControl1.Invalidate();
            }
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (materialTextBox1.Text != null && materialTextBox21.Text != null)
                {
                    Ubicacion ubi = new Ubicacion(materialTextBox21.Text, latitud, longitud);
                    Estadio estadio = new Estadio(materialTextBox1.Text, ubi);
                    Estadio.addStadium(estadio);
                    materialLabel2.Text = "Se creo el estadio correctamente ✅";
                    materialLabel2.Visible = true;
                }
                else
                {
                    MessageBox.Show("Complete los campos que se le solicitan", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                materialLabel2.Text = "No se creo el estadio";
                materialLabel2.Visible = true;
            }
            finally
            {
                materialTextBox1.Text = "";
                materialTextBox21.Text = "";
                latitud = 0;
                longitud = 0;
            }
        }
    }
}
