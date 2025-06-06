namespace RandoMapCore.Input;

internal class ToggleBenchwarpPinsInput : MapUIKeyInput
{
    internal ToggleBenchwarpPinsInput()
        : base("Toggle Benchwarp Pins", UnityEngine.KeyCode.W)
    {
        Instance = this;
    }

    internal static ToggleBenchwarpPinsInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && Interop.HasBenchwarp;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleBenchwarpPins();
        base.DoAction();
    }
}
