using MapChanger;
using MapChanger.MonoBehaviours;
using RandoMapCore.Pathfinder;
using UnityEngine;

namespace RandoMapCore.Modes;

internal abstract class TransitionRandoMode : RmcMapMode
{
    public override bool DisableAreaNames => true;

    public override bool? RoomActiveOverride(RoomSprite roomSprite)
    {
        return RmcPathfinder.Slt.GetRoomActive(roomSprite.Rsd.SceneName);
    }

    public override Vector4? RoomColorOverride(RoomSprite roomSprite)
    {
        return roomSprite.Selected
            ? RmcColors.GetColor(RmcColorSetting.Room_Selected)
            : RmcPathfinder.Slt.GetRoomColor(roomSprite.Rsd.SceneName);
    }

    public override Vector4? QuickMapTitleColorOverride(QuickMapTitle qmt)
    {
        return RmcColors.GetColor(ColorSetting.UI_Neutral);
    }

    public override bool? NextAreaNameActiveOverride(NextAreaName nextAreaName)
    {
        return false;
    }

    public override bool? NextAreaArrowActiveOverride(NextAreaArrow nextAreaArrow)
    {
        return false;
    }
}

internal class TransitionNormalMode : TransitionRandoMode
{
    public override string ModeName => Settings.RmcMode.Transition_Normal.ToString().ToCleanName();
}

internal class TransitionVisitedOnlyMode : TransitionRandoMode
{
    public override string ModeName => Settings.RmcMode.Transition_Visited_Only.ToString().ToCleanName();
}

internal class TransitionAllRoomsMode : TransitionRandoMode
{
    public override string ModeName => Settings.RmcMode.Transition_All_Rooms.ToString().ToCleanName();
}
