using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;


namespace ConsoleApp1
{
    internal class ObjetoT
    {
        private float _rotationAngle = 0.0f;    
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _shaderProgramColored;

        // Dimensiones de la T
        private float width = 1.0f;        // Ancho total de la barra horizontal
        private float height = 1.0f;       // Altura total de la T
        private float thickness = 0.2f;    // Grosor de las barras

        private Vector3 _center; // Variable para almacenar el centro de la T

        private float[] _vertices;

        private readonly uint[] _indices = {
            // Barra horizontal - Front face
             0, 1, 2,
             2, 3, 0,

            // Barra vertical - Front face
            4, 5, 6,
            6, 7, 4,

            // Back face
            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            // Conectar frente y atrás
             0, 1, 9,
            9, 8, 0,

            1, 2, 10,
            10, 9, 1,

            2, 3, 11,
            11, 10, 2,

            3, 0, 8,
            8, 11, 3,

            4, 5, 13,
            13, 12, 4,

            5, 6, 14,
            14, 13, 5,

            6, 7, 15,
            15, 14, 6,

            7, 4, 12,
            12, 15, 7,
        };
        public ObjetoT(Vector3 center, float width, float height, float thickness)
        {
            _center = center;
            this.width = width;
            this.height = height;
            this.thickness = thickness;
            CalculateVertices();
        }

        private void CalculateVertices()
        {
            float halfWidth = width / 2.0f;
            float halfHeight = height / 2.0f;
            float halfThickness = thickness / 2.0f;

            _vertices = new float[] {
                // Barra horizontal de la T - Front face
                _center.X - halfWidth, _center.Y + halfThickness, _center.Z + halfThickness,  1.0f, 0.0f, 0.0f,  // Top-left
                _center.X + halfWidth, _center.Y + halfThickness, _center.Z + halfThickness,  1.0f, 0.0f, 0.0f,  // Top-right
                _center.X + halfWidth, _center.Y - halfThickness, _center.Z + halfThickness,  1.0f, 0.0f, 0.0f,  // Bottom-right
                _center.X - halfWidth, _center.Y - halfThickness, _center.Z + halfThickness,  1.0f, 0.0f, 0.0f,  // Bottom-left

                // Barra vertical de la T - Front face
                _center.X - halfThickness, _center.Y - halfThickness, _center.Z + halfThickness,  0.0f, 0.0f, 1.0f,  // Top-left
                _center.X + halfThickness, _center.Y - halfThickness, _center.Z + halfThickness,  0.0f, 0.0f, 1.0f,  // Top-right
                _center.X + halfThickness, _center.Y - halfHeight, _center.Z + halfThickness,  0.0f, 0.0f, 1.0f,  // Bottom-right
                _center.X - halfThickness, _center.Y - halfHeight, _center.Z + halfThickness,  0.0f, 0.0f, 1.0f,  // Bottom-left

                // Barra horizontal de la T - Back face
                _center.X - halfWidth, _center.Y + halfThickness, _center.Z - halfThickness,  0.0f, 1.0f, 0.0f,  // Top-left
                _center.X + halfWidth, _center.Y + halfThickness, _center.Z - halfThickness,  0.0f, 1.0f, 0.0f,  // Top-right
                _center.X + halfWidth, _center.Y - halfThickness, _center.Z - halfThickness,  0.0f, 1.0f, 0.0f,  // Bottom-right
                _center.X - halfWidth, _center.Y - halfThickness, _center.Z - halfThickness,  0.0f, 1.0f, 0.0f,  // Bottom-left

                // Barra vertical de la T - Back face
                _center.X - halfThickness, _center.Y - halfThickness, _center.Z - halfThickness,  1.0f, 1.0f, 0.0f,  // Top-left
                _center.X + halfThickness, _center.Y - halfThickness, _center.Z - halfThickness,  1.0f, 1.0f, 0.0f,  // Top-right
                _center.X + halfThickness, _center.Y - halfHeight, _center.Z - halfThickness,  1.0f, 1.0f, 0.0f,  // Bottom-right
                _center.X - halfThickness, _center.Y - halfHeight, _center.Z - halfThickness,  1.0f, 1.0f, 0.0f,  // Bottom-left
            };
        }
        public void SetPosition(Vector3 newPosition)
        {
            _center = newPosition;
            CalculateVertices(); // Recalcula los vértices con el nuevo centro
        }

        public void Inicializar()
        {
            // Configurar los VBO, VAO, EBO y shaders
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindVertexArray(0);

            CreateShaderProgram();
        }

        public void Dibujar(float deltaTime)
        {
            _rotationAngle += deltaTime * 50.0f;
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotationAngle));

            GL.UseProgram(_shaderProgramColored);
            int modelLocation = GL.GetUniformLocation(_shaderProgramColored, "model");
            GL.UniformMatrix4(modelLocation, false, ref model);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }


        private void CreateShaderProgram()
        {
            string vertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPosition;
            layout(location = 1) in vec3 aColor;

            out vec3 ourColor;

            uniform mat4 model;

            void main()
            {
                gl_Position = model * vec4(aPosition, 1.0);
                ourColor = aColor;
            }
            ";

            string fragmentShaderColoredSource = @"
            #version 330 core
            in vec3 ourColor;
            out vec4 FragColor;

            void main()
            {
                FragColor = vec4(ourColor, 1.0);
            }
            ";

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShaderColored = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderColored, fragmentShaderColoredSource);
            GL.CompileShader(fragmentShaderColored);

            _shaderProgramColored = GL.CreateProgram();
            GL.AttachShader(_shaderProgramColored, vertexShader);
            GL.AttachShader(_shaderProgramColored, fragmentShaderColored);
            GL.LinkProgram(_shaderProgramColored);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShaderColored);
        }
        private void CheckShaderCompileStatus(int shader, string type)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine($"ERROR::{type}_SHADER_COMPILATION_ERROR\n{infoLog}");
            }
        }

        public void Unload()
        {
            if (_vertexBufferObject != -1)
                GL.DeleteBuffer(_vertexBufferObject);
            if (_vertexArrayObject != -1)
                GL.DeleteVertexArray(_vertexArrayObject);
        }

    }
}
