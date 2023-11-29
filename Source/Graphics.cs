using OpenTK.Mathematics;

namespace MC
{
  struct MeshData
  {
    public Vector3 Position { get; set; }
    public Texture? Texture { get; set; }
  }

  class Graphics
  {
    private static readonly Shader _baseShader = new("Shaders/base.vert", "Shaders/base.frag");

    public static void Initialize()
    {
      _baseShader.Initialize();
    }

    public static void DrawCube(MeshData mesh)
    {

    }
  }
}
