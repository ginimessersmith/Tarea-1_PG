using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;


namespace ConsoleApp1
{
    internal class Game : GameWindow

    {
        
        private ObjetoT _objetoT;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(width, height));
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // Crear el objeto T con un centro en (0.5f, 0.5f, 0.0f)
            _objetoT = new ObjetoT(new Vector3(0.5f, 0.5f, 0.0f), 1.0f, 1.0f, 0.2f);
            _objetoT.Inicializar();
            _objetoT.SetPosition(new Vector3(1.0f, 1.0f, 0.0f));
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _objetoT.Dibujar((float)args.Time);

            Context.SwapBuffers();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _objetoT.Unload();
        }


    }
}
