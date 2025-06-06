using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalProgramacion
{
    public class ZonaCircular
    {
        public Nodo primero {  get; set; }
        public Nodo ultimo {  get; set; }
        public Nodo actual {  get; set; }
        public static int index = 0;
        
        public void InsertarLista(Zonas zona)
        {
            Nodo insertar = new Nodo(zona, index);
            if (primero == null)
            {
                primero = ultimo = insertar;
                primero.siguiente = primero;
                primero.anterior = primero;
                actual = primero;
            } else
            {
                ultimo.siguiente = primero;
                insertar.siguiente = primero;
                insertar.anterior = ultimo;
                primero.anterior = insertar;
                ultimo = insertar;
            }

            index++;
        }
        public List<Zonas> returnZonas()
        {
            List<Zonas> zonas = new List<Zonas>();

            if (primero == null)
                return zonas;

            Nodo actual = primero;
            do
            {
                zonas.Add(actual.Zona);
                actual = actual.siguiente;
            } while (actual != primero);

            return zonas;
        }
    }
}
