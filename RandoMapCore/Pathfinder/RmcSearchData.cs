using System.Collections.ObjectModel;
using RandoMapCore.Pathfinder.Actions;
using RandoMapCore.Settings;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using RCPathfinder;
using RCPathfinder.Actions;
using JU = RandomizerCore.Json.JsonUtil;

namespace RandoMapCore.Pathfinder;

internal class RmcSearchData : SearchData
{
    internal const string BENCHWARP = "Benchwarp";
    internal const string STARTWARP = "Start Warp";
    internal const string DREAMGATE = "Dreamgate";

    private static readonly HashSet<string> _topFallTransitions =
    [
        "Crossroads_21[top1]",
        "Fungus2_14[top1]",
        "Fungus2_30[top1]",
        "Deepnest_01b[top1]",
        "Deepnest_03[top1]",
        "Fungus2_25[top1]",
        "Deepnest_34[top1]",
        "Deepnest_35[top1]",
        "Deepnest_39[top1]",
        "Deepnest_East_02[top1]",
        "Deepnest_East_03[top1]",
        "Deepnest_East_06[top1]",
        "Deepnest_East_08[top1]",
        "Deepnest_East_11[top1]",
        "Deepnest_East_14[top2]",
        "Deepnest_East_14b[top1]",
        "Room_Colosseum_02[top1]",
        "Room_Colosseum_02[top2]",
        "Abyss_06_Core[top1]",
        "Abyss_15[top1]",
        "Abyss_17[top1]",
        "Waterways_01[top1]",
        "Waterways_02[top1]",
        "Waterways_02[top2]",
        "Waterways_02[top3]",
        "Waterways_06[top1]",
        "Waterways_07[top1]",
        "Waterways_08[top1]",
        "Waterways_15[top1]",
        "Room_GG_Shortcut[top1]",
        "Ruins1_03[top1]",
        "Ruins1_05b[top1]",
        "Ruins1_05c[top1]",
        "Ruins1_05c[top2]",
        "Ruins1_05c[top3]",
        "Ruins1_05[top1]",
        "Ruins1_09[top1]",
        "Ruins1_23[top1]",
        "Ruins2_03b[top1]",
        "Ruins2_03b[top2]",
        "Ruins2_07[top1]",
        "RestingGrounds_10[top1]",
        "RestingGrounds_10[top2]",
        "Mines_02[top1]",
        "Mines_04[top1]",
        "Mines_18[top1]",
        "Mines_25[top1]",
        "Fungus3_08[top1]",
        "Fungus3_34[top1]",
        "White_Palace_03_hub[top1]",
        "White_Palace_04[top1]",
        "White_Palace_18[top1]",
    ];

    private static readonly HashSet<string> _infectionTransitions =
    [
        "Crossroads_03[bot1]",
        "Crossroads_06[right1]",
        "Crossroads_10[left1]",
        "Crossroads_19[top1]",
    ];

    internal RmcSearchData(LogicManager localLM)
        : base(localLM)
    {
        Dictionary<string, HashSet<Term>> positionsByScene = [];
        foreach (var term in GetAllStateTerms())
        {
            if (TransitionData.TryGetScene(term.Name, out var scene))
            {
                if (positionsByScene.TryGetValue(scene, out var positions))
                {
                    _ = positions.Add(term);
                    continue;
                }

                positionsByScene[scene] = [term];
            }
        }

        StateTermsByScene = new(
            positionsByScene.ToDictionary(kvp => kvp.Key, kvp => new ReadOnlyCollection<Term>([.. kvp.Value]))
        );

        TransitionActions = new(
            StandardActionLookup
                .Values.SelectMany(list => list)
                .Where(a => a is TransitionAction)
                .ToDictionary(a => a.Source.Name, a => (TransitionAction)a)
        );

        TargetInstructionLookup = new(
            StandardActionLookup
                .Values.SelectMany(list => list)
                .Where(a => a is IInstruction)
                .ToDictionary(a => a.Target.Name, a => (IInstruction)a)
        );

        BenchwarpActions = new([.. MakeBenchwarpActions().OrderBy(a => a.Target?.Id)]);
        DreamgateAction = new();
    }

    // Only for terms with a single defined scene. Should not be using these to look up terms like
    // Can_Stag or Lower_Tram
    internal ReadOnlyDictionary<string, ReadOnlyCollection<Term>> StateTermsByScene { get; }

    internal ReadOnlyDictionary<string, TransitionAction> TransitionActions { get; }

    // Includes transition and waypoint-based instructions, where the target terms are fixed and unique.
    internal ReadOnlyDictionary<string, IInstruction> TargetInstructionLookup { get; }
    internal ReadOnlyCollection<BenchwarpAction> BenchwarpActions { get; }
    internal DreamgateAction DreamgateAction { get; }

    // Set to true to temporarily check if super sequence breaks are back in logic or not
    internal bool SuperSequenceBreakTempMode { get; set; } = false;

