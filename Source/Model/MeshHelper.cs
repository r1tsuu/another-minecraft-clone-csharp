using System.Collections;

namespace MC.Model
{
  public static class MeshHelper
  {
    public static MeshData GenerateMeshDataFromQuads(List<Quad> quads)
    {
      var meshData = new MeshData();

      var indices = meshData.Indices;
      var vertices = meshData.Vertices;

      foreach (var quad in quads)
      {
        indices.Add(vertices.Count); // BottomLeft
        indices.Add(vertices.Count + 1); // BottomRight
        indices.Add(vertices.Count + 2); // TopRight

        indices.Add(vertices.Count + 2); // TopRight
        indices.Add(vertices.Count + 3); // TopLeft
        indices.Add(vertices.Count); // BottomLeft

        vertices.Add(quad.BottomLeft);
        vertices.Add(quad.BottomRight);
        vertices.Add(quad.TopRight);
        vertices.Add(quad.TopLeft);
      }

      return meshData;
    }
  }
}
