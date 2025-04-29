namespace RandoMapCore.Input;

internal class SelectionReticleInput : MapUIKeyInput
{
    internal SelectionReticleInput()
        : base("Toggle Selection Reticle", UnityEngine.KeyCode.S)
    {
        Instance = this;
    }

    internal static SelectionReticleInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition()
            && (RandoMapCoreMod.Data.EnablePinSelection || RandoMapCoreMod.Data.EnableRoomSelection);
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleShowReticle();
        base.DoAction();
    }
}
