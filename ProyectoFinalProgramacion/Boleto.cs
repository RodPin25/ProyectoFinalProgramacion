using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalProgramacion
{
    public class Boleto
    {
        public string idBoleto=Guid.NewGuid().ToString();
        public Asiento asiento { get; set; }
        public string username { get; set; }
        public DateTime fechaCompra { get; set; } = DateTime.Now;
        public Boleto(Asiento asiento, string username, DateTime fechaCompra)
        {
            this.asiento = asiento;
            this.username = username;
            this.fechaCompra = fechaCompra;
        }
    }
}
