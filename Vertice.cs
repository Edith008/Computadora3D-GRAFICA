using System;

namespace Computadora3D
{
    [Serializable]
    public class Vertice
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vertice() { }

        public Vertice(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
