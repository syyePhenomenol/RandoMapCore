using UnityEngine;

namespace RandoMapCore.Input;

internal abstract class MapUIKeyInput : RmcGlobalHotkeyInput
{
    internal MapUIKeyInput(string name, KeyCode key)
        : base(name, "UI", key) { }
}
