namespace RandoMapCore.Data;

public static class PlacementTracker
{
    internal static event Action Update;

    public static void OnUpdate()
    {
        Update?.Invoke();
    }
}
