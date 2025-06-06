using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using MaterialSkin;
using MaterialSkin.Controls;

namespace ProyectoFinalProgramacion
{
    public partial class EventosForm : MaterialForm
    {
        private Eventos eventos = new Eventos();
        private MainForm mainForm { get; set; }
        private MaterialLabel eventStadium;
        public EventosForm(MainForm mainForm)
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
        }


        private void EventosForm_Load(object sender, EventArgs e)
        {
            List<Eventos> listaEventos = Eventos.GetEventos();

            if (listaEventos == null)
            {
                PictureBox iconBox = new PictureBox();
                Label message = new Label();

                //Configurando el texto
                message.Text = "No hay ningun evento disponible, por favor intente mas tarde";
                message.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                message.AutoSize = true;
                message.TextAlign = ContentAlignment.MiddleCenter;
                message.Location = new Point(30, 140);

                //Configurando el PictureBox
                iconBox.Image = Properties.Resources.local_cafe_24dp_000_FILL0_wght400_GRAD0_opsz24;//Aca iria la imagen pero no se como se agrega desde la carpeta Resources
                iconBox.SizeMode = PictureBoxSizeMode.StretchImage;
                iconBox.Size = new Size(100, 100);
                iconBox.Location = new Point(50, 30);

                // Configurar grupo
                GroupBox grupoError=new GroupBox();
                grupoError.Size = new Size(300, 200);
                grupoError.Controls.Add(iconBox);
                grupoError.Controls.Add(message);

                // Centrar el grupo en el formulario
                grupoError.Location = new Point(
                    (this.ClientSize.Width - grupoError.Width) / 2,
                    (this.ClientSize.Height - grupoError.Height) / 2
                );

                grupoError.Anchor = AnchorStyles.None;
                grupoError.FlatStyle = FlatStyle.Flat;
                grupoError.BackColor = Color.Transparent;
                grupoError.Text = ""; // Sin título

                // Agregar al formulario
                flowLayoutPanel1.Controls.Add(grupoError);


            }
            else
            {
                createEventGrid(listaEventos, flowLayoutPanel1);
            }
        }
        private void createEventGrid(List<Eventos> listaEvent, FlowLayoutPanel contenedor)
        {
            contenedor.Controls.Clear(); // Limpia el contenedor antes de agregar

            foreach (Eventos e in listaEvent)
            {
                // Crear la tarjeta
                MaterialCard eventCard = new MaterialCard
                {
                    Width = 400,
                    Height = 180,
                    Margin = new Padding(10),
                    BackColor = Color.White
                };

                // Crear y configurar etiquetas
                Label eventTitle = new Label
                {
                    Text = e.nombreEvento,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    AutoSize = true,
                    Location = new Point(10, 10)
                };

                MaterialLabel eventDate = new MaterialLabel
                {
                    Text = "Fecha: " + e.FechaEvento.ToShortDateString(),
                    Location = new Point(10, 50),
                    AutoSize = true
                };

                if (e.estadio != null)
                {
                    eventStadium = new MaterialLabel
                    {
                        Text = "Estadio: " + e.estadio.NombreEstadio,
                        Location = new Point(10, 80),
                        AutoSize = true
                    };
                }

                // Botón de comprar entrada
                MaterialButton buyTicket = new MaterialButton
                {
                    Text = "Comprar",
                    Location = new Point(10, 120),
                    Width = 100
                };

                // Agregar controles a la tarjeta
                eventCard.Controls.Add(eventTitle);
                eventCard.Controls.Add(eventDate);
                eventCard.Controls.Add(eventStadium);
                eventCard.Controls.Add(buyTicket);

                // Agregar tarjeta al contenedor
                contenedor.Controls.Add(eventCard);

                buyTicket.Click += (s, args) =>
                {
                    openBuyTickets(e);
                };
            }
        }
        
        private void openBuyTickets(Eventos eventoSeleccionado)
        {
            mainForm.AbrirFormularioEnPanel(new VentaBoleto(eventoSeleccionado,mainForm));
        }
    }
}
