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
    public partial class Pagar : MaterialForm
    {
        EsperaClass compraActual { get; set; } = new EsperaClass();
        MainForm mainForm { get; set; }
        Zonas zona { get; set; }
        public Pagar(MainForm mainForm, Zonas zona)
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
            compraActual = EsperaFila.getPrimerTurno();
            materialLabel3.Text = "Id: " + compraActual.idEspera;
            materialLabel4.Text = compraActual.fechaIngreso.ToString("HH:mm:ss") + " Tiene 10 minutos para poder realizar su compra";
            this.mainForm = mainForm;
            this.zona = zona;
        }

        private bool verifyCardNumber(string cardNumber)
        {
            // Validar que tenga solo dígitos y longitud válida
            if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                return false;

            int sum = 0;

            // Recorremos de derecha a izquierda
            for (int i = 0; i < cardNumber.Length; i++)
            {
                int digit = int.Parse(cardNumber[cardNumber.Length - 1 - i].ToString());

                // Duplicar cada segundo dígito
                if (i % 2 == 1)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
            }

            return sum % 10 == 0;
        }

        private bool validateCardDate(string month, string year)
        {
            if (!int.TryParse(month, out int mes) || !int.TryParse(year, out int anio))
                return false;

            anio += 2000; // Asumimos que el año es de 2000 en adelante
            if (mes < 1 || mes > 12) return false;

            var fechaExp = new DateTime(anio, mes, 1).AddMonths(1).AddDays(-1); // último día del mes
            return fechaExp >= DateTime.Today;
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {

            Console.WriteLine("Número válido: " + verifyCardNumber(materialTextBox1.Text));
            Console.WriteLine("Fecha válida: " + validateCardDate(materialTextBox3.Text, materialTextBox4.Text));
            Console.WriteLine("CVV vacío: " + string.IsNullOrWhiteSpace(materialTextBox5.Text));
            Console.WriteLine("Nombre vacío: " + string.IsNullOrWhiteSpace(materialTextBox2.Text));

            // Validar los campos de la tarjeta
            if (string.IsNullOrWhiteSpace(materialTextBox1.Text) ||
                string.IsNullOrWhiteSpace(materialTextBox5.Text) ||
                string.IsNullOrWhiteSpace(materialTextBox2.Text) ||
                !verifyCardNumber(materialTextBox1.Text) ||
                !validateCardDate(materialTextBox3.Text, materialTextBox4.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.");
                return;
            }
            // Si todo es correcto, proceder con el pago
            MessageBox.Show("Pago realizado con éxito.");
            unvalidateSeats(compraActual.asientos);
            mainForm.AbrirFormularioEnPanel(new BoletosFormulario(mainForm, compraActual.asientos,zona)); //Aca tengo que abrir el form donde puede descargar los tickets y se genera el QR
            this.Close();
        }

        private void materialTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (materialTextBox1.Text.Length >= 16)
            {
                materialTextBox2.Focus();
                materialTextBox2.Select();
                return;
            }
        }

        private void materialTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (materialTextBox2.Text.Length >= 3)
            {
                materialTextBox3.Focus();
                materialTextBox3.Select();
                return;
            }
        }

        private void materialTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (materialTextBox3.Text.Length >= 2)
            {
                materialTextBox4.Focus();
                materialTextBox4.Select();
                return;
            }
        }

        private void materialTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (materialTextBox4.Text.Length > 1)
            {
                materialTextBox5.Focus();
                materialTextBox5.Select();
                return;
            }
        }
        private void unvalidateSeats(List<Asiento> asientos)
        {
            foreach (var asiento in asientos)
            {
                // Desmarcar el asiento como seleccionado
                asiento.Seleccionado = false;
                // Marcar el asiento como ocupado
                asiento.Ocupado = true;
            }
            // Actualizar la lista de espera
            EsperaFila.deleteFromJson(compraActual);
            // Asignar el primer turno a la siguiente persona en la fila
            EsperaFila.asignarPrimerTurno();
        }
    }
}
