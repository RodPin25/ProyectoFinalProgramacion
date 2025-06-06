using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

namespace ProyectoFinalProgramacion
{
    public class Zonas
    {
        public string nombreLocalidad { get; set; }
        public int cantidadAsientos { get; set; }
        public double PrecioBoleto { get; set; }
        public List<List<Asiento>> Asientos { get; set; } = new List<List<Asiento>>();

        // Color como entero ARGB
        public int colorArgb { get; set; }

        // Rectángulo como propiedades simples
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        // Propiedad para obtener Color a partir del ARGB
        [JsonIgnore]
        public Color color
        {
            get => Color.FromArgb(colorArgb);
            set => colorArgb = value.ToArgb();
        }

        // Propiedad para obtener el Rectangle a partir de las coordenadas
        [JsonIgnore]
        public Rectangle rectangulo
        {
            get => new Rectangle(X, Y, Width, Height);
            set
            {
                X = value.X;
                Y = value.Y;
                Width = value.Width;
                Height = value.Height;
            }
        }

        // Constructor vacío necesario para deserialización
        public Zonas() { }

        public Zonas(string nombreLocalidad, int cantidadAsientos, double precioBoleto, Color color, Rectangle rectangulo,int columnas)
        {
            this.nombreLocalidad = nombreLocalidad;
            this.cantidadAsientos = cantidadAsientos;
            this.PrecioBoleto = precioBoleto;
            this.color = color;
            this.rectangulo = rectangulo;
            InicializarAsientos(columnas); // Inicializa los asientos al crear la zona
        }
        public void InicializarAsientos(int columnas)
        {
            int filas = cantidadAsientos / columnas; //Para saber cuantas son las filas que tenemos que hacer
            for (int i = 0; i < filas; i++)
            {
                var fila = new List<Asiento>();
                for (int j = 0; j < columnas; j++)
                {
                    fila.Add(new Asiento(i, j));
                }
                Asientos.Add(fila);
            }
        }

    }
}
