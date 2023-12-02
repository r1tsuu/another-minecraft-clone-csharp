using OpenTK.Mathematics;

namespace MC.Model
{
  public class MeshData
  {
    public List<int> Indices { get; } = [];
    public List<Vector3> Vertices { get; } = [];
  }
}
