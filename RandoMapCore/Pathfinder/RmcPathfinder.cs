using HutongGames.PlayMaker.Actions;
using MapChanger;
using RandoMapCore.Settings;
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
    internal static InstructionTracker IT { get; private set; }
    internal static SceneLogicTracker Slt { get; private set; }
    internal static RouteManager RM { get; private set; }

    public override void OnEnterGame()
    {
        LE = new(RandoMapCoreMod.Data.PM.lm);
        SD = new(LE.LocalLM);
        PS = new(LE, RandoMapCoreMod.Data.Context, true);
        PSNoSequenceBreak = new(LE, RandoMapCoreMod.Data.Context, false);
        StateSync = new(LE.LocalLM.StateManager);
        IT = new(SD);
        Slt = new(SD, PS.LocalPM, PSNoSequenceBreak.LocalPM);

        Events.OnWorldMap += OnWorldMap;
        Events.OnQuickMap += OnQuickMap;

        Data.PlacementTracker.Update += OnPlacementTrackerUpdate;
        ItemChanger.Events.OnBeginSceneTransition += OnBeginSceneTransition;

        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RM = new();
            ModeManager.OnModeChanged += RM.ResetRoute;
            On.HutongGames.PlayMaker.Actions.SetPlayerDataString.OnEnter += TrackDreamgateSet;
        }

        // Testing.LogProgressionData();
        // Testing.DebugActions();
        // Testing.SingleStartDestinationTest();
        // Testing.SceneToSceneTest();
    }

    public override void OnQuitToMenu()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            ModeManager.OnModeChanged -= RM.ResetRoute;
            On.HutongGames.PlayMaker.Actions.SetPlayerDataString.OnEnter -= TrackDreamgateSet;
        }

        Events.OnWorldMap -= OnWorldMap;
        Events.OnQuickMap -= OnQuickMap;

        Data.PlacementTracker.Update -= OnPlacementTrackerUpdate;
        ItemChanger.Events.OnBeginSceneTransition -= OnBeginSceneTransition;

        LE = null;
        SD = null;
        PS = null;
        PSNoSequenceBreak = null;
        StateSync = null;
        IT = null;
        Slt = null;
        RM = null;
    }

    internal static ProgressionManager GetLocalPM()
    {
        return
            RandoMapCoreMod.GS.PathfinderSequenceBreaks
                is SequenceBreakSetting.SequenceBreaks
                    or SequenceBreakSetting.SuperSequenceBreaks
            ? PS.LocalPM
            : PSNoSequenceBreak.LocalPM;
    }

    private static void OnWorldMap(GameMap _)
    {
        UpdateLocalProgression();
        Slt.Update();
    }

    private static void OnQuickMap(GameMap _0, GlobalEnums.MapZone _1)
    {
        UpdateLocalProgression();
        Slt.Update();
    }

    private static void OnPlacementTrackerUpdate()
    {
        PS.ReferenceUpdate();
        PSNoSequenceBreak.ReferenceUpdate();
    }

    private static void OnBeginSceneTransition(ItemChanger.Transition transition)
    {
        UpdateLocalProgression();
        IT.TrackAction(transition);
        RM?.CheckRoute(transition);
    }

    private static void UpdateLocalProgression()
    {
        try
        {
            PS.LocalUpdate();
            PSNoSequenceBreak.LocalUpdate();
            StateSync.Update();
            IT.UpdateSequenceBreakActions();
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError(e);
        }
    }

    private static void TrackDreamgateSet(
        On.HutongGames.PlayMaker.Actions.SetPlayerDataString.orig_OnEnter orig,
        SetPlayerDataString self
    )
    {
        orig(self);

        if (self.stringName.Value is "dreamGateScene")
        {
            IT.LinkDreamgate();
            RM?.ResetRoute();
        }
    }
}
