using Computadora3D;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace Computadora3D
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Computadora 3D ",
                // CONFIGURACIÓN PARA OPENGL 3.3
                //API = ContextAPI.OpenGL,
                //APIVersion = new Version(3, 3),  // ← OpenGL 3.3
                //Profile = ContextProfile.Core,
                //Flags = ContextFlags.ForwardCompatible
            };

            using (var window = new GameWindow(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}