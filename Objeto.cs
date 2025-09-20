using System;
using System.Collections.Generic;

namespace Computadora3D
{
    [Serializable]
    class Objeto
    {
        public string Nombre { get; set; } = string.Empty;
        // Centro del objeto [x, y, z]
        public double[] Centro { get; set; } = new double[3];

        // Posición del objeto en el escenario [x, y, z]
        public double[] Posicion { get; set; } = new double[3];
        public List<Partes> Partes { get; set; } = new();
        public Dictionary<string, Partes> PartesDiccionario { get; set; } = new();

        // Agregar parte
        public void AgregarParte(Partes parte)
        {
            if (!PartesDiccionario.ContainsKey(parte.Nombre))
            {
                Partes.Add(parte);
                PartesDiccionario[parte.Nombre] = parte;
            }
        }

        // Obtener parte
        public Partes? GetParte(string nombre)
        {
            PartesDiccionario.TryGetValue(nombre, out var parte);
            return parte;
        }
    }
}
