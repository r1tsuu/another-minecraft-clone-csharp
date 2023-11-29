using MC.Api;
using OpenTK.Graphics.OpenGL4;
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
    private readonly Graphics _graphics;
    private readonly List<MeshData> _cubes = [];

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
      _graphics = new(_camera);

      for (int x = 0; x < 100; x++)
      {
        for (int z = 0; z < 100; z++)
        {
          _cubes.Add(new MeshData()
          {
            Position = (x, 0.0f, z),
            Texture = (x + z) % 2 == 0 ? _texture1 : _texture2
          });
        }
      }
    }

    private void HandleLoad()
    {
      GL.ClearColor(0f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      Graphics.Initialize();

      _texture1.Initialize();
      _texture2.Initialize();
    }

    private void HandleRender(FrameEventArgs e)
    {
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      _cubes.ForEach(cube => _graphics.DrawCube(cube));
      _window.SwapBuffers();
    }

    private void HandleUpdate(FrameEventArgs e)
    {
      var updatePayload = new UpdatePayload(_window, (float)e.Time);
      _camera.OnUpdate(updatePayload);
    }

    public Game Initialize()
    {
      _window.Load += HandleLoad;
      _window.RenderFrame += HandleRender;
      _window.UpdateFrame += HandleUpdate;
      return this;
    }

    public void Run()
    {
      _window.Run();
    }
  }
}
