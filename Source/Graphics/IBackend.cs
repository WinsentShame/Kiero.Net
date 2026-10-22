namespace Kiero.Net;

internal interface IBackend
{
    bool isReady { get; }
    bool Init(nint handle, int rtvFormat);
    void NewFrame();
    void Render(nint drawData);
    void Shutdown();
    bool TryGetSize(out float w, out float h);
    void OnResize();
    void OnReset();
    void OnResetPost();
}