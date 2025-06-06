using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalProgramacion
{
    public class EsperaClass
    {
        public string idEspera { get; set; } =Guid.NewGuid().ToString();
        public List<Asiento> asientos { get; set; }
        public Usuario usuario { get; set; }
        public int status { get; set; }
        public DateTime fechaIngreso { get; set; } = DateTime.Now;
        public bool EstaExpirada => DateTime.Now - fechaIngreso > TimeSpan.FromMinutes(10);


        public EsperaClass() { } // Necesario para deserializar

        public EsperaClass(List<Asiento> boletos, Usuario usuario, int status)
        {
            this.asientos = boletos;
            this.usuario = usuario;
            this.status = status;
            this.fechaIngreso = DateTime.Now;
            this.idEspera = Guid.NewGuid().ToString();
        }


    }
}
