using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
    private readonly Shape _cube;
    private readonly Matrix4 _projection;

    private float _speed = 1.5f;
    private Vector3 _position;
    private Vector3 _front;
    private Vector3 _up;
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
      _window = new(GameWindowSettings.Default, windowSettings);
      _shader = new("Shaders/base.vert", "Shaders/base.frag");
      _texture1 = new("Resources/container.png");
      _texture2 = new("Resources/awesomeface.png");
      _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), _windowWidth / _windowHeight, 0.1f, 100.0f);
      _model1 = Matrix4.CreateTranslation((1f, 0.0f, 0f));
      _model2 = Matrix4.CreateTranslation((-1f, 0.0f, 0f));
      _rectangle1 = Shape.CreateRectangle(_texture1, _shader);
      _cube = Shape.CreateCube(_texture2, _shader);

      _position = new(0.0f, 0.0f, 3.0f);
      _front = new(0.0f, 0.0f, -1.0f);
      _up = new(0.0f, 1.0f, 0.0f);
    }

    private void HandleWindowLoad()
    {
      GL.ClearColor(0f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      _shader.Initialize();
      _texture1.Initialize();
      _texture2.Initialize();
      _rectangle1.Initialize();
      _cube.Initialize();
    }

    private void HandleWindowRenderFrame(FrameEventArgs e)
    {
      _renderTime += e.Time;
      ProcessInput((float)e.Time);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      var view = Matrix4.LookAt(_position, _position + _front, _up);
      _shader.SetMatrix4("view", view);
      _shader.SetMatrix4("projection", _projection);

      _shader.SetMatrix4("model", _model1);
      _rectangle1.Draw();
      _shader.SetMatrix4("model", _model2);
      _cube.Draw();

      _window.SwapBuffers();
    }

    private void ProcessInput(float deltaTime)
    {
      if (!_window.IsFocused) return;
      var isKeyDown = _window.KeyboardState.IsKeyDown;

      var speed = _speed * deltaTime;

      if (isKeyDown(Keys.W))
      {
        _position += _front * speed;
      }

      if (isKeyDown(Keys.S))
      {
        _position -= _front * speed;
      }

      if (isKeyDown(Keys.D))
      {
        _position += Vector3.Normalize(Vector3.Cross(_front, _up)) * speed;
      }

      if (isKeyDown(Keys.A))
      {
        _position -= Vector3.Normalize(Vector3.Cross(_front, _up)) * speed;
      }

      if (isKeyDown(Keys.Space))
      {
        _position += _up * speed;
      }

      if (isKeyDown(Keys.LeftShift))
      {
        _position -= _up * speed;
      }
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
