using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using QRCoder;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Windows.Media;
using System.IO.Ports;

namespace ProyectoFinalProgramacion
{
    public partial class BoletosFormulario : MaterialForm
    {
        List<Asiento> asientos { get; set; }
        MainForm mainForm { get; set; }
        List<Boleto> boletos = new List<Boleto>();
        FlowLayoutPanel contenedorPanel = new FlowLayoutPanel();
        MaterialButton btnDescargarPDF;
        Zonas zonaElegida { get; set; }

        public BoletosFormulario(MainForm mainForm, List<Asiento> asi, Zonas zona)
        {
            InitializeComponent();
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.DARK;
            skinManager.ColorScheme = new ColorScheme(Primary.Grey800, Primary.Grey900, Primary.Indigo100, Accent.Cyan100, TextShade.WHITE);

            this.FormBorderStyle = FormBorderStyle.None;
            this.TopLevel = false;
            this.Dock = DockStyle.Fill;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.mainForm = mainForm;
            this.asientos = asi;
            this.zonaElegida = zona;

            contenedorPanel.Dock = DockStyle.Top;
            contenedorPanel.AutoScroll = true;
            contenedorPanel.FlowDirection = FlowDirection.LeftToRight;
            contenedorPanel.WrapContents = true;
            contenedorPanel.Padding = new Padding(10);

            if (boletos == null) boletos=BoletosJSON.returnList();

            createTickets(asientos);
            showTickets();
            agregarBotonDescargarPDF();
        }

        public void createTickets(List<Asiento> asientos)
        {
            foreach (var asiento in asientos)
            {
                Boleto boleto = new Boleto(asiento, mainForm.username, DateTime.Now);
                boletos.Add(boleto);
                BoletosJSON.agregarBoleto(boleto);
                BoletosJSON.guardarJson(); // Guardamos el boleto en el JSON
            }
        }

        public void showTickets()
        {
            contenedorPanel.Controls.Clear();

            if (boletos.Count > 0)
            {
                foreach (Boleto bol in boletos)
                {
                   if(bol.username == mainForm.username)
                    {
                        MaterialCard contenedor = new MaterialCard
                        {
                            Width = 240,
                            Height = 200,
                            Padding = new Padding(10),
                            Margin = new Padding(10)
                        };

                        MaterialLabel idLabel = new MaterialLabel { Text = "Id: " + bol.idBoleto, AutoSize = true };
                        MaterialLabel usernameLabel = new MaterialLabel { Text = "Usuario: " + bol.username, AutoSize = true };
                        PictureBox qrBox = new PictureBox
                        {
                            Image = GenerarQR(bol),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Width = 100,
                            Height = 100
                        };

                        agregarDeleteButton(bol, contenedor);
                        contenedor.Controls.Add(idLabel);
                        contenedor.Controls.Add(usernameLabel);
                        contenedor.Controls.Add(qrBox);

                        contenedorPanel.Controls.Add(contenedor);
                    }
                }
            }
            contenedorPanel.AutoSize = true;
            this.Controls.Add(contenedorPanel);

            //Guardamos la transaccion
            Transaccion transaccion = new Transaccion(boletos, mainForm.username, zonaElegida.nombreLocalidad, DateTime.Now);
            string ruta = "../../JSON/transacciones.txt";
            if (!File.Exists(ruta))
            {
                File.Create(ruta).Close();
            }

            File.AppendAllText(ruta,transaccion.ToString());

            //Enviamos los boletos al Arduino
            EnviarBoletosAArduino();
        }

        private Bitmap GenerarQR(Boleto boleto)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(boleto.asiento.Columna + " "+ boleto.asiento.Fila + " " + boleto.username + " " + zonaElegida.nombreLocalidad, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }

        private void agregarBotonDescargarPDF()
        {
            btnDescargarPDF = new MaterialButton
            {
                Text = "Descargar QR en PDF",
                Dock = DockStyle.Bottom
            };
            btnDescargarPDF.Click += DescargarBoletosComoPDF;
            this.Controls.Add(btnDescargarPDF);
        }

        private void agregarDeleteButton(Boleto boleto, MaterialCard contenedor)
        {
            MaterialButton deleteButton = new MaterialButton
            {
                Text = "Eliminar Boleto",
                Dock = DockStyle.Bottom
            };
            deleteButton.Click += (sender, e) => EliminarBoleto(boleto, contenedor);
            contenedor.Controls.Add(deleteButton);
        }

        private void EliminarBoleto(Boleto boleto, MaterialCard contenedor)
        {
            if (boletos.Contains(boleto))
            {
                boleto.asiento.Seleccionado = false; // Desmarcamos el asiento
                boleto.asiento.Ocupado = false; // Marcamos el asiento como no ocupado
                boletos.Remove(boleto);
                BoletosJSON.eliminarBoleto(boleto);
                BoletosJSON.guardarJson(); // Guardamos el cambio en el JSON
                contenedorPanel.Controls.Remove(contenedor);
                MessageBox.Show("Boleto eliminado exitosamente.");
            }
            else
            {
                MessageBox.Show("El boleto no existe.");
            }
        }
        private void DescargarBoletosComoPDF(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = "Boletos.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    PdfDocument documento = new PdfDocument();
                    documento.Info.Title = "Boletos con QR";

                    foreach (var boleto in boletos)
                    {
                        PdfPage pagina = documento.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(pagina);

                        // Texto
                        gfx.DrawString($"ID: {boleto.idBoleto}", new XFont("Arial", 14), XBrushes.Black, new XPoint(40, 40));
                        gfx.DrawString($"Usuario: {boleto.username}", new XFont("Arial", 14), XBrushes.Black, new XPoint(40, 70));

                        // QR a imagen temporal
                        using (Bitmap qr = GenerarQR(boleto))
                        {
                            string tempFile = Path.GetTempFileName();
                            qr.Save(tempFile, System.Drawing.Imaging.ImageFormat.Png);

                            using (XImage xImage = XImage.FromFile(tempFile))
                            {
                                gfx.DrawImage(xImage, 40, 100, 150, 150);
                            }

                            File.Delete(tempFile); // Limpieza del archivo temporal
                        }
                    }

                    documento.Save(saveFileDialog.FileName);
                    MessageBox.Show("PDF generado exitosamente.");
                }
            }
        }
        private SerialPort serialPort;

        // Método para enviar los boletos al Arduino
        private void EnviarBoletosAArduino()
        {
            try
            {
                //Para detectar automaticamente el puerto COM del Arduino
                foreach (string portName in SerialPort.GetPortNames())
                {
                    try
                    {
                        serialPort = new SerialPort(portName, 9600);
                        serialPort.Open();

                        string datos = $"{zonaElegida.nombreLocalidad};{mainForm.username}";
                        serialPort.WriteLine(datos);
                        serialPort.Close();
                        MessageBox.Show($"Datos enviados correctamente por {portName}");
                        break;
                    }
                    catch
                    {
                        // Intenta con otro puerto
                        continue;
                    }
                }

            } catch(Exception ex)
            {
                MessageBox.Show("Error al enviar los datos al Arduino: " + ex.Message);
            }
        }
    }
}
