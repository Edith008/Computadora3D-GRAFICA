using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System;

namespace Computadora3D
{
    public class GameWindow : OpenTK.Windowing.Desktop.GameWindow
    {
        private Objeto monitor;
        private Objeto cpu;
        private Objeto teclado;

        private float rotationAngle = 0.0f;
        private Matrix4 projection;
        private Matrix4 view;
        private bool glInitialized = false;

        public GameWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Configurar matrices de proyección y vista
            projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)Size.X / (float)Size.Y,
                0.1f,
                100.0f);

            view = Matrix4.LookAt(
                new Vector3(0.0f, 2.0f, 8.0f),
                Vector3.Zero,
                Vector3.UnitY);

            CrearComputadora();
            glInitialized = true;
            VerificarOpenGL();
        }

        private void VerificarOpenGL()
        {
            try
            {
                if (glInitialized)
                {
                    Console.WriteLine($"OpenGL Version: {GL.GetString(StringName.Version)}");
                    Console.WriteLine($"GPU: {GL.GetString(StringName.Renderer)}");
                    Console.WriteLine($"GLSL Version: {GL.GetString(StringName.ShadingLanguageVersion)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo información de OpenGL: {ex.Message}");
            }
        }

        private void CrearComputadora()
        {
            //********************************************************************
            // CREAR MONITOR *****************************************************
            //********************************************************************
            monitor = new Objeto("Monitor");

            // PANTALLA PRINCIPAL (BLANCA)
            Vector3[] coloresPantalla = new Vector3[]
            {
                new Vector3(0.0f, 0.0f, 0.0f),  // Frente (BLANCO)
                new Vector3(0.3f, 0.3f, 0.3f),  // Atrás (gris)
                new Vector3(0.5f, 0.5f, 0.5f),  // Izquierda (gris claro)
                new Vector3(0.5f, 0.5f, 0.5f),  // Derecha (gris claro)
                new Vector3(0.4f, 0.4f, 0.4f),  // Superior (gris medio)
                new Vector3(0.2f, 0.2f, 0.2f)   // Inferior (gris oscuro)
            };

            var pantallaMonitor = new Poligono(new Vector3(3.0f, 1.8f, 0.1f), coloresPantalla);
            monitor.AgregarPoligono(pantallaMonitor, new Vector3(0.0f, 1.0f, 0.0f)); // Subir la pantalla

            // SOPORTE DEL MONITOR (PARTE VERTICAL)
            Vector3[] coloresSoporteVertical = new Vector3[]
            {
                new Vector3(0.3f, 0.3f, 0.3f),  // Frente
                new Vector3(0.3f, 0.3f, 0.3f),  // Atrás
                new Vector3(0.3f, 0.3f, 0.3f),  // Izquierda
                new Vector3(0.3f, 0.3f, 0.3f),  // Derecha
                new Vector3(0.3f, 0.3f, 0.3f),  // Superior
                new Vector3(0.3f, 0.3f, 0.3f)   // Inferior
            };

            var soporteVertical = new Poligono(new Vector3(0.1f, 1.0f, 0.1f), coloresSoporteVertical);
            monitor.AgregarPoligono(soporteVertical, new Vector3(0.0f, 0.0f, 0.0f));

            // BASE DEL MONITOR (PARTE HORIZONTAL)
            Vector3[] coloresBase = new Vector3[]
            {
                new Vector3(0.4f, 0.4f, 0.4f),  // Frente
                new Vector3(0.4f, 0.4f, 0.4f),  // Atrás
                new Vector3(0.4f, 0.4f, 0.4f),  // Izquierda
                new Vector3(0.4f, 0.4f, 0.4f),  // Derecha
                new Vector3(0.4f, 0.4f, 0.4f),  // Superior
                new Vector3(0.4f, 0.4f, 0.4f)   // Inferior
            };

            var baseMonitor = new Poligono(new Vector3(1.0f, 0.1f, 0.8f), coloresBase);
            monitor.AgregarPoligono(baseMonitor, new Vector3(0.0f, -0.5f, 0.0f)); // Debajo del soporte

            //********************************************************************
            // CREAR CPU *********************************************************
            //********************************************************************
            cpu = new Objeto("CPU");

            Vector3[] coloresCPU = new Vector3[]
            {
                new Vector3(0.2f, 0.2f, 0.2f),  // Frente
                new Vector3(0.1f, 0.1f, 0.1f),  // Atrás
                new Vector3(0.25f, 0.25f, 0.25f), // Izquierda
                new Vector3(0.25f, 0.25f, 0.25f), // Derecha
                new Vector3(0.15f, 0.15f, 0.15f), // Superior
                new Vector3(0.3f, 0.3f, 0.3f)    // Inferior
            };

            var cajaCpu = new Poligono(new Vector3(1.0f, 2.0f, 2.0f), coloresCPU);
            cpu.AgregarPoligono(cajaCpu, new Vector3(3.0f, 0.0f, 0.0f));

            //********************************************************************
            // CREAR TECLADO *****************************************************
            //********************************************************************
            teclado = new Objeto("Teclado");

            Vector3[] coloresTeclado = new Vector3[]
            {
                new Vector3(0.1f, 0.1f, 0.1f),  // Frente
                new Vector3(0.05f, 0.05f, 0.05f), // Atrás
                new Vector3(0.15f, 0.15f, 0.15f), // Izquierda
                new Vector3(0.15f, 0.15f, 0.15f), // Derecha
                new Vector3(0.08f, 0.08f, 0.08f), // Superior
                new Vector3(0.2f, 0.2f, 0.2f)    // Inferior
            };

            var cuerpoTeclado = new Poligono(new Vector3(4.0f, 0.2f, 1.5f), coloresTeclado);
            teclado.AgregarPoligono(cuerpoTeclado, new Vector3(0.0f, -1.5f, 0.0f));
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Matriz de modelo con rotación
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotationAngle));

            // Dibujar los objetos
            monitor.Dibujar(model, view, projection);
            cpu.Dibujar(model, view, projection);
            teclado.Dibujar(model, view, projection);

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // Rotación manual con flechas
            if (KeyboardState.IsKeyDown(Keys.Right))
                rotationAngle += 0.5f;
            if (KeyboardState.IsKeyDown(Keys.Left))
                rotationAngle -= 0.5f;

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);

            projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)Size.X / (float)Size.Y,
                0.1f,
                100.0f);
        }

        protected override void OnUnload()
        {
            monitor?.Dispose();
            cpu?.Dispose();
            teclado?.Dispose();
            base.OnUnload();
        }
    }
}