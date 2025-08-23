using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;

namespace Computadora3D
{
    public class Poligono : IDisposable
    {
        private int vertexArrayObject;
        private int vertexBufferObject;
        private int shaderProgram;
        private bool shaderCompiled = false;
        private Vector3[] coloresCaras; // Colores para cada cara

        public Poligono(Vector3 size, Vector3[] coloresCaras)
        {
            if (coloresCaras.Length != 6)
                throw new ArgumentException("Debe proporcionar 6 colores (uno por cara)");

            this.coloresCaras = coloresCaras;

            float x = size.X / 2f;
            float y = size.Y / 2f;
            float z = size.Z / 2f;

            // 36 vértices (12 triángulos) con colores por cara
            float[] vertices = new float[36 * 6]; // 36 vértices × 6 componentes (3 posición + 3 color)

            int index = 0;

            // Cara frontal (0)
            AddCara(ref index, vertices, new Vector3[]
            {
                new Vector3(-x, -y,  z), new Vector3( x, -y,  z), new Vector3( x,  y,  z),
                new Vector3(-x, -y,  z), new Vector3( x,  y,  z), new Vector3(-x,  y,  z)
            }, coloresCaras[0]);

            // Cara trasera (1)
            AddCara(ref index, vertices, new Vector3[]
            {
                new Vector3(-x, -y, -z), new Vector3(-x,  y, -z), new Vector3( x,  y, -z),
                new Vector3(-x, -y, -z), new Vector3( x,  y, -z), new Vector3( x, -y, -z)
            }, coloresCaras[1]);

            // Cara izquierda (2)
            AddCara(ref index, vertices, new Vector3[]
            {
                new Vector3(-x, -y, -z), new Vector3(-x, -y,  z), new Vector3(-x,  y,  z),
                new Vector3(-x, -y, -z), new Vector3(-x,  y,  z), new Vector3(-x,  y, -z)
            }, coloresCaras[2]);

            // Cara derecha (3)
            AddCara(ref index, vertices, new Vector3[]
            {
                new Vector3( x, -y, -z), new Vector3( x,  y, -z), new Vector3( x,  y,  z),
                new Vector3( x, -y, -z), new Vector3( x,  y,  z), new Vector3( x, -y,  z)
            }, coloresCaras[3]);

            // Cara superior (4)
            AddCara(ref index, vertices, new Vector3[]
            {
                new Vector3(-x,  y,  z), new Vector3( x,  y,  z), new Vector3( x,  y, -z),
                new Vector3(-x,  y,  z), new Vector3( x,  y, -z), new Vector3(-x,  y, -z)
            }, coloresCaras[4]);

            // Cara inferior (5)
            AddCara(ref index, vertices, new Vector3[]
            {
                new Vector3(-x, -y,  z), new Vector3(-x, -y, -z), new Vector3( x, -y, -z),
                new Vector3(-x, -y,  z), new Vector3( x, -y, -z), new Vector3( x, -y,  z)
            }, coloresCaras[5]);

            InitializeGraphics(vertices);
            CompilarShader();
        }

        private void AddCara(ref int index, float[] vertices, Vector3[] posiciones, Vector3 color)
        {
            foreach (var pos in posiciones)
            {
                vertices[index++] = pos.X;
                vertices[index++] = pos.Y;
                vertices[index++] = pos.Z;
                vertices[index++] = color.X;
                vertices[index++] = color.Y;
                vertices[index++] = color.Z;
            }
        }

        private void InitializeGraphics(float[] vertices)
        {
            try
            {
                vertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(vertexArrayObject);

                vertexBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float),
                             vertices, BufferUsageHint.StaticDraw);

                // Posiciones (atributo 0)
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                // Colores (atributo 1)
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(1);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inicializando gráficos: {ex.Message}");
            }
        }

        private void CompilarShader()
        {
            try
            {
                string vertexShaderSource = @"
                    #version 330 core
                    layout (location = 0) in vec3 aPosition;
                    layout (location = 1) in vec3 aColor;
                    uniform mat4 model;
                    uniform mat4 view;
                    uniform mat4 projection;
                    out vec3 ourColor;
                    void main()
                    {
                        gl_Position = projection * view * model * vec4(aPosition, 1.0);
                        ourColor = aColor;
                    }";

                string fragmentShaderSource = @"
                    #version 330 core
                    out vec4 FragColor;
                    in vec3 ourColor;
                    void main()
                    {
                        FragColor = vec4(ourColor, 1.0);
                    }";

                int vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, vertexShaderSource);
                GL.CompileShader(vertexShader);

                int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, fragmentShaderSource);
                GL.CompileShader(fragmentShader);

                shaderProgram = GL.CreateProgram();
                GL.AttachShader(shaderProgram, vertexShader);
                GL.AttachShader(shaderProgram, fragmentShader);
                GL.LinkProgram(shaderProgram);

                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);

                shaderCompiled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error compilando shader: {ex.Message}");
            }
        }

        public void Dibujar(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            if (!shaderCompiled) return;

            GL.UseProgram(shaderProgram);

            int modelLocation = GL.GetUniformLocation(shaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(shaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(shaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, 1, false, ref model.Row0.X);
            GL.UniformMatrix4(viewLocation, 1, false, ref view.Row0.X);
            GL.UniformMatrix4(projectionLocation, 1, false, ref projection.Row0.X);

            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            if (vertexBufferObject != 0)
                GL.DeleteBuffer(vertexBufferObject);

            if (vertexArrayObject != 0)
                GL.DeleteVertexArray(vertexArrayObject);

            if (shaderCompiled && shaderProgram != 0)
                GL.DeleteProgram(shaderProgram);
        }
    }
}