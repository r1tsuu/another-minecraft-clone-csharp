using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MC
{
  public class Game
  {
    private readonly int _windowWidth = 800;
    private readonly int _windowHeight = 600;

    private readonly GameWindow _window;
    private readonly Shader _shader;
    private readonly Texture _texture1;
    private readonly Texture _texture2;
    private readonly Shape _rectangle1;
    private readonly Shape _rectangle2;
    private readonly Matrix4 _projection;
    private Matrix4 _model1;
    private Matrix4 _model2;
    private Matrix4 _view;

    private double _renderTime;

    public Game()
    {
      var windowSettings = new NativeWindowSettings()
      {
        Size = (_windowWidth, _windowHeight),
        Title = "Minecraft Clone"
      };
      _window = new(GameWindowSettings.Default, windowSettings);
      _shader = new("Shaders/base.vert", "Shaders/base.frag");
      _texture1 = new("Resources/container.png");
      _texture2 = new("Resources/awesomeface.png");
      _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), _windowWidth / _windowHeight, 0.1f, 100.0f);
      _model1 = Matrix4.CreateTranslation((1f, 0.0f, 0f));
      _model2 = Matrix4.CreateTranslation((-1f, 0.0f, 0f));
      _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
      _rectangle1 = Shape.CreateRectangle(_texture1, _shader);
      _rectangle2 = Shape.CreateRectangle(_texture2, _shader);
    }

    private void HandleWindowLoad()
    {
      GL.ClearColor(0f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      _shader.Initialize();
      _texture1.Initialize();
      _texture2.Initialize();
      _rectangle1.Initialize();
      _rectangle2.Initialize();
    }


    private void HandleWindowRenderFrame(FrameEventArgs e)
    {
      _renderTime += e.Time;
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _shader.SetMatrix4("view", _view);
      _shader.SetMatrix4("projection", _projection);

      _model1 = Matrix4.CreateTranslation((float)Math.Cos(_renderTime), (float)Math.Sin(_renderTime), 0.0f) * Matrix4.CreateRotationZ((float)Math.Cos(_renderTime));
      _shader.SetMatrix4("model", _model1);
      _rectangle1.Draw();
      _model2 = Matrix4.CreateTranslation((float)Math.Sin(_renderTime), (float)Math.Cos(_renderTime), 0.0f) * Matrix4.CreateRotationZ((float)Math.Sin(_renderTime));
      _shader.SetMatrix4("model", _model2);
      _rectangle2.Draw();

      _window.SwapBuffers();
    }

    public Game Initialize()
    {
      _window.Load += HandleWindowLoad;
      _window.RenderFrame += HandleWindowRenderFrame;
      return this;
    }

    public void Run()
    {
      _window.Run();
    }
  }
}
