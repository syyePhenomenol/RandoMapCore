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

    public override bool ActiveCondition()
    {
        return base.ActiveCondition() && RandoMapCoreMod.GS.RoomSelectionOn;
    }

    public override void DoAction()
    {
        if (TransitionRoomSelector.Instance?.SelectedObject is ISelectable obj)
        {
            _ = RmcPathfinder.RM.TryGetNextRouteTo(obj.Key);

            RouteText.Instance.Update();
            RouteSummaryText.Instance.Update();
            RoomSelectionPanel.Instance.Update();
        }
    }
}
