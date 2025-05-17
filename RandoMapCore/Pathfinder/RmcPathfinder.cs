using MapChanger;
using RandomizerCore.Logic;
using RCPathfinder;

namespace RandoMapCore.Pathfinder;

public class RmcPathfinder : HookModule
{
    internal static RmcLogicExtender LE { get; private set; }
    internal static RmcSearchData SD { get; private set; }
    internal static RmcProgressionSynchronizer PS { get; private set; }
    internal static RmcProgressionSynchronizer PSNoSequenceBreak { get; private set; }
    internal static StateSynchronizer StateSync { get; private set; }
    internal static RouteManager RM { get; private set; }
    internal static InstructionTracker IT { get; private set; }
    internal static SceneLogicTracker Slt { get; private set; }

    public override void OnEnterGame()
    {
        LE = new(RandoMapCoreMod.Data.PM.lm);
        SD = new(LE.LocalLM);

        PS = new(RandoMapCoreMod.Data.PM, LE);
        PSNoSequenceBreak = new(RandoMapCoreMod.Data.PMNoSequenceBreak, LE);
        StateSync = new(LE.LocalLM.StateManager);

        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RM = new();
            IT = new(SD, RM);

            ItemChanger.Events.OnBeginSceneTransition += OnBeginSceneTransition;
            ModeManager.OnModeChanged += RM.ResetRoute;
            On.HutongGames.PlayMaker.Actions.SetPlayerDataString.OnEnter += IT.TrackDreamgateSet;
            Data.PlacementTracker.Update += OnPlacementTrackerUpdate;
        }

        Slt = new();

        Events.OnWorldMap += OnWorldMap;
        Events.OnQuickMap += OnQuickMap;

        // Testing.LogProgressionData();
        // Testing.DebugActions();
        // Testing.SingleStartDestinationTest();
        // Testing.SceneToSceneTest();
    }

    public override void OnQuitToMenu()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            ItemChanger.Events.OnBeginSceneTransition -= OnBeginSceneTransition;
            ModeManager.OnModeChanged -= RM.ResetRoute;
            On.HutongGames.PlayMaker.Actions.SetPlayerDataString.OnEnter -= IT.TrackDreamgateSet;
            Data.PlacementTracker.Update -= OnPlacementTrackerUpdate;
        }

        Events.OnWorldMap -= OnWorldMap;
        Events.OnQuickMap -= OnQuickMap;

        LE = null;
        SD = null;
        PS = null;
        PSNoSequenceBreak = null;
        StateSync = null;
        RM = null;
        IT = null;
        Slt = null;
    }

    internal static ProgressionManager GetLocalPM()
    {
        return RandoMapCoreMod.GS.PathfinderOutOfLogic ? PS.LocalPM : PSNoSequenceBreak.LocalPM;
    }

    private static void OnPlacementTrackerUpdate()
    {
        PSNoSequenceBreak.Update();
        IT.UpdateSequenceBreakActions();
    }

    private static void OnBeginSceneTransition(ItemChanger.Transition transition)
    {
        Update();
        IT.TrackAction(transition);
        RM.CheckRoute(transition);
    }

    private static void OnWorldMap(GameMap _)
    {
        Update();
    }

    private static void OnQuickMap(GameMap _0, GlobalEnums.MapZone _1)
    {
        Update();
    }

    internal static void Update()
    {
        try
        {
            PS.Update();
            PSNoSequenceBreak.Update();
            StateSync.Update();
            Slt.Update();
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }
    }
}
