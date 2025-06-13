using System.Collections.ObjectModel;
using MagicUI.Elements;
using MapChanger;
using RandoMapCore.Data;
using RandoMapCore.Pathfinder;

namespace RandoMapCore.Transition;

internal abstract class TransitionStringList(Func<RmcTransitionDef, RmcTransitionDef, bool> requiresSequenceBreak)
{
    private readonly Dictionary<RmcTransitionDef, RmcTransitionDef> _placements = [];

    internal ReadOnlyDictionary<RmcTransitionDef, RmcTransitionDef> Placements => new(_placements);

    internal void Add(KeyValuePair<RmcTransitionDef, RmcTransitionDef> kvp)
    {
        _placements.Add(kvp.Key, kvp.Value);
    }

    internal IEnumerable<RunCollection> GetFormattedPlacements()
    {
        return [.. Placements.Select(kvp => GetFormattedPlacement(kvp.Key, kvp.Value))];
    }

    internal RunCollection GetFormattedPlacement(RmcTransitionDef source, RmcTransitionDef target)
    {
        return
        [
            .. (requiresSequenceBreak(source, target) ? new RunCollection([new("*")]) : []),
            .. (GetPlacementLine(source, target)),
        ];
    }

    private protected abstract RunCollection GetPlacementLine(RmcTransitionDef source, RmcTransitionDef target);

    private protected RunCollection GetInPlacementLine(RmcTransitionDef source, RmcTransitionDef target)
    {
        return
        [
            new($"{source.SceneName.LC()}[{source.DoorName.LC()}]")
            {
                Color = RmcPathfinder.Slt.GetRoomColor(source.SceneName) with { w = 1f },
            },
            new(" -> "),
            new(target.DoorName.LC())
            {
                Color = RmcPathfinder.Slt.GetRoomColor(target.SceneName) with { w = 1f },
                Bold = true,
            },
        ];
    }
}

internal class OutTransitionStringList(
    Func<RmcTransitionDef, RmcTransitionDef, bool> requiresSequenceBreak,
    bool revealTargets
) : TransitionStringList(requiresSequenceBreak)
{
    private protected override RunCollection GetPlacementLine(RmcTransitionDef source, RmcTransitionDef target)
    {
        return
        [
            new(source.DoorName.LC())
            {
                Color = RmcPathfinder.Slt.GetRoomColor(source.SceneName) with { w = 1f },
                Bold = true,
            },
            new(" -> "),
            revealTargets
                ? new($"{target.SceneName.LC()}[{target.DoorName.LC()}]")
                {
                    Color = RmcPathfinder.Slt.GetRoomColor(target.SceneName) with { w = 1f },
                }
                : new("???"),
        ];
    }
}

internal class InTransitionStringList(
    Func<RmcTransitionDef, RmcTransitionDef, bool> requiresSequenceBreak,
    bool revealSources
) : TransitionStringList(requiresSequenceBreak)
{
    private protected override RunCollection GetPlacementLine(RmcTransitionDef source, RmcTransitionDef target)
    {
        return
        [
            revealSources
                ? new($"{source.SceneName.LC()}[{source.DoorName.LC()}]")
                {
                    Color = RmcPathfinder.Slt.GetRoomColor(source.SceneName) with { w = 1f },
                }
                : new("???"),
            new(" -> "),
            new(target.DoorName.LC())
            {
                Color = RmcPathfinder.Slt.GetRoomColor(target.SceneName) with { w = 1f },
                Bold = true,
            },
        ];
    }
}
