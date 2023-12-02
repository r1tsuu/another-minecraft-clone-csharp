using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MC
{
  struct GameObjectData
  {
    public Vector3 Position { get; set; }
    public Texture? Texture { get; set; }
  }

  class Graphics
  {
    private static readonly Shader _baseShader = new("Shaders/base.vert", "Shaders/base.frag");
    private static readonly Shape _cube = Shape.CreateCube(_baseShader);
    public Camera Camera { get; set; }

    public Graphics(Camera camera)
    {
      Camera = camera;
    }

    public static void Initialize()
    {
      _baseShader.Initialize();

      _cube.Initialize();
    }


    public void DrawCube(GameObjectData meshData)
    {
      _baseShader.Use();
      _baseShader.SetMatrix4("view", Camera.GetView());
      _baseShader.SetMatrix4("projection", Camera.GetProjection());
      _baseShader.SetBool("enableFxaa", false);
      var model = Matrix4.CreateTranslation(meshData.Position);
      _baseShader.SetMatrix4("model", model);
      meshData.Texture?.Use(TextureUnit.Texture0);
      // GL.Enable(EnableCap.CullFace);
      _cube.Draw();
      // GL.Disable(EnableCap.CullFace);
    }
  }
}
