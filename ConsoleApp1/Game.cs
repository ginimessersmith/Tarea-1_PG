using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Xml.Linq;


namespace ConsoleApp1
{
    internal class Game : GameWindow

    {

        private Escenario escenario;
        private float angle = 0.0f;
        private float angleHorizontal = 0.0f; 
        private bool isMouseDown = false;
        private Vector2 lastMousePos; 
        private float pitch = 0.0f;
        private float yaw = 0.0f;
        private float zoom = 2.0f;
        private AdministradorObjetos administradorObjetos;

        public Game(int width, int height)
           : base(width, height, GraphicsMode.Default, "OpenTK Window")
        {
            VSync = VSyncMode.On;
            administradorObjetos = new AdministradorObjetos();
        }

        protected override void OnLoad(EventArgs e)
        {
            //Objeto cubo = new Objeto();
            escenario = new Escenario();
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                Width / (float)Height,
                0.1f,
                100.0f
            );
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            //Objeto piramide = CrearPiramide();
            //Objeto cubo = CrearCubo();
            Objeto piramide = new Objeto();
            Objeto cubo = new Objeto();
            //cubo.SetCentroDeMasa(new Punto(2.0f, 2.0f, 2.0f));

            //administradorObjetos.GuardarObjetos(cubo, "cubo.json");
            cubo = administradorObjetos.CargarDesdeJSON("cubo.json");
            piramide = administradorObjetos.CargarDesdeJSON("piramide.json");
            
            //cubo.SetCentroDeMasa(new Punto(-2.0f, -2.0f, 0.0f));
            piramide.SetCentroDeMasa(new Punto(-2.0f,0.0f,0.0f));
            cubo.SetCentroDeMasa(new Punto(2.0f, 0.0f, 0.0f));
            escenario.AddObjeto("Piramide", piramide);
            escenario.AddObjeto("Cubo", cubo);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(0.0f, 0.0f, -zoom);

            GL.Rotate(pitch, 1.0f, 0.0f, 0.0f);
            GL.Rotate(yaw, 0.0f, 1.0f, 0.0f);

