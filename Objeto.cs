using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Computadora3D
{
    [Serializable]
    class Objeto
    {
        public string Nombre { get; set; } = string.Empty;
        public double[] Centro { get; set; } = new double[3];
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


        // Transformaciones 
        public Vector3 Traslacion { get; private set; } = Vector3.Zero;
        public Vector3 Escalado { get; private set; } = new(1f, 1f, 1f);
        public float RotacionY { get; private set; } = 0f;
        public bool UsarProyeccionPerspectiva { get; private set; } = true;

        public void AplicarTraslacion(Vector3 delta) => Traslacion += delta;
        public void AplicarEscalado(Vector3 factor) => Escalado *= factor;
        public void AplicarRotacion(float angulo) => RotacionY += angulo;
        public void CambiarProyeccion() => UsarProyeccionPerspectiva = !UsarProyeccionPerspectiva;

    }
}
