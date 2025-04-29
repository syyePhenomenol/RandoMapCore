using MapChanger;
using RandoMapCore.Pathfinder.Actions;

namespace RandoMapCore.Pathfinder;

public class RmcPathfinder : HookModule
{
    internal static RmcSearchData SD { get; private set; }
    internal static DreamgateTracker Dgt { get; private set; }
    internal static RouteManager RM { get; private set; }
    internal static SceneLogicTracker Slt { get; private set; }

    public override void OnEnterGame()
    {
        SD = new(RandoMapCoreMod.Data.PM);

        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            Dgt = new(SD);
            RM = new(SD);

            On.HutongGames.PlayMaker.Actions.SetPlayerDataString.OnEnter += Dgt.TrackDreamgateSet;
            ItemChanger.Events.OnBeginSceneTransition += Dgt.LinkDreamgateToPosition;
            ItemChanger.Events.OnBeginSceneTransition += RM.CheckRoute;
            MapChanger.Settings.OnSettingChanged += RM.ResetRoute;
        }

        Slt = new(SD);
        Events.OnWorldMap += Slt.Events_OnWorldMap;
        Events.OnQuickMap += Slt.Events_OnQuickMap;

        // Testing.LogProgressionData(SD);
        // Testing.DebugActions(SD);
        // Testing.SingleStartDestinationTest(SD);
        // Testing.SceneToSceneTest(SD, Slt);
    }

    public override void OnQuitToMenu()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            On.HutongGames.PlayMaker.Actions.SetPlayerDataString.OnEnter -= Dgt.TrackDreamgateSet;
            ItemChanger.Events.OnBeginSceneTransition -= Dgt.LinkDreamgateToPosition;
            ItemChanger.Events.OnBeginSceneTransition -= RM.CheckRoute;
            MapChanger.Settings.OnSettingChanged -= RM.ResetRoute;
        }

        Events.OnWorldMap -= Slt.Events_OnWorldMap;
        Events.OnQuickMap -= Slt.Events_OnQuickMap;

        SD = null;
        Dgt = null;
        RM = null;
        Slt = null;
    }
}
