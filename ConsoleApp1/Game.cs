using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace ConsoleApp1
{
    internal class Game : GameWindow

    {

        private Escenario escenario;
        private float angle = 0.0f;
        private bool isMouseDown = false; // Para rastrear si el botón del mouse está presionado
        private Vector2 lastMousePos; // Última posición del mouse
        private float pitch = 0.0f; // Rotación alrededor del eje X
        private float yaw = 0.0f;   // Rotación alrededor del eje Y
        private float zoom = 2.0f;  // Distancia de la cámara al objeto

        public Game(int width, int height)
           : base(width, height, GraphicsMode.Default, "OpenTK Window")
        {
            VSync = VSyncMode.On; // Habilitar VSync para evitar el tearing
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Configuración de la proyección en perspectiva
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                Width / (float)Height,
                0.1f,  // near plane
                100.0f // far plane
            );
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            List<Punto> puntosDelCubo = new List<Punto>
    {
        new Punto(-0.5f, -0.5f, -0.5f),
        new Punto( 0.5f, -0.5f, -0.5f),
        new Punto( 0.5f,  0.5f, -0.5f),
        new Punto(-0.5f,  0.5f, -0.5f),
        new Punto(-0.5f, -0.5f,  0.5f),
        new Punto( 0.5f, -0.5f,  0.5f),
        new Punto( 0.5f,  0.5f,  0.5f),
        new Punto(-0.5f,  0.5f,  0.5f)
    };

            // Crear las caras del cubo como polígonos
            List<Poligono> carasDelCubo = new List<Poligono>
    {
        new Poligono(new List<Punto> { puntosDelCubo[0], puntosDelCubo[1], puntosDelCubo[2], puntosDelCubo[3] }, 1.0f, 0.0f, 0.0f), // Rojo
        new Poligono(new List<Punto> { puntosDelCubo[4], puntosDelCubo[5], puntosDelCubo[6], puntosDelCubo[7] }, 0.0f, 1.0f, 0.0f), // Verde
        new Poligono(new List<Punto> { puntosDelCubo[0], puntosDelCubo[1], puntosDelCubo[5], puntosDelCubo[4] }, 0.0f, 0.0f, 1.0f), // Azul
        new Poligono(new List<Punto> { puntosDelCubo[2], puntosDelCubo[3], puntosDelCubo[7], puntosDelCubo[6] }, 1.0f, 1.0f, 0.0f), // Amarillo
        new Poligono(new List<Punto> { puntosDelCubo[0], puntosDelCubo[3], puntosDelCubo[7], puntosDelCubo[4] }, 1.0f, 0.0f, 1.0f), // Magenta
        new Poligono(new List<Punto> { puntosDelCubo[1], puntosDelCubo[2], puntosDelCubo[6], puntosDelCubo[5] }, 0.0f, 1.0f, 1.0f)  // Cyan
    };

            List<Punto> puntosDeLaT = new List<Punto>
{
    // Parte superior de la T (Rectángulo Horizontal - Frente)
    new Punto(-1.0f,  0.5f,  0.1f),  // Frente - Superior Izquierda
    new Punto( 1.0f,  0.5f,  0.1f),  // Frente - Superior Derecha
    new Punto( 1.0f,  1.0f,  0.1f),  // Frente - Inferior Derecha
    new Punto(-1.0f,  1.0f,  0.1f),  // Frente - Inferior Izquierda

    // Tronco de la T (Rectángulo Vertical - Frente)
    new Punto(-0.5f, -1.0f,  0.1f), // Frente - Inferior Izquierda
    new Punto( 0.5f, -1.0f,  0.1f), // Frente - Inferior Derecha
    new Punto( 0.5f,  0.5f,  0.1f), // Frente - Superior Derecha
    new Punto(-0.5f,  0.5f,  0.1f), // Frente - Superior Izquierda

    // Parte superior de la T (Rectángulo Horizontal - Atrás)
    new Punto(-1.0f,  0.5f, -0.1f),  // Atrás - Superior Izquierda
    new Punto( 1.0f,  0.5f, -0.1f),  // Atrás - Superior Derecha
    new Punto( 1.0f,  1.0f, -0.1f),  // Atrás - Inferior Derecha
    new Punto(-1.0f,  1.0f, -0.1f),  // Atrás - Inferior Izquierda

    // Tronco de la T (Rectángulo Vertical - Atrás)
    new Punto(-0.5f, -1.0f, -0.1f), // Atrás - Inferior Izquierda
    new Punto( 0.5f, -1.0f, -0.1f), // Atrás - Inferior Derecha
    new Punto( 0.5f,  0.5f, -0.1f), // Atrás - Superior Derecha
    new Punto(-0.5f,  0.5f, -0.1f), // Atrás - Superior Izquierda
};


            List<Poligono> carasDeLaT = new List<Poligono>
{
    // Parte superior de la T (Rectángulo Horizontal - Frente)
    new Poligono(new List<Punto> { puntosDeLaT[0], puntosDeLaT[1], puntosDeLaT[2], puntosDeLaT[3] }, 1.0f, 0.0f, 0.0f), // Rojo

    // Tronco de la T (Rectángulo Vertical - Frente)
    new Poligono(new List<Punto> { puntosDeLaT[4], puntosDeLaT[5], puntosDeLaT[6], puntosDeLaT[7] }, 0.0f, 1.0f, 0.0f), // Verde

    // Parte superior de la T (Rectángulo Horizontal - Atrás)
    new Poligono(new List<Punto> { puntosDeLaT[8], puntosDeLaT[9], puntosDeLaT[10], puntosDeLaT[11] }, 1.0f, 0.0f, 0.0f), // Rojo

    // Tronco de la T (Rectángulo Vertical - Atrás)
    new Poligono(new List<Punto> { puntosDeLaT[12], puntosDeLaT[13], puntosDeLaT[14], puntosDeLaT[15] }, 0.0f, 1.0f, 0.0f), // Verde

    // Conexiones laterales
    // Lado derecho superior de la T
    new Poligono(new List<Punto> { puntosDeLaT[1], puntosDeLaT[9], puntosDeLaT[10], puntosDeLaT[2] }, 0.0f, 0.0f, 1.0f), // Azul
    // Lado izquierdo superior de la T
    new Poligono(new List<Punto> { puntosDeLaT[0], puntosDeLaT[8], puntosDeLaT[11], puntosDeLaT[3] }, 1.0f, 1.0f, 0.0f), // Amarillo
    // Lado derecho del tronco de la T
    new Poligono(new List<Punto> { puntosDeLaT[5], puntosDeLaT[13], puntosDeLaT[14], puntosDeLaT[6] }, 0.0f, 1.0f, 1.0f), // Cyan
    // Lado izquierdo del tronco de la T
    new Poligono(new List<Punto> { puntosDeLaT[4], puntosDeLaT[12], puntosDeLaT[15], puntosDeLaT[7] }, 1.0f, 0.0f, 1.0f), // Magenta

    // Conexión entre la parte superior y el tronco (cara inferior)
    new Poligono(new List<Punto> { puntosDeLaT[3], puntosDeLaT[11], puntosDeLaT[15], puntosDeLaT[7] }, 1.0f, 0.5f, 0.5f), // Naranja
    
    //new Poligono(new List<Punto> { puntosDeLaT[0], puntosDeLaT[8], puntosDeLaT[12], puntosDeLaT[4] }, 0.5f, 1.0f, 0.5f), // Verde claro
};


            Objeto letraT = new Objeto();
            foreach (var cara in carasDeLaT)
            {
                letraT.AddPoligono(cara);
            }

            // Crear el objeto cubo
            Objeto cubo = new Objeto();
            foreach (var cara in carasDelCubo)
            {
                cubo.AddPoligono(cara);
            }

            // Establecer el centro de masa del cubo
            cubo.SetCentroDeMasa(new Punto(2.0f, 0.0f, 0.0f));

            Objeto cubo2 = new Objeto();
            foreach (var cara in carasDelCubo)
            {
                cubo2.AddPoligono(cara);
            }
            cubo2.SetCentroDeMasa(new Punto(2.0f, 1.0f, 0.0f));

            Objeto cubo3 = new Objeto();
            foreach (var cara in carasDelCubo)
            {
                cubo3.AddPoligono(cara);
            }
            cubo3.SetCentroDeMasa(new Punto(1.0f, 2.0f, 0.0f));

            Objeto cubo4 = new Objeto();
            foreach (var cara in carasDelCubo)
            {
                cubo4.AddPoligono(cara);
            }
            cubo4.SetCentroDeMasa(new Punto(3.0f, 2.0f, 0.0f));



            Objeto cubo5 = new Objeto();
            foreach (var cara in carasDelCubo)
            {
                cubo5.AddPoligono(cara);
            }
            cubo5.SetCentroDeMasa(new Punto(2.0f, 2.0f, 0.0f));

            letraT.SetCentroDeMasa(new Punto(-2.0f, 3.0f, 0.0f));


            // Crear el escenario y agregar ambos objetos
            escenario = new Escenario();
            escenario.AddObjeto(cubo);
            escenario.AddObjeto(cubo2);
            escenario.AddObjeto(cubo3);
            escenario.AddObjeto(cubo4);
            escenario.AddObjeto(cubo5);
            escenario.AddObjeto(letraT);
            // agregar la clase parte el objeto se debe descomponer en partes, las partes son poligonos


        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Establecer la matriz de modelo/vista
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Aplicar la traslación para el zoom
            GL.Translate(0.0f, 0.0f, -zoom);

            // Aplicar la rotación de la vista basada en el mouse
            GL.Rotate(pitch, 1.0f, 0.0f, 0.0f); // Rotar alrededor del eje X
            GL.Rotate(yaw, 0.0f, 1.0f, 0.0f);   // Rotar alrededor del eje Y

            // Dibujar los ejes cartesianos
            GL.Begin(PrimitiveType.Lines);

            // Eje X en rojo
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);

            // Eje Y en verde
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            // Eje Z en azul
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);

            GL.End();

            // Dibujar el escenario (y por ende el cubo)
            escenario.DibujarEscenario();
            Console.WriteLine($"Zoom: {zoom}, Pitch: {pitch}, Yaw: {yaw}");

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

            // Ajustar la distancia de la cámara (zoom) en función del scroll del mouse
            zoom -= e.DeltaPrecise * 0.5f;

            // Limitar el zoom para evitar que pase a través del objeto o se aleje demasiado
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

                yaw += delta.X * 0.5f;  // Ajustar sensibilidad
                pitch += delta.Y * 0.5f; // Invertir si quieres mover el mouse hacia abajo para mover la cámara hacia arriba
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height); // Configurar el viewport al tamaño de la ventana
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            // Lógica de actualización, como manejar la entrada del usuario, se coloca aquí
        }


    }
}
