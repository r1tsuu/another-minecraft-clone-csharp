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
    private readonly Texture _texture0;
    private readonly Texture _texture1;
    private readonly Shape _rectangle;
    private readonly Matrix4 _projection;
    private Matrix4 _model;
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
      _texture0 = new("Resources/container.png");
      _texture1 = new("Resources/awesomeface.png");
      _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), _windowWidth / _windowHeight, 0.1f, 100.0f);
      _model = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45)) * Matrix4.CreateTranslation((0.5f, 0.3f, 0f));
      _view = Matrix4.CreateTranslation(1.0f, 0.0f, -3.0f);
      _rectangle = Shape.CreateRectangle(_texture0, _shader);
    }

    private void HandleWindowLoad()
    {
      GL.ClearColor(0f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      _shader.Initialize();
      _texture0.Initialize();
      _texture1.Initialize();
      _rectangle.Initialize();
    }

    bool setTextureFlag = false;

    private void HandleWindowRenderFrame(FrameEventArgs e)
    {
      _renderTime += e.Time;
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      _rectangle.Draw();

      if (_renderTime > 5 && !setTextureFlag)
      {
        setTextureFlag = true;
        _rectangle.Texture = _texture1;
      }

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
