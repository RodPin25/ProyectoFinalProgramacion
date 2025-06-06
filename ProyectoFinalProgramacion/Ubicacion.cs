using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalProgramacion
{
    public class Ubicacion
    {
        public string UbicacionString {  get; set; }
        public double Latitud {  get; set; }
        public double Longitud { get; set; }

        public Ubicacion(string ubicacionString, double latitud, double longitud)
        {
            UbicacionString = ubicacionString;
            Latitud = latitud;
            Longitud = longitud;
        }
    }
}
