using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace MC
{
  public class Texture
  {
    private int _handle;
    private readonly ImageResult _image;

    public Texture(string path)
    {
      StbImage.stbi_set_flip_vertically_on_load(1);
      _image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
    }

    public void Initialize()
    {
      _handle = GL.GenTexture();

      GL.ActiveTexture(TextureUnit.Texture0);
      GL.BindTexture(TextureTarget.Texture2D, _handle);

      GL.TexImage2D(
        TextureTarget.Texture2D,
        0,
        PixelInternalFormat.Rgba,
        _image.Width,
        _image.Height,
        0,
        PixelFormat.Rgba,
        PixelType.UnsignedByte,
        _image.Data
      );

      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

      // GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxAnisotropy, 0);
      GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }

    public void Use(TextureUnit unit)
    {
      GL.ActiveTexture(unit);
      GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
  }
}
