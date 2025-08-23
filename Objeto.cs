using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Computadora3D
{
    public class Objeto : IDisposable
    {
        public string Nombre { get; set; }
        private List<(Poligono poligono, Vector3 posicion)> poligonos;

        public Objeto(string nombre)
        {
            Nombre = nombre;
            poligonos = new List<(Poligono, Vector3)>();
        }

        public void AgregarPoligono(Poligono poligono, Vector3 posicion = default)
        {
            poligonos.Add((poligono, posicion));
        }

        public void Dibujar(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            foreach (var (poligono, posicion) in poligonos)
            {
                // Aplicar traslación a cada polígono
                Matrix4 modeloTransformado = model * Matrix4.CreateTranslation(posicion);
                poligono.Dibujar(modeloTransformado, view, projection);
            }
        }

        public void Dispose()
        {
            foreach (var (poligono, _) in poligonos)
            {
                poligono.Dispose();
            }
            poligonos.Clear();
        }
    }
}