namespace RandoMapCore.Modes;

internal static class Conditions
{
    internal static bool RandoCoreMapModEnabled()
    {
        return MapChanger.Settings.MapModEnabled() && MapChanger.Settings.CurrentMode() is RmcMapMode;
    }

    internal static bool ItemRandoModeEnabled()
    {
        return MapChanger.Settings.MapModEnabled() && MapChanger.Settings.CurrentMode() is ItemRandoMode;
    }

    internal static bool TransitionRandoModeEnabled()
    {
        return MapChanger.Settings.MapModEnabled() && MapChanger.Settings.CurrentMode() is TransitionRandoMode;
    }
}
