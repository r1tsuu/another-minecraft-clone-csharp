using MC.Api;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MC
{
  class Camera : IUpdateListener, IWindowResizeListener
  {
    public float Fov { get; set; } = MathHelper.DegreesToRadians(120.0f);
    public float AspectRatio { get; set; }
    public float DepthNear { get; set; } = 0.1f;
    public float DepthFar { get; set; } = 100.0f;

    public float Speed { get; set; } = 1.5f;
    public Vector3 Position { get; set; } = new(0f, 0.0f, 3.0f);
    public Vector3 Front { get; set; } = (0.0f, 0.0f, -1.0f);
    public Vector3 Up { get; set; } = (0.0f, 1.0f, 0.0f);

    public float Sensivity { get; set; } = 1.5f;
    public float Yaw { get; set; } = -45.0f;
    public float Pitch { get; set; } = 45f;

    private Vector2? _lastPosition = null;

    public Camera(float aspectRatio)
    {
      AspectRatio = aspectRatio;
    }

    public Matrix4 GetView()
    {
      return Matrix4.LookAt(Position, Position + Front, Up);
    }

    public Matrix4 GetProjection()
    {
      return Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, DepthNear, DepthFar);
    }

    public void OnUpdate(UpdatePayload payload)
    {
      var window = payload.Window;
      if (!window.IsFocused) return;

      ProcessKeyboard(window.KeyboardState, payload.DeltaTime);

      if (_lastPosition is Vector2 lastPosition)
      {
        var mouse = window.MouseState;
        var deltaX = mouse.X - lastPosition.X;
        var deltaY = mouse.Y - lastPosition.Y;
        Yaw += deltaX * Sensivity;
        var newPitch = Pitch - deltaY * Sensivity;
        Pitch = newPitch switch
        {
          > 89.0f => 89.0f,
          < -89.0f => -89.0f,
          _ => newPitch
        };

        Front = new Vector3()
        {
          X = (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Cos(MathHelper.DegreesToRadians(Yaw))),
          Y = (float)Math.Sin(MathHelper.DegreesToRadians(Pitch)),
          Z = (float)(Math.Cos(MathHelper.DegreesToRadians(Pitch)) * Math.Sin(MathHelper.DegreesToRadians(Yaw)))
        };
      }
      _lastPosition = window.MousePosition;
    }

    private void ProcessKeyboard(KeyboardState keyboard, float deltaTime)
    {
      if (keyboard.IsKeyDown(Keys.W))
      {
        Position += Front * Speed * deltaTime;
      }

      if (keyboard.IsKeyDown(Keys.S))
      {
        Position -= Front * Speed * deltaTime;
      }

      if (keyboard.IsKeyDown(Keys.A))
      {
        Position -= Vector3.Normalize(Vector3.Cross(Front, Up)) * Speed * deltaTime;
      }

      if (keyboard.IsKeyDown(Keys.D))
      {
        Position += Vector3.Normalize(Vector3.Cross(Front, Up)) * Speed * deltaTime;
      }

      if (keyboard.IsKeyDown(Keys.Space))
      {
        Position += Up * Speed * deltaTime;
      }

      if (keyboard.IsKeyDown(Keys.LeftShift))
      {
        Position -= Up * Speed * deltaTime;
      }
    }

    public void OnWindowResize(WindowResizePayload payload)
    {
      AspectRatio = payload.EventArgs.Width / payload.EventArgs.Height;
    }
  }
}
