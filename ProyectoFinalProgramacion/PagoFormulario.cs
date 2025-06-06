using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProyectoFinalProgramacion
{
    public partial class PagoFormulario : MaterialForm
    {
        private EsperaClass compraFila { get; set; }
        private Label numeroId = new Label();
        private Label NombreUsuario = new Label();
        private MaterialListBox boletos = new MaterialListBox();
        private MaterialTextBox priceTextBox = new MaterialTextBox();
        private MaterialTextBox cardTextBox = new MaterialTextBox();
        private MaterialTextBox cardNameTextBox = new MaterialTextBox();
        private MaterialTextBox cardCCVTextBox = new MaterialTextBox();
        private MaterialButton pagarButton = new MaterialButton();

        public PagoFormulario()
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

            //ConfigItems();
            //placeItems();
        }

        //private void getData()
        //{
        //    compraFila = EsperaFila.getPrimerTurno();
        //}

        //private void setData()
        //{
        //    getData();
        //    numeroId.Text = "ID de compra: " + compraFila.idEspera;
        //    NombreUsuario.Text = "Usuario: " + compraFila.usuario.Username;
        //    setTickets(boletos);
        //    double total = calculateTotal();
        //    priceTextBox.Text = $"Total: {total.ToString("C2")}";
        //}

        //private void placeItems()
        //{
        //    setData();

        //    layout.FlowDirection = FlowDirection.TopDown;
        //    layout.Dock = DockStyle.Fill;
        //    layout.WrapContents = false;
        //    layout.AutoScroll = true;
        //    layout.AutoSize = true;
        //    layout.Padding = new Padding(20);
        //    layout.BackColor = Color.Transparent;

        //    materialCard1.Controls.Add(numeroId);
        //    materialCard1.Controls.Add(NombreUsuario);
        //    materialCard1.Controls.Add(boletos);
        //    materialCard1.Controls.Add(priceTextBox);
        //    materialCard1.Controls.Add(cardTextBox);
        //    materialCard1.Controls.Add(cardNameTextBox);
        //    materialCard1.Controls.Add(cardCCVTextBox);
        //    materialCard1.Controls.Add(pagarButton);

        //    layout.Controls.Add(materialCard1);

        //    //Configuraciones del material Card
        //    materialCard1.AutoSize = true;
        //    materialCard1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        //    materialCard1.Padding = new Padding(20);
        //}

        //private void ConfigItems()
        //{
        //    numeroId = new Label();
        //    numeroId.AutoSize = true;
        //    numeroId.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        //    numeroId.ForeColor = Color.White;
        //    numeroId.TextAlign = ContentAlignment.MiddleCenter;

        //    NombreUsuario = new Label();
        //    NombreUsuario.AutoSize = true;
        //    NombreUsuario.Font = new Font("Segoe UI", 12, FontStyle.Regular);
        //    NombreUsuario.ForeColor = Color.White;
        //    NombreUsuario.TextAlign = ContentAlignment.MiddleCenter;

        //    boletos = new MaterialListBox();
        //    boletos.Dock = DockStyle.Fill;
        //    boletos.BackColor = Color.FromArgb(40, 40, 40);

        //    priceTextBox = new MaterialTextBox();
        //    priceTextBox.Hint = "Precio Total";

        //    cardTextBox = new MaterialTextBox();
        //    cardTextBox.Hint = "Número de Tarjeta";

        //    cardNameTextBox = new MaterialTextBox();
        //    cardNameTextBox.Hint = "Nombre en la Tarjeta";

        //    cardCCVTextBox = new MaterialTextBox();
        //    cardCCVTextBox.Hint = "CCV";

        //    pagarButton = new MaterialButton();
        //    pagarButton.Text = "Pagar";
        //    //pagarButton.Click += pagarButton_Click;
        //}


        //private void setTickets(MaterialListBox lst)
        //{
        //    List<Asiento> tickets = compraFila.asientos;
        //    foreach (Asiento ticket in tickets)
        //    {
        //        MaterialListBoxItem item = new MaterialListBoxItem
        //        {
        //            Text = $"Columna: {ticket.Columna} y Fila: {ticket.Fila}",
        //        };
        //        lst.Items.Add(item);
        //    }
        //}

        //private double calculateTotal()
        //{
        //    return 0.00;
        //}

        private void PagoFormulario_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Se cargo el formulario del pago, en caso de que no se vea no se esta renderizando pero si cargando");
        }
    }
}
