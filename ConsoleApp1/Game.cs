using OpenTK.Windowing.Desktop;
using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;


namespace ConsoleApp1
{
    internal class Game : GameWindow

    {
        private float _rotationAngle = 0.0f;
        private int _vertexBufferObject = -1;
        private int _vertexArrayObject = -1;
        private int _shaderProgramWhite;
        private int _shaderProgramColored;
        private readonly float[] _vertices = {
       // Barra horizontal de la T - Front face (rojo)
    -0.5f,  0.5f,  0.1f,  1.0f, 0.0f, 0.0f,  // Top-left
     0.5f,  0.5f,  0.1f,  1.0f, 0.0f, 0.0f,  // Top-right
     0.5f,  0.3f,  0.1f,  1.0f, 0.0f, 0.0f,  // Bottom-right
    -0.5f,  0.3f,  0.1f,  1.0f, 0.0f, 0.0f,  // Bottom-left

    // Barra vertical de la T - Front face (azul)
    -0.1f,  0.3f,  0.1f,  0.0f, 0.0f, 1.0f,  // Top-left
     0.1f,  0.3f,  0.1f,  0.0f, 0.0f, 1.0f,  // Top-right
     0.1f, -0.5f,  0.1f,  0.0f, 0.0f, 1.0f,  // Bottom-right
    -0.1f, -0.5f,  0.1f,  0.0f, 0.0f, 1.0f,  // Bottom-left

    // Back face (verde)
    -0.5f,  0.5f, -0.1f,  0.0f, 1.0f, 0.0f,  // Top-left
     0.5f,  0.5f, -0.1f,  0.0f, 1.0f, 0.0f,  // Top-right
     0.5f,  0.3f, -0.1f,  0.0f, 1.0f, 0.0f,  // Bottom-right
    -0.5f,  0.3f, -0.1f,  0.0f, 1.0f, 0.0f,  // Bottom-left

    -0.1f,  0.3f, -0.1f,  1.0f, 1.0f, 0.0f,  // Top-left
     0.1f,  0.3f, -0.1f,  1.0f, 1.0f, 0.0f,  // Top-right
     0.1f, -0.5f, -0.1f,  1.0f, 1.0f, 0.0f,  // Bottom-right
    -0.1f, -0.5f, -0.1f,  1.0f, 1.0f, 0.0f,  // Bottom-left
        };

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

       
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(width, height));
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Configurar la T con colores
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            // Configurar el VBO para los vértices de la T
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // Configurar el EBO para los índices
            int elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // Configurar los atributos de los vértices
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Configurar el atributo de color
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Desvincular VAO
            GL.BindVertexArray(0);

            CreateShaderProgram();
            GL.UseProgram(_shaderProgramColored);  // Usa el shader que maneja colores

            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Crear la matriz de rotación
            _rotationAngle += (float)args.Time * 50.0f;
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(_rotationAngle));

            // Dibujar la T con colores
            GL.UseProgram(_shaderProgramColored); // Usa el shader para colores
            int modelLocation = GL.GetUniformLocation(_shaderProgramColored, "model");
            GL.UniformMatrix4(modelLocation, false, ref model);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            if (_vertexBufferObject != -1)
                GL.DeleteBuffer(_vertexBufferObject);
            if (_vertexArrayObject != -1)
                GL.DeleteVertexArray(_vertexArrayObject);
        }

        private int _shaderProgram;

        private string vertexShaderSource = @"
         #version 330 core
    layout(location = 0) in vec3 aPosition;
    layout(location = 1) in vec3 aColor;

    out vec3 ourColor;

    uniform mat4 model;

    void main()
    {
        gl_Position = model * vec4(aPosition, 1.0);
        ourColor = aColor; // Pasamos el color al fragment shader
    }
         ";

        private string fragmentShaderWhiteSource = @"
    #version 330 core
    in vec3 ourColor;
    out vec4 FragColor;

    void main()
    {
        FragColor = vec4(1.0, 1.0, 1.0, 1.0); // Colorea las caras en blanco
    }
";
        private string fragmentShaderColoredSource = @"
    #version 330 core
    in vec3 ourColor;
    out vec4 FragColor;

    void main()
    {
        FragColor = vec4(ourColor, 1.0); // Aplica el color de la arista
    }
";

        private void CreateShaderProgram()
        {
            // Vertex Shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            // Fragment Shader White
            int fragmentShaderWhite = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderWhite, fragmentShaderWhiteSource);
            GL.CompileShader(fragmentShaderWhite);

            // Fragment Shader Colored
            int fragmentShaderColored = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderColored, fragmentShaderColoredSource);
            GL.CompileShader(fragmentShaderColored);

            // Shader Program for White (Faces)
            _shaderProgramWhite = GL.CreateProgram();
            GL.AttachShader(_shaderProgramWhite, vertexShader);
            GL.AttachShader(_shaderProgramWhite, fragmentShaderWhite);
            GL.LinkProgram(_shaderProgramWhite);

            // Shader Program for Colored (Edges)
            _shaderProgramColored = GL.CreateProgram();
            GL.AttachShader(_shaderProgramColored, vertexShader);
            GL.AttachShader(_shaderProgramColored, fragmentShaderColored);
            GL.LinkProgram(_shaderProgramColored);

            // Cleanup
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShaderWhite);
            GL.DeleteShader(fragmentShaderColored);
        }

    }
}
