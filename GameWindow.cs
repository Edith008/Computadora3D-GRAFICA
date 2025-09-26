using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Computadora3D
{
    class MiGameWindow : GameWindow
    {
        public Escenario Escenario { get; private set; } = new Escenario();
        private readonly string rutaJSON;

        private int _shaderProgram;
        private int _vao;
        private int _vbo;

        // 📌 Variables de cámara
        private Vector3 camPos = new Vector3(3, 2, 5);
        private Vector3 camTarget = Vector3.Zero;
        //private float camSpeed = 0.1f; // velocidad de movimiento
        //private float rotationAngle = 0f; // en radianes


        public MiGameWindow(int width, int height, string title, string rutaJSON)
            : base(GameWindowSettings.Default, new NativeWindowSettings
            {
                Size = new Vector2i(width, height),
                Title = title
            })
        {
            this.rutaJSON = rutaJSON;
        }

        public void CargarEscenario()
        {
            if (!File.Exists(rutaJSON))
            {
                Console.WriteLine("Archivo JSON no encontrado: " + rutaJSON);
                return;
            }

            try
            {
                string json = File.ReadAllText(rutaJSON);
                var cargado = JsonSerializer.Deserialize<Escenario>(json);

                if (cargado != null)
                {
                    Escenario = cargado;
                    Escenario.ObjetosDiccionario = new Dictionary<string, Objeto>();

                    foreach (var objeto in Escenario.Objetos)
                    {
                        // Guardar objeto en diccionario
                        Escenario.ObjetosDiccionario[objeto.Nombre.Trim()] = objeto;

                        Console.WriteLine($"  Objeto: {objeto.Nombre}, Partes: {objeto.Partes.Count}");

                        // Construir diccionario de partes
                        objeto.PartesDiccionario = new Dictionary<string, Partes>();
                        foreach (var parte in objeto.Partes)
                        {
                            objeto.PartesDiccionario[parte.Nombre.Trim()] = parte;

                            Console.WriteLine($"    Parte: {parte.Nombre}, Caras: {parte.Caras.Count}");
                            foreach (var cara in parte.Caras)
                            {
                                Console.WriteLine($"      Cara: {cara.Nombre}, Vértices: {cara.Vertices.Count}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar JSON: " + ex.Message);
            }
        }


        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            CargarEscenario();

            // Shaders
            string vertexShaderSource = @"
                #version 330 core
                layout(location = 0) in vec3 aPosition;
                layout(location = 1) in vec3 aColor;
                out vec3 vertexColor;
                uniform mat4 model;
                uniform mat4 view;
                uniform mat4 projection;
                void main()
                {
                    vertexColor = aColor;
                    gl_Position = projection * view * model * vec4(aPosition, 1.0);
                }
            ";

            string fragmentShaderSource = @"
                #version 330 core
                in vec3 vertexColor;
                out vec4 FragColor;
                void main()
                {
                    FragColor = vec4(vertexColor, 1.0);
                }
            ";

            _shaderProgram = CrearShader(vertexShaderSource, fragmentShaderSource);
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
                Close();

            if (input.IsKeyPressed(Keys.R))
                Escenario.AplicarRotacion(0.5f);

            if (input.IsKeyPressed(Keys.T))
                Escenario.AplicarTraslacion(new Vector3(0.1f, 0f, 0f));

            if (input.IsKeyPressed(Keys.E))
                Escenario.AplicarEscalado(new Vector3(1.1f));

            if (input.IsKeyPressed(Keys.F))
                Escenario.CambiarProyeccion();

            var monitor = Escenario.GetObjeto("Monitor");
            if (monitor != null)
            {
                if (input.IsKeyPressed(Keys.M))
                    monitor.AplicarRotacion(0.5f);

                if (input.IsKeyPressed(Keys.N)) // 
                    monitor.AplicarTraslacion(new Vector3(1f, 0f, 0f));
                //if (input.IsKeyDown(Keys.M))
                //    monitor.AplicarTraslacion(new Vector3((float)(2.0f * args.Time), 0f, 0f));

                if (input.IsKeyPressed(Keys.B))
                    monitor.AplicarEscalado(new Vector3(1.1f));

                if (input.IsKeyPressed(Keys.V))
                    monitor.CambiarProyeccion();
            }

            var pantalla = monitor?.GetParte("pantalla");
            if (pantalla != null)
            {
                if (input.IsKeyPressed(Keys.P)) // tecla para rotar pantalla
                    pantalla.AplicarRotacion(MathHelper.DegreesToRadians(45f));

                if (input.IsKeyPressed(Keys.O)) // 
                    pantalla.AplicarTraslacion(new Vector3(1f, 0f, 0f));

                if (input.IsKeyPressed(Keys.I))
                    pantalla.AplicarEscalado(new Vector3(1.1f));

                if (input.IsKeyPressed(Keys.U))
                    pantalla.CambiarProyeccion();
            }

            var soporte = monitor?.GetParte("Soporte");
            if (soporte != null)
            {
                if (input.IsKeyPressed(Keys.A)) // tecla para rotar pantalla
                    soporte.AplicarRotacion(MathHelper.DegreesToRadians(45f));

                if (input.IsKeyPressed(Keys.S)) // 
                    soporte.AplicarTraslacion(new Vector3(1f, 0f, 0f));

                if (input.IsKeyPressed(Keys.D))
                    soporte.AplicarEscalado(new Vector3(1.1f));

                if (input.IsKeyPressed(Keys.Q))
                    soporte.CambiarProyeccion();
            }


        }




        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Dibujar();

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        private void ConfigurarMatrices(Matrix4 model)
        {
            Matrix4 projection;

            if (Escenario.UsarProyeccionPerspectiva)
            {
                projection = Matrix4.CreatePerspectiveFieldOfView(
                    MathHelper.DegreesToRadians(60f),
                    Size.X / (float)Size.Y,
                    0.1f,
                    100f
                );
            }
            else
            {
                float aspect = Size.X / (float)Size.Y;
                projection = Matrix4.CreateOrthographic(10f * aspect, 10f, 0.1f, 100f);
            }

            Matrix4 view = Matrix4.LookAt(camPos, camTarget, Vector3.UnitY);

            GL.UseProgram(_shaderProgram);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref projection);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref view);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref model);
        }


        private Vector3 ObtenerCentroEscenario()
        {
            if (Escenario == null) return Vector3.Zero;
            return new Vector3((float)Escenario.Centro[0], (float)Escenario.Centro[1],(float)Escenario.Centro[2]);
        }
        private void Dibujar()
        {
            if (Escenario == null) return;

            Vector3 centro = ObtenerCentroEscenario();
            Matrix4 escenarioModel = Matrix4.CreateTranslation(-(float)centro.X, -(float)centro.Y, -(float)centro.Z);
            Matrix4 transformaciones = Matrix4.CreateScale(Escenario.Escalado) *
                                       Matrix4.CreateRotationY(Escenario.RotacionY) *
            Matrix4.CreateTranslation(Escenario.Traslacion);

            foreach (var objeto in Escenario.Objetos)
            {
                Matrix4 objetoModel = Matrix4.CreateTranslation(
                    (float)objeto.Posicion[0],
                    (float)objeto.Posicion[1],
                    (float)objeto.Posicion[2]
                ) * escenarioModel;

                //Matrix4 objetoTransform = Matrix4.CreateScale(objeto.Escalado) *
                //          Matrix4.CreateRotationY(objeto.RotacionY) *
                //          Matrix4.CreateTranslation(objeto.Traslacion);


                            Matrix4 objetoTransform =
                Matrix4.CreateTranslation((float)objeto.Centro[0], (float)objeto.Centro[1], (float)objeto.Centro[2]) *
                (Matrix4.CreateScale(objeto.Escalado) *
                 Matrix4.CreateRotationY(objeto.RotacionY) *
                 Matrix4.CreateTranslation(objeto.Traslacion)) *
                Matrix4.CreateTranslation(-(float)objeto.Centro[0], -(float)objeto.Centro[1], -(float)objeto.Centro[2]);


                foreach (var parte in objeto.Partes)
                {
                    Matrix4 parteModel = Matrix4.CreateTranslation(
                        (float)parte.Posicion[0],
                        (float)parte.Posicion[1],
                        (float)parte.Posicion[2]
                    ) * objetoModel;

                    //Matrix4 parteTransform = Matrix4.CreateScale(parte.Escalado) *
                    //                Matrix4.CreateRotationY(parte.RotacionY) *
                    //                Matrix4.CreateTranslation(parte.Traslacion);

                    Matrix4 parteTransform =
                    Matrix4.CreateTranslation((float)parte.Centro[0], (float)parte.Centro[1], (float)parte.Centro[2]) *
                    (Matrix4.CreateScale(parte.Escalado) *
                     Matrix4.CreateRotationY(parte.RotacionY) *
                     Matrix4.CreateTranslation(parte.Traslacion)) *
                    Matrix4.CreateTranslation(-(float)parte.Centro[0], -(float)parte.Centro[1], -(float)parte.Centro[2]);


                    Matrix4 modelFinal = parteModel * objetoModel * escenarioModel;

                    //ConfigurarMatrices(parteModel * transformaciones); 
                    ConfigurarMatrices(parteModel * parteTransform * objetoTransform* transformaciones );
                    //ConfigurarMatrices( modelFinal);


                    foreach (var cara in parte.Caras)
                    {
                        if (cara.Vertices.Count < 3) continue;

                        List<float> dataList = new();
                        for (int i = 0; i < cara.Vertices.Count - 2; i++)
                        {
                            var v0 = cara.Vertices[0];
                            var v1 = cara.Vertices[i + 1];
                            var v2 = cara.Vertices[i + 2];
                            float[] color = cara.Color;

                            dataList.AddRange(new float[] { (float)v0.X, (float)v0.Y, (float)v0.Z, color[0], color[1], color[2] });
                            dataList.AddRange(new float[] { (float)v1.X, (float)v1.Y, (float)v1.Z, color[0], color[1], color[2] });
                            dataList.AddRange(new float[] { (float)v2.X, (float)v2.Y, (float)v2.Z, color[0], color[1], color[2] });
                        }

                        float[] data = dataList.ToArray();

                        GL.BindVertexArray(_vao);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                        GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.DynamicDraw);

                        GL.EnableVertexAttribArray(0);
                        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

                        GL.EnableVertexAttribArray(1);
                        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

                        GL.DrawArrays(PrimitiveType.Triangles, 0, data.Length / 6);
                        GL.BindVertexArray(0);
                    }
                }
            }
        }


        private int CrearShader(string vertexSrc, string fragmentSrc)
        {
            int vertex = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertex, vertexSrc);
            GL.CompileShader(vertex);
            GL.GetShader(vertex, ShaderParameter.CompileStatus, out int vStatus);
            if (vStatus != 1) Console.WriteLine(GL.GetShaderInfoLog(vertex));

            int fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragment, fragmentSrc);
            GL.CompileShader(fragment);
            GL.GetShader(fragment, ShaderParameter.CompileStatus, out int fStatus);
            if (fStatus != 1) Console.WriteLine(GL.GetShaderInfoLog(fragment));

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertex);
            GL.AttachShader(program, fragment);
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
            if (status != 1) Console.WriteLine(GL.GetProgramInfoLog(program));

            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);

            return program;
        }
    }
}
