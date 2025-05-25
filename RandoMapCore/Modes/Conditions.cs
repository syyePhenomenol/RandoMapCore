using MapChanger;

namespace RandoMapCore.Modes;

public static class Conditions
{
    public static bool RandoCoreMapModEnabled()
    {
        return MapChangerMod.IsEnabled() && ModeManager.CurrentMode() is RmcMapMode;
    }

    public static bool ItemRandoModeEnabled()
    {
        return MapChangerMod.IsEnabled() && ModeManager.CurrentMode() is ItemRandoMode;
    }

    public static bool TransitionRandoModeEnabled()
    {
        return MapChangerMod.IsEnabled() && ModeManager.CurrentMode() is TransitionRandoMode;
    }
}
