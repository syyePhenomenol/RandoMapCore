using UnityEngine;

namespace RandoMapCore.Input;

internal class DebugInput() : RmcGlobalHotkeyInput("Pin Debug Tool", "Misc", KeyCode.D)
{
    public override void DoAction()
    {
        Debugger.LogMapPosition();
    }
}