            GL.Begin(PrimitiveType.Lines);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);

            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);

            GL.End();

            escenario.DibujarEscenario();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            base.OnUpdateFrame(e);

            float rotationSpeed = 10.0f; // Ajusta esta velocidad según sea necesario

            if (escenario.GetObjeto("Cubo") != null)
            {
                // Calcula el ángulo de rotación basado en el tiempo transcurrido
                float angle = rotationSpeed * (float)e.Time; // Ángulo en grados

                // Realiza la rotación
                escenario.GetObjeto("Cubo").Rotar(Vector3.UnitY, angle);
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Left)
            {
                isMouseDown = true;
                lastMousePos = new Vector2(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Left)
            {
                isMouseDown = false;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            zoom -= e.DeltaPrecise * 0.5f;

            if (zoom < 1.0f)
                zoom = 1.0f;
            if (zoom > 20.0f)
                zoom = 20.0f;
        }


        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (isMouseDown)
            {
                Vector2 delta = new Vector2(e.X, e.Y) - lastMousePos;
                lastMousePos = new Vector2(e.X, e.Y);

                yaw += delta.X * 0.5f; 
                pitch += delta.Y * 0.5f; 
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height); 
        }

        private Objeto CrearCubo()
        {
            Objeto cubo = new Objeto();

            // Definir los puntos para cada cara del cubo
            // Cara 1 (Frontal)
            Punto[] puntosCara1 = {
        new Punto(-0.5f, -0.5f, -0.5f),
        new Punto(0.5f, -0.5f, -0.5f),
        new Punto(0.5f, 0.5f, -0.5f),
        new Punto(-0.5f, 0.5f, -0.5f)
    };

            Parte cara1 = new Parte();
            cara1.AddPoligono(new Poligono(new List<Punto>(puntosCara1), 1.0f, 0.0f, 0.0f)); // Color rojo

            // Cara 2 (Trasera)
            Punto[] puntosCara2 = {
        new Punto(-0.5f, -0.5f, 0.5f),
        new Punto(0.5f, -0.5f, 0.5f),
        new Punto(0.5f, 0.5f, 0.5f),
        new Punto(-0.5f, 0.5f, 0.5f)
    };

            Parte cara2 = new Parte();
            cara2.AddPoligono(new Poligono(new List<Punto>(puntosCara2), 0.0f, 1.0f, 0.0f)); // Color verde

            // Cara 3 (Inferior)
            Punto[] puntosCara3 = {
        new Punto(-0.5f, -0.5f, -0.5f),
        new Punto(0.5f, -0.5f, -0.5f),
        new Punto(0.5f, -0.5f, 0.5f),
        new Punto(-0.5f, -0.5f, 0.5f)
    };

            Parte cara3 = new Parte();
            cara3.AddPoligono(new Poligono(new List<Punto>(puntosCara3), 0.0f, 0.0f, 1.0f)); // Color azul

            // Cara 4 (Superior)
            Punto[] puntosCara4 = {
        new Punto(-0.5f, 0.5f, -0.5f),
        new Punto(0.5f, 0.5f, -0.5f),
        new Punto(0.5f, 0.5f, 0.5f),
        new Punto(-0.5f, 0.5f, 0.5f)
    };

            Parte cara4 = new Parte();
            cara4.AddPoligono(new Poligono(new List<Punto>(puntosCara4), 1.0f, 1.0f, 0.0f)); // Color amarillo

            // Cara 5 (Lateral izquierda)
            Punto[] puntosCara5 = {
        new Punto(-0.5f, -0.5f, -0.5f),
        new Punto(-0.5f, 0.5f, -0.5f),
        new Punto(-0.5f, 0.5f, 0.5f),
        new Punto(-0.5f, -0.5f, 0.5f)
    };

            Parte cara5 = new Parte();
            cara5.AddPoligono(new Poligono(new List<Punto>(puntosCara5), 1.0f, 0.0f, 1.0f)); // Color magenta

            // Cara 6 (Lateral derecha)
            Punto[] puntosCara6 = {
        new Punto(0.5f, -0.5f, -0.5f),
        new Punto(0.5f, 0.5f, -0.5f),
        new Punto(0.5f, 0.5f, 0.5f),
        new Punto(0.5f, -0.5f, 0.5f)
    };

            Parte cara6 = new Parte();
            cara6.AddPoligono(new Poligono(new List<Punto>(puntosCara6), 0.0f, 1.0f, 1.0f)); // Color cyan

            // Agregar las caras al cubo
            cubo.AddParte("Cara1", cara1);
            cubo.AddParte("Cara2", cara2);
            cubo.AddParte("Cara3", cara3);
            cubo.AddParte("Cara4", cara4);
            cubo.AddParte("Cara5", cara5);
            cubo.AddParte("Cara6", cara6);

            return cubo;
        }

        private Objeto CrearPiramide()
        {
            Objeto piramide = new Objeto();

            // Definir los puntos para la base de la pirámide
            // Base (cuadrado)
            Punto[] puntosBase = {
        new Punto(-0.5f, -0.5f, -0.5f),
        new Punto(0.5f, -0.5f, -0.5f),
        new Punto(0.5f, -0.5f, 0.5f),
        new Punto(-0.5f, -0.5f, 0.5f)
    };

            Parte baseParte = new Parte();
            baseParte.AddPoligono(new Poligono(new List<Punto>(puntosBase), 1.0f, 0.0f, 0.0f)); // Color rojo

            // Definir los puntos para las caras triangulares
            // Cara 1 (Frontal)
            Punto[] puntosCara1 = {
        new Punto(-0.5f, -0.5f, -0.5f),
        new Punto(0.5f, -0.5f, -0.5f),
        new Punto(0.0f, 0.5f, 0.0f)
    };

            Parte cara1 = new Parte();
            cara1.AddPoligono(new Poligono(new List<Punto>(puntosCara1), 0.0f, 1.0f, 0.0f)); // Color verde

            // Cara 2 (Derecha)
            Punto[] puntosCara2 = {
        new Punto(0.5f, -0.5f, -0.5f),
        new Punto(0.5f, -0.5f, 0.5f),
        new Punto(0.0f, 0.5f, 0.0f)
    };

            Parte cara2 = new Parte();
            cara2.AddPoligono(new Poligono(new List<Punto>(puntosCara2), 0.0f, 0.0f, 1.0f)); // Color azul

            // Cara 3 (Trasera)
            Punto[] puntosCara3 = {
        new Punto(0.5f, -0.5f, 0.5f),
        new Punto(-0.5f, -0.5f, 0.5f),
        new Punto(0.0f, 0.5f, 0.0f)
    };

            Parte cara3 = new Parte();
            cara3.AddPoligono(new Poligono(new List<Punto>(puntosCara3), 1.0f, 1.0f, 0.0f)); // Color amarillo

            // Cara 4 (Izquierda)
            Punto[] puntosCara4 = {
        new Punto(-0.5f, -0.5f, 0.5f),
        new Punto(-0.5f, -0.5f, -0.5f),
        new Punto(0.0f, 0.5f, 0.0f)
    };

            Parte cara4 = new Parte();
            cara4.AddPoligono(new Poligono(new List<Punto>(puntosCara4), 1.0f, 0.0f, 1.0f)); // Color magenta

            // Agregar la base y las caras a la pirámide
            piramide.AddParte("Base", baseParte);
            piramide.AddParte("Cara1", cara1);
            piramide.AddParte("Cara2", cara2);
            piramide.AddParte("Cara3", cara3);
            piramide.AddParte("Cara4", cara4);

            return piramide;
        }

    }
}
