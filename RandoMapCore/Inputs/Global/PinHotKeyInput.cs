using UnityEngine;

namespace RandoMapCore.Input;

internal abstract class PinHotkeyInput : RmcGlobalHotkeyInput
{
    internal PinHotkeyInput(string name, KeyCode key)
        : base(name, "Pins", key) { }
}
