namespace RandoMapCore.Input;

internal class ToggleSpoilersInput : PinHotkeyInput
{
    internal ToggleSpoilersInput()
        : base("Toggle Spoilers", UnityEngine.KeyCode.Alpha1)
    {
        Instance = this;
    }

    internal static ToggleSpoilersInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableSpoilerToggle;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.LS.ToggleSpoilers();
        base.DoAction();
    }
}
