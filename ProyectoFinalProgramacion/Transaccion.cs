using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalProgramacion
{
    public class Transaccion
    {
        public string idTransaccion=Guid.NewGuid().ToString();
        public List<Boleto> boletos { get; set; }
        public string username { get; set; }
        public DateTime fechaTransaccion { get; set; } = DateTime.Now;
        public string nombreLocalidad { get; set; } = "Localidad General"; // Por defecto, se puede cambiar según la zona

        public Transaccion(List<Boleto> boletos, string username, string localidad, DateTime fechaTransaccion)
        {
            this.boletos = boletos;
            this.username = username;
            this.fechaTransaccion = fechaTransaccion;
            this.nombreLocalidad = localidad;
        }
    }
}