    protected override Dictionary<Term, List<StandardAction>> MakeStandardActions()
    {
        Dictionary<Term, List<StandardAction>> actions = [];

        var waypointActions = JU.DeserializeFromEmbeddedResource<WaypointActionDef[]>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.Pathfinder.Data.waypointActions.json"
            )
            .ToDictionary(wad => (wad.Start, wad.Destination), wad => wad.Text);

        var routeCompassOverrides = MapChanger.JsonUtil.DeserializeFromAssembly<
            Dictionary<string, Dictionary<string, string>>
        >(RandoMapCoreMod.Assembly, "RandoMapCore.Resources.Compass.routeCompassOverrides.json");

        // Logic-defined actions (both in-scene and waypoint jumps)
        foreach (var destination in GetAllStateTerms())
        {
            var logic = (DNFLogicDef)LM.GetLogicDefStrict(destination.Name);

            foreach (var start in logic.GetTerms().Where(t => t.Type is TermType.State))
            {
                if (waypointActions.TryGetValue((start.Name, destination.Name), out var text))
                {
                    _ = routeCompassOverrides.TryGetValue(start.Name, out var compassObjects);

                    AddAction(
                        start.Name is "Can_Stag"
                            ? new StagAction(start, destination, logic, text, compassObjects)
                            : new WaypointAction(start, destination, logic, text ?? start.Name, compassObjects)
                    );

                    continue;
                }

                AddAction(new InSceneAction(start, destination, logic));
            }
        }

        var vanillaInfectionTransitions = _infectionTransitions.All(TransitionData.IsVanillaTransition);

        // Transitions
        foreach ((var sourceDef, var targetDef) in TransitionData.GetPlacements())
        {
            if (
                !StateTermLookup.TryGetValue(sourceDef.Name, out var sourceTerm)
                || !StateTermLookup.TryGetValue(targetDef.Name, out var targetTerm)
            )
            {
                RandoMapCoreMod.Instance.LogWarn(
                    $"One of the terms {sourceDef.Name} and/or {targetDef.Name} does not exist in logic"
                );
                continue;
            }

            TransitionAction ta;
            if (routeCompassOverrides.TryGetValue(sourceDef.Name, out var compassObjects))
            {
                ta = new(sourceTerm, targetTerm, compassObjects.Values.First());
            }
            else
            {
                ta = new(sourceTerm, targetTerm, sourceDef.DoorName);
            }

            if (_topFallTransitions.Contains(ta.Source.Name))
            {
                ta = new TopFallTransitionAction(ta, LM.GetLogicDefStrict(ta.Source.Name));
            }

            if (vanillaInfectionTransitions && _infectionTransitions.Contains(ta.Source.Name))
            {
                ta = new InfectionTransitionAction(ta);
            }

            AddAction(ta);
        }

        return actions;

        void AddAction(StandardAction a)
        {
            if (actions.TryGetValue(a.Source, out var actionList))
            {
                actionList.Add(a);
                return;
            }

            actions[a.Source] = [a];
        }
    }

    protected override IEnumerable<AbstractAction> GetAdditionalActions(Node node)
    {
        List<AbstractAction> actions = [];

        if (node.Current is ArbitraryPosition)
        {
            if (RandoMapCoreMod.GS.PathfinderBenchwarp)
            {
                actions.AddRange(
                    BenchwarpActions.Where(ba => BenchwarpInterop.GetVisitedBenchKeys().Contains(ba.BenchKey))
                );
            }

            if (RandoMapCoreMod.GS.PathfinderDreamgate && DreamgateAction.Target is not null)
            {
                actions.Add(DreamgateAction);
            }
        }

        if (
            !SuperSequenceBreakTempMode
            && RandoMapCoreMod.GS.PathfinderSequenceBreaks is SequenceBreakSetting.SuperSequenceBreaks
            && RandoMapCoreMod.LS.SuperSequenceBreaks.TryGetValue(node.Current.Term.Name, out var oolTargets)
        )
        {
            foreach (var target in oolTargets)
            {
                if (TargetInstructionLookup.TryGetValue(target, out var oolInstruction))
                {
                    actions.Add(new SuperSequenceBreakAction(node.Current.Term, oolInstruction));
                }
            }
        }

        // foreach (var a in actions)
        // {
        //     RandoMapCoreMod.Instance.LogFine($"Adding action to node {node.Current.Term.Name}: {a.DebugString}");
        // }

        return actions;
    }

    private IEnumerable<BenchwarpAction> MakeBenchwarpActions()
    {
        List<BenchwarpAction> actions = [];

        if (Interop.HasBenchwarp)
        {
            actions.Add(new StartWarpAction(RandoMapCoreMod.Data.StartTerm));

            foreach (var kvp in BenchwarpInterop.BenchKeys)
            {
                if (StateTermLookup.TryGetValue(kvp.Key, out var benchTerm))
                {
                    actions.Add(new BenchwarpAction(benchTerm, kvp.Value));
                }
            }
        }

        return actions;
    }
}
