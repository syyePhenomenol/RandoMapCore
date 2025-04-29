using RandoMapCore.UI;

namespace RandoMapCore.Input;

internal class LocationHintInput : RmcWorldMapInput
{
    internal LocationHintInput()
        : base("Show Location Hint", () => InputHandler.Instance.inputActions.quickCast)
    {
        Instance = this;
    }

    internal static LocationHintInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition()
            && RandoMapCoreMod.Data.EnablePinSelection
            && RandoMapCoreMod.Data.EnableLocationHints;
    }

    public override bool ActiveCondition()
    {
        return base.ActiveCondition() && RandoMapCoreMod.GS.PinSelectionOn;
    }

    public override void DoAction()
    {
        PinSelectionPanel.Instance.RevealHint();
        PinSelectionPanel.Instance.Update();
    }
}
