using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Computadora3D
{
    [Serializable]
    class Partes
    {
        public string Nombre { get; set; } = string.Empty;
        public double[] Centro { get; set; } = new double[3];
        public double[] Posicion { get; set; } = new double[3];
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
