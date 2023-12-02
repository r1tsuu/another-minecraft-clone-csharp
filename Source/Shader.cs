using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MC
{
  public class Shader
  {
    private readonly string _vertexSource;
    private readonly string _fragmentSource;
    private int _handle;

    public Shader(string vertexPath, string fragmentPath)
    {
      _vertexSource = File.ReadAllText(vertexPath);
      _fragmentSource = File.ReadAllText(fragmentPath);
    }

    public Shader Initialize()
    {
      var vertexShader = GL.CreateShader(ShaderType.VertexShader);
      GL.ShaderSource(vertexShader, _vertexSource);
      var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(fragmentShader, _fragmentSource);

      GL.CompileShader(vertexShader);
      GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vertexSuccess);
      if (vertexSuccess == 0)
      {
        var infoLog = GL.GetShaderInfoLog(vertexShader);
        Console.WriteLine(infoLog);
      }

      GL.CompileShader(fragmentShader);
      GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fragmentSuccess);
      if (vertexSuccess == 0)
      {
        var infoLog = GL.GetShaderInfoLog(vertexShader);
        Console.WriteLine(infoLog);
      }

      _handle = GL.CreateProgram();
      GL.AttachShader(_handle, vertexShader);
      GL.AttachShader(_handle, fragmentShader);
      GL.LinkProgram(_handle);

      GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int programLinkSuccess);
      if (programLinkSuccess == 0)
      {
        var infoLog = GL.GetProgramInfoLog(_handle);
        Console.WriteLine(infoLog);
      }

      GL.DetachShader(_handle, vertexShader);
      GL.DetachShader(_handle, fragmentShader);
      GL.DeleteShader(vertexShader);
      GL.DeleteShader(fragmentShader);

      return this;
    }

    public int GetUniformLocation(string name)
    {
      return GL.GetUniformLocation(_handle, name);
    }

    public int GetAttribute(string name)
    {
      return GL.GetAttribLocation(_handle, name);
    }

    public void SetInt(string name, int value)
    {
      var location = GetUniformLocation(name);
      GL.Uniform1(location, value);
    }

    public void SetVec3(string name, Vector3 value)
    {
      var location = GetUniformLocation(name);
      GL.Uniform3(location, value);
    }

    public void SetMatrix4(string name, Matrix4 value)
    {
      var location = GetUniformLocation(name);
      GL.UniformMatrix4(location, true, ref value);
    }

    public void SetBool(string name, bool value)
    {
      SetInt(name, value ? 1 : 0);
    }

    public void Use()
    {
      GL.UseProgram(_handle);
    }
  }
}


