using MapChanger.MonoBehaviours;
using RandoMapCore.Pathfinder;
using RandoMapCore.Rooms;
using RandoMapCore.UI;

namespace RandoMapCore.Input;

internal class SelectRoomRouteInput : RmcWorldMapInput
{
    internal SelectRoomRouteInput()
        : base("Select Pathfinder Route", () => InputHandler.Instance.inputActions.menuSubmit)
    {
        Instance = this;
    }

    internal static SelectRoomRouteInput Instance { get; private set; }

    public override bool UseCondition()
    {
        return base.UseCondition() && RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder;
    }

    public override bool ActiveCondition()
    {
        return base.ActiveCondition() && RandoMapCoreMod.GS.RoomSelectionOn;
    }

    public override void DoAction()
    {
        if (RmcRoomManager.Selector?.SelectedObject is ISelectable obj)
        {
            try
            {
                RmcPathfinder.RM.TryGetNextRouteTo(obj.Key);
            }
            catch (Exception e)
            {
                RandoMapCoreMod.Instance.LogError(e);
            }
        }
    }
}
