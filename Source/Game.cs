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
    private readonly Matrix4 _projection;
    private readonly Matrix4 _model;
    private readonly Matrix4 _view;

    private int _vertexArrayObject;
    private readonly float[] _vertices = [
        // verices          // textures
      0.5f,  0.5f, 0.0f,  1.0f, 1.0f, // top right
      0.5f, -0.5f, 0.0f,  1.0f, 0.0f, // bottom right
      -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
      -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
    ];
    private readonly uint[] _indices = [
      0, 1, 3,
      1, 2, 3
    ];

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
      _model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
      _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
    }

    private void HandleWindowLoad()
    {
      _shader.Initialize();
      _shader.Use();
      GL.ClearColor(0f, 0.3f, 0.3f, 1.0f);

      var vertexBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
      GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

      _vertexArrayObject = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject);

      var positionAttribute = _shader.GetAttribute("aPosition");
      GL.EnableVertexAttribArray(positionAttribute);
      GL.VertexAttribPointer(positionAttribute, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

      var texCoordAttribute = _shader.GetAttribute("aTexCoord");
      GL.EnableVertexAttribArray(texCoordAttribute);
      GL.VertexAttribPointer(texCoordAttribute, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

      var elementBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
      GL.BufferData(
        BufferTarget.ElementArrayBuffer,
        _indices.Length * sizeof(uint),
        _indices,
        BufferUsageHint.StaticDraw
      );

      _texture0.Initialize();
      _texture0.Use(TextureUnit.Texture0);
      _texture1.Initialize();
      _texture1.Use(TextureUnit.Texture1);

      _shader.SetInt("texture0", 0);
      _shader.SetInt("texture1", 1);
    }

    private void HandleWindowRenderFrame(FrameEventArgs e)
    {
      GL.Clear(ClearBufferMask.ColorBufferBit);
      GL.BindVertexArray(_vertexArrayObject);
      _texture0.Use(TextureUnit.Texture0);
      _texture1.Use(TextureUnit.Texture2);
      _shader.Use();

      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
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
