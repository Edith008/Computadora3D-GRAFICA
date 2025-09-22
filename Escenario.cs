using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Computadora3D
{
    [Serializable]
    class Escenario
    {
        public string Nombre { get; set; } = string.Empty;
        public double[] Centro { get; set; } = new double[3];
        public double[] Posicion { get; set; } = new double[3];
        public List<Objeto> Objetos { get; set; } = new();
        public Dictionary<string, Objeto> ObjetosDiccionario { get; set; } = new();

        // Agregar objeto al escenario
        public void AgregarObjeto(Objeto objeto)
        {
            if (!ObjetosDiccionario.ContainsKey(objeto.Nombre))
            {
                Objetos.Add(objeto);
                ObjetosDiccionario[objeto.Nombre] = objeto;
            }
        }

        // Obtener objeto por nombre
        public Objeto? GetObjeto(string nombre)
        {
            ObjetosDiccionario.TryGetValue(nombre, out var objeto);
            return objeto;
        }


        // Transformaciones globales
        public Vector3 Traslacion { get; private set; } = Vector3.Zero;
        public float RotacionY { get; private set; } = 0f;
        public Vector3 Escalado { get; private set; } = new Vector3(1f, 1f, 1f);

        // Proyección: true = perspectiva, false = ortográfica
        public bool UsarProyeccionPerspectiva { get; private set; } = true;

        // Métodos de transformación
        public void AplicarTraslacion(Vector3 delta) => Traslacion += delta;
        public void AplicarRotacion(float angulo) => RotacionY += angulo;
        public void AplicarEscalado(Vector3 factor) => Escalado *= factor;

        // Cambiar proyección
        public void CambiarProyeccion() => UsarProyeccionPerspectiva = !UsarProyeccionPerspectiva;


    }
}
