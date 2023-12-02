
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
    private readonly List<GameObjectData> _cubes = [];
    private readonly Stack<GameObjectData> _cubePool = new();

    private double _fpsTimer = 0.0;
    private int _frameCount = 0;

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
      for (var x = 0; x < 64; x++)
      {
        for (var y = 0; y < 2; y++)
        {
          for (var z = 0; z < 16; z++)
          {
            // blocks[x, y, z] = new Block(y > 3 ? BlockType.Air : BlockType.Default);
            _cubes.Add(new GameObjectData()
            {
              Position = (x, y, z),
              Texture = x > 6 ? _texture2 : _texture1
            });
          }
        }
      }

      _camera = new(_windowWidth / _windowHeight);
      _shader = new("Shaders/base.vert", "Shaders/base.frag");
      _texture1 = new("Resources/container.png");
      _texture2 = new("Resources/awesomeface.png");
      _graphics = new(_camera);
    }

    private void HandleLoad()
    {
      GL.ClearColor(0f, 0.3f, 0.3f, 1.0f);
      GL.Enable(EnableCap.DepthTest);
      GL.Enable(EnableCap.CullFace);
      GL.CullFace(CullFaceMode.Back);
      // GL.FrontFace(FrontFaceDirection.Ccw);

      Graphics.Initialize();
      // _chunkRenderer.Initialize();
      // _texture2.Initialize();
      // _texture1.Initialize();

    }

    private void HandleRender(FrameEventArgs e)
    {

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      // _chunkRenderer.Draw(_camera);
      foreach (var cube in _cubes)
      {
        _graphics.DrawCube(cube);
      };
      // for (var i = 0; i < _cubes.Count; i++)
      // {
      //   var cube = _cubes[i];
      //   // _graphics.DrawCube(cube);
      // }
      _window.SwapBuffers();

      _fpsTimer += e.Time;
      _frameCount++;

      if (_fpsTimer >= 1.0) // Update every second
      {
        double fps = _frameCount / _fpsTimer;
        _window.Title = $"Minecraft Clone - FPS: {fps:F2}";

        // Reset counters
        _fpsTimer = 0.0;
        _frameCount = 0;
      }
      GL.Flush();
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
