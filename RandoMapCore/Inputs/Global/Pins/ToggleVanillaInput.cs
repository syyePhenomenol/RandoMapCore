namespace RandoMapCore.Input;

internal class ToggleVanillaInput : PinHotkeyInput
{
    internal ToggleVanillaInput()
        : base("Toggle Vanilla Pins", UnityEngine.KeyCode.Alpha3)
    {
        Instance = this;
    }

    internal static ToggleVanillaInput Instance { get; private set; }

    public override void DoAction()
    {
        RandoMapCoreMod.LS.ToggleVanilla();
        base.DoAction();
    }
}
