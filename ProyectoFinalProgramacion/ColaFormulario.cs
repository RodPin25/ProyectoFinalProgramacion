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

namespace ProyectoFinalProgramacion
{
    public partial class ColaFormulario : MaterialForm
    {
        private MainForm mainForm;
        private string username;
        private Usuario usuarioActual;
        private FileSystemWatcher watcher;
        private Zonas zonaElegida;

        public ColaFormulario(MainForm mainForm, Zonas zonaElegida)
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
            this.username = mainForm.username;

            //Comenzamos a verificar la fila de espera
            checkQueue();
            iniciarWatcher();
            this.zonaElegida = zonaElegida;
        }
        private void placeItems(MaterialCard contenedor)
        {
            contenedor.AutoSize = false; // Clave para poder modificar el tamaño

            int widthParent = contenedor.Parent.Width;
            int heightParent = contenedor.Parent.Height;

            contenedor.Size = new Size(widthParent - 100, heightParent - 100); // o el tamaño que desees
            contenedor.Location = new Point(
                (widthParent - contenedor.Width) / 2,
                (heightParent - contenedor.Height) / 2
            );
        }
        private void getUsuarioActual(string username)
        {
            usuarioActual=Usuario.returnUsuario(username);
        }
        public void checkQueue()
        {
            getUsuarioActual(username);
            // Cargar los datos desde el JSON
            if (!EsperaFila.cargarJson())
            {
                MessageBox.Show("No se pudo cargar la fila de espera.");
                return;
            }

            //Activar el usuario si en caso ningun usuario tiene el estado 1 en la lista de espera
            EsperaFila.asignarPrimerTurno();
            // Buscar la posición del usuario
            int lugar = EsperaFila.getPlaceQueue(usuarioActual);

            if (lugar == 0)
            {
                MessageBox.Show("No estás en la fila de espera.");
                return;
            }

            // Obtener objeto de espera del usuario
            EsperaClass miEspera = EsperaFila.listaEspera.FirstOrDefault(e => e.usuario.Username == usuarioActual.Username);

            if (miEspera != null)
            {
                if (miEspera.status == 1)
                {
                    MessageBox.Show("¡Es tu turno! Puedes proceder al pago.");
                    mainForm.BeginInvoke(new Action(() => {
                        mainForm.AbrirFormularioEnPanel(new Pagar(mainForm,zonaElegida));
                    }));
                }
                else
                {
                    // Si aun no puede pasar a la compra.
                    MaterialCard contendor = new MaterialCard();
                    contendor.AutoSize = false;
                    contendor.Size = new Size(300, 150); // Puedes ajustarlo
                    contendor.Padding = new Padding(10);

                    // Crear un layout que alinee los elementos al centro
                    TableLayoutPanel layout = new TableLayoutPanel();
                    layout.Dock = DockStyle.Fill;
                    layout.ColumnCount = 1;
                    layout.RowCount = 2;
                    layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                    layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                    layout.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    layout.BackColor = Color.Transparent;

                    // Centrar los contenidos
                    layout.Anchor = AnchorStyles.None;

                    // Crear los labels
                    MaterialLabel messageLabel = new MaterialLabel();
                    messageLabel.Text = $"Tu lugar en la fila es: {lugar}";
                    messageLabel.TextAlign = ContentAlignment.MiddleCenter;
                    messageLabel.Dock = DockStyle.Fill;

                    MaterialLabel secondMessageLabel = new MaterialLabel();
                    secondMessageLabel.Text = "Por favor espera un momento.";
                    secondMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
                    secondMessageLabel.Dock = DockStyle.Fill;

                    // Agregar labels al layout
                    layout.Controls.Add(messageLabel, 0, 0);
                    layout.Controls.Add(secondMessageLabel, 0, 1);

                    // Agregar el layout al contenedor
                    contendor.Controls.Add(layout);

                    // Agregar labels al layout
                    layout.Controls.Add(messageLabel, 0, 0);
                    layout.Controls.Add(secondMessageLabel, 0, 1);

                    // Agregar el layout al contenedor
                    contendor.Controls.Add(layout);
                    flowLayoutPanel1.Controls.Add(contendor);

                    placeItems(contendor);
                }
            }
        }
        private void iniciarWatcher()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = Path.GetFullPath("../../JSON");
            watcher.Filter = "Espera.json";
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Changed += (s, e) =>
            {
                // Para evitar múltiples eventos, espera un poco antes de ejecutar
                Task.Delay(500).ContinueWith(_ =>
                {
                    this.Invoke((MethodInvoker)(() => checkQueue()));
                });
            };

            watcher.EnableRaisingEvents = true;
        }

    }
}
