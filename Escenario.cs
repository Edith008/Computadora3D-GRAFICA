using System;
using System.Collections.Generic;

namespace Computadora3D
{
    [Serializable]
    class Escenario
    {
        public string Nombre { get; set; } = string.Empty;

        // Centro del escenario [x, y, z]
        public double[] Centro { get; set; } = new double[3];

        // Posición del escenario [x, y, z]
        public double[] Posicion { get; set; } = new double[3];

        // Lista de objetos
        public List<Objeto> Objetos { get; set; } = new();

        // Diccionario para acceso rápido por nombre
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
    }
}
