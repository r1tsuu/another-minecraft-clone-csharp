using OpenTK.Mathematics;

namespace MC.Model
{
  public struct Quad
  {
    public Vector3 BottomLeft { get; set; }
    public Vector3 BottomRight { get; set; }
    public Vector3 TopRight { get; set; }
    public Vector3 TopLeft { get; set; }
  }
}
