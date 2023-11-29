using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MC.Api
{

  public struct UpdatePayload
  {
    public GameWindow Window { get; set; }
    public float DeltaTime { get; }

    public UpdatePayload(GameWindow window, float deltaTime)
    {
      Window = window;
      DeltaTime = deltaTime;
    }
  }

  interface IUpdateListener
  {
    void OnUpdate(UpdatePayload args);
  }

  public struct MouseMovePayload
  {
    public GameWindow Window { get; set; }
    public MouseMoveEventArgs EventArgs { get; }

    public MouseMovePayload(GameWindow window, MouseMoveEventArgs eventArgs)
    {
      Window = window;
      EventArgs = eventArgs;
    }
  }

  interface IMouseMoveListener
  {
    void OnMouseMove(MouseMovePayload payload);
  }

  public struct WindowResizePayload
  {
    public GameWindow Window { get; set; }
    public ResizeEventArgs EventArgs { get; }

    public WindowResizePayload(GameWindow window, ResizeEventArgs eventArgs)
    {
      Window = window;
      EventArgs = eventArgs;
    }
  }

  interface IWindowResizeListener
  {
    void OnWindowResize(WindowResizePayload payload);
  }

  public struct RenderPayload
  {
    public GameWindow Window { get; set; }
    public float DeltaTime { get; }

    public RenderPayload(GameWindow window, float deltaTime)
    {
      Window = window;
      DeltaTime = deltaTime;
    }
  }

  interface IRenderListener
  {
    void OnRender(RenderPayload payload);
  }
}
