namespace RandoMapCore.Input;

internal class RoomPanelInput : MapUIKeyInput
{
    internal RoomPanelInput()
        : base("Toggle Room Panel", UnityEngine.KeyCode.R)
    {
        Instance = this;
    }

    internal static RoomPanelInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableRoomSelection;
    }

    public override void DoAction()
    {
        RandoMapCoreMod.GS.ToggleRoomSelection();
        base.DoAction();
    }
}
