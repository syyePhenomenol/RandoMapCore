using MapChanger;

namespace RandoMapCore.Modes;

internal static class Conditions
{
    internal static bool RandoCoreMapModEnabled()
    {
        return MapChangerMod.IsEnabled() && ModeManager.CurrentMode() is RmcMapMode;
    }

    internal static bool ItemRandoModeEnabled()
    {
        return MapChangerMod.IsEnabled() && ModeManager.CurrentMode() is ItemRandoMode;
    }

    internal static bool TransitionRandoModeEnabled()
    {
        return MapChangerMod.IsEnabled() && ModeManager.CurrentMode() is TransitionRandoMode;
    }
}
