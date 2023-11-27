using OpenTK.Graphics.OpenGL4;

namespace MC
{
  class Shape
  {
    private readonly float[] _vertices;
    private readonly uint[] _indices;
    private Texture _texture;
    public Texture Texture { get => _texture; set => _texture = value; }
    private Shader _shader;
    public Shader Shader { get => _shader; set => _shader = value; }
    private int _vertexArrayObject;

    public Shape(float[] vertices, uint[] indices, Texture texture, Shader shader)
    {
      _vertices = vertices;
      _indices = indices;
      _texture = texture;
      _shader = shader;
    }

    public void Initialize()
    {
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
      GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
    }

    public void Draw()
    {
      GL.BindVertexArray(_vertexArrayObject);
      _texture.Use(TextureUnit.Texture0);
      _shader.Use();
      GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public static Shape CreateRectangle(Texture texture, Shader shader)
    {
      float[] vertices = [
        // verices          // textures
        0.5f,  0.5f, 0.0f,  1.0f, 1.0f, // top right
        0.5f, -0.5f, 0.0f,  1.0f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
      ];

      uint[] indices = [
        0, 1, 3,
        1, 2, 3
      ];

      return new Shape(vertices, indices, texture, shader);
    }
  }
}
