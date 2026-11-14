namespace RandoMapCore;

internal static class Interop
{
    internal static bool HasBenchwarp { get; private set; } = false;
    internal static bool HasMoreStags { get; private set; } = false;

    internal static void FindInteropMods()
    {
        HasBenchwarp = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name is "Benchwarp");
        HasMoreStags = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name is "MoreStags");
    }
}
