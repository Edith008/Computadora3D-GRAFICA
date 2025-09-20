//using OpenTK.Windowing.Desktop;
//using OpenTK.Mathematics;

//namespace Computadora3D
//{
//    class Program
//    {
//        static void Main()
//        {
//            var nativeSettings = new NativeWindowSettings()
//            {
//                Size = new Vector2i(800, 600),
//                Title = "Computadora 3D"
//            };

//            using var window = new GameWindow(GameWindowSettings.Default, nativeSettings);
//            window.Run();
//        }
//    }
//}


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


//using OpenTK.Windowing.Desktop;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using OpenTK.Graphics.OpenGL4;

//var nativeSettings = new NativeWindowSettings()
//{
//    Size = new Vector2i(800, 600),
//    Title = "Computadora 3D",
//    API = ContextAPI.OpenGL,
//    APIVersion = new Version(3, 3), // o 2,1 para máxima compatibilidad
//    Profile = ContextProfile.Compatibility
//};

//using var window = new GameWindow(GameWindowSettings.Default, nativeSettings);
//window.Run();

