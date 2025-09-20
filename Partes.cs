using System;
using System.Collections.Generic;

namespace Computadora3D
{
    [Serializable]
    class Partes
    {
        public string Nombre { get; set; } = string.Empty;
        // Centro de la parte [x, y, z]
        public double[] Centro { get; set; } = new double[3];

        // Posición de la parte en el objeto [x, y, z]
        public double[] Posicion { get; set; } = new double[3];
        // Lista de caras
        public List<Cara> Caras { get; set; } = new();
        public Dictionary<string, Cara> CarasDiccionario { get; set; } = new();
        // Agregar cara
        public void AgregarCara(Cara cara)
        {
            if (!CarasDiccionario.ContainsKey(cara.Nombre))
            {
                Caras.Add(cara);
                CarasDiccionario[cara.Nombre] = cara;
            }
        }

        // Obtener cara
        public Cara? GetCara(string nombre)
        {
            CarasDiccionario.TryGetValue(nombre, out var cara);
            return cara;
        }
    }
}
