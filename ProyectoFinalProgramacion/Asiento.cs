using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoFinalProgramacion
{
    public class Asiento
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public bool Ocupado { get; set; }
        public bool Seleccionado { get; set; }

        [JsonIgnore]
        public Rectangle rect { get; set; }

        public Asiento(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
        }
    }
}
