using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalProgramacion
{
    public class Nodo
    {
        public Nodo anterior {  get; set; }
        public Nodo siguiente {  get; set; }
        public Zonas Zona {  get; set; }
        public int index { get; set; }

        public Nodo(Zonas zonas, int index)
        {
            anterior = null;
            siguiente = null;
            this.index = index;
            Zona = zonas;
        }
    }
}
