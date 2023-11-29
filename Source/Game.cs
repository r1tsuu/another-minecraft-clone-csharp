using MC.Api;
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

    private readonly Camera _camera;
    private readonly GameWindow _window;
    private readonly Shader _shader;
    private readonly Texture _texture1;
    private readonly Texture _texture2;
    private readonly Shape _rectangle1;
    private readonly Shape _cube;

    private Matrix4 _model1;
    private Matrix4 _model2;

    private double _renderTime;

    public Game()
    {
      var windowSettings = new NativeWindowSettings()
      {
        Size = (_windowWidth, _windowHeight),
        Title = "Minecraft Clone"
      };


      _window = new(GameWindowSettings.Default, windowSettings)
      {
        CursorState = CursorState.Grabbed
      };
      _camera = new(_windowWidth / _windowHeight);
      _shader = new("Shaders/base.vert", "Shaders/base.frag");
      _texture1 = new("Resources/container.png");
      _texture2 = new("Resources/awesomeface.png");
      _model1 = Matrix4.CreateTranslation((1f, 0.0f, 0f));
      _model2 = Matrix4.CreateTranslation((-1f, 0.0f, 0f));
      _rectangle1 = Shape.CreateRectangle(_texture1, _shader);
      _cube = Shape.CreateCube(_texture2, _shader);
    }

    private void HandleLoad()
    {
      GL.ClearColor(0f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      _shader.Initialize();
      _texture1.Initialize();
      _texture2.Initialize();
      _rectangle1.Initialize();
      _cube.Initialize();
    }

    private void HandleRender(FrameEventArgs e)
    {
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _shader.SetMatrix4("view", _camera.GetView());
      _shader.SetMatrix4("projection", _camera.GetProjection());

      _shader.SetMatrix4("model", _model1);
      _rectangle1.Draw();
      _shader.SetMatrix4("model", _model2);
      _cube.Draw();

      _window.SwapBuffers();
    }

    private void HandleUpdate(FrameEventArgs e)
    {
      var updatePayload = new UpdatePayload(_window, (float)e.Time);
      _camera.OnUpdate(updatePayload);
      _renderTime += e.Time;
    }

    private void HandleMouseMove(MouseMoveEventArgs e)
    {
      if (_window.IsFocused)
      {
        // _window.MousePosition = (e.X + _window.Size.X / 2.0f, e.Y + _window.Size.Y / 2.0f);
      }
    }

    public Game Initialize()
    {
      _window.Load += HandleLoad;
      _window.RenderFrame += HandleRender;
      _window.UpdateFrame += HandleUpdate;
      _window.MouseMove += HandleMouseMove;
      _window.Resize += (e) =>
      _window.UpdateFrame += HandleRender;
      {

      };
      return this;
    }

    public void Run()
    {
      _window.Run();
    }
  }
}
