
using System;
using OpenTK.Windowing.Desktop;

namespace Computadora3D
{
    class Program
    {
        static void Main()
        {
            string rutaJSON = @"C:\Users\Acer\Desktop\Grafica2025\Computadora3D\Archivo.json";

            using var window = new MiGameWindow(800, 600, "Computadora 3D", rutaJSON);
            window.Run();
        }
    }
}

