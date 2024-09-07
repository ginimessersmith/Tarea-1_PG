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

        public Game(int width, int height)
           : base(width, height, GraphicsMode.Default, "OpenTK Window")
        {
            VSync = VSyncMode.On; 
        }

        protected override void OnLoad(EventArgs e)
        {
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

            AdministradorObjetos administradorObjetos = new AdministradorObjetos();

            string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "escenario.json");
            Console.WriteLine("Buscando archivo en: " + rutaArchivo);

            if (File.Exists(rutaArchivo))
            {
            
                escenario = administradorObjetos.CargarObjetos(rutaArchivo);

                if (escenario.GetObjeto("letraT") != null) 
                {
                    Objeto letraT = escenario.GetObjeto("letraT");
                    letraT.SetCentroDeMasa(new Punto(1.0f, 2.0f, 1.0f)); 
                }
                else
                {
                    Console.WriteLine("No se encontró el objeto letraT.");
                }
            }
            else{
                Console.WriteLine("no encontro el archivo");
                escenario = new Escenario();
            }

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

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            
        }


    }
}
