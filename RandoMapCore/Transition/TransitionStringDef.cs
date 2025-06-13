using MagicUI.Elements;
using MapChanger;

namespace RandoMapCore.Transition;

internal readonly struct TransitionStringDef
{
    internal TransitionStringDef(string scene)
    {
        foreach (var p in RandoMapCoreMod.Data.RandomizedTransitionPlacements)
        {
            if (p.Key.SceneName == scene)
            {
                if (RandoMapCoreMod.Data.UncheckedReachableTransitions.Contains(p.Key.Name))
                {
                    RandomizedUncheckedReachable.Add(p);
                }
                else if (RandoMapCoreMod.Data.VisitedTransitions.ContainsKey(p.Key.Name))
                {
                    RandomizedVisitedOut.Add(p);
                }
                else
                {
                    RandomizedUncheckedUnreachable.Add(p);
                }
            }
            // Display only one-way in-transitions in coupled rando
            else if (
                p.Value.SceneName == scene
                && (
                    !RandoMapCoreMod.Data.IsCoupledRando
                    || !RandoMapCoreMod.Data.RandomizedTransitionPlacements.ContainsKey(p.Value)
                )
            )
            {
                if (RandoMapCoreMod.Data.VisitedTransitions.ContainsKey(p.Key.Name))
                {
                    RandomizedVisitedIn.Add(p);
                }
                else
                {
                    RandomizedUncheckedIn.Add(p);
                }
            }
        }

        foreach (var p in RandoMapCoreMod.Data.VanillaTransitionPlacements)
        {
            if (p.Key.SceneName == scene)
            {
                if (RandoMapCoreMod.Data.PM.Get(p.Key.Name) > 0)
                {
                    VanillaReachableOut.Add(p);
                }
                else
                {
                    VanillaUnreachableOut.Add(p);
                }
            }
            // Display only one-way in-transitions
            else if (
                p.Value.SceneName == scene
                && !RandoMapCoreMod.Data.VanillaTransitionPlacements.ContainsKey(p.Value)
            )
            {
                if (RandoMapCoreMod.Data.PM.Get(p.Key.Name) > 0)
                {
                    VanillaReachableIn.Add(p);
                }
                else
                {
                    VanillaUnreachableIn.Add(p);
                }
            }
        }
    }

    internal OutTransitionStringList RandomizedUncheckedReachable { get; } =
        new(
            (source, target) =>
                !RandoMapCoreMod.Data.UncheckedReachableTransitionsNoSequenceBreak.Contains(source.Name),
            RandoMapCoreMod.Data.EnableSpoilerToggle && RandoMapCoreMod.LS.SpoilerOn
        );

    internal OutTransitionStringList RandomizedUncheckedUnreachable { get; } =
        new((source, target) => false, RandoMapCoreMod.Data.EnableSpoilerToggle && RandoMapCoreMod.LS.SpoilerOn);

    internal InTransitionStringList RandomizedUncheckedIn { get; } =
        new((source, target) => false, RandoMapCoreMod.Data.EnableSpoilerToggle && RandoMapCoreMod.LS.SpoilerOn);

    internal OutTransitionStringList RandomizedVisitedOut { get; } =
        new((source, target) => RandoMapCoreMod.Data.OutOfLogicVisitedTransitions.Contains(source.Name), true);

    internal InTransitionStringList RandomizedVisitedIn { get; } =
        new((source, target) => RandoMapCoreMod.Data.OutOfLogicVisitedTransitions.Contains(source.Name), true);

    internal OutTransitionStringList VanillaReachableOut { get; } =
        new((source, target) => RandoMapCoreMod.Data.PMNoSequenceBreak.Get(source.Name) is 0, true);

    internal InTransitionStringList VanillaReachableIn { get; } =
        new((source, target) => RandoMapCoreMod.Data.PMNoSequenceBreak.Get(source.Name) is 0, true);

    internal OutTransitionStringList VanillaUnreachableOut { get; } = new((source, target) => false, true);

    internal InTransitionStringList VanillaUnreachableIn { get; } = new((source, target) => false, true);

    internal RunCollection GetFullText()
    {
        List<RunCollection> subsections = [];

        if (RandomizedUncheckedReachable.Placements.Any())
        {
            subsections.Add(
                [
                    new($"{"Unchecked Reachable".L()}:\n"),
                    .. RunCollection.Join("\n", RandomizedUncheckedReachable.GetFormattedPlacements()),
                ]
            );
        }

        if (RandomizedUncheckedUnreachable.Placements.Any())
        {
            subsections.Add(
                [
                    new($"{"Unchecked Unreachable".L()}:\n"),
                    .. RunCollection.Join("\n", RandomizedUncheckedUnreachable.GetFormattedPlacements()),
                ]
            );
        }

        if (RandomizedUncheckedIn.Placements.Any())
        {
            subsections.Add(
                [
                    new($"{"Unchecked (in)".L()}:\n"),
                    .. RunCollection.Join("\n", RandomizedUncheckedIn.GetFormattedPlacements()),
                ]
            );
        }

        if (RandomizedVisitedOut.Placements.Any() || RandomizedVisitedIn.Placements.Any())
        {
            subsections.Add(
                [
                    new($"{"Visited".L()}:\n"),
                    .. RunCollection.Join(
                        "\n",
                        RandomizedVisitedOut
                            .GetFormattedPlacements()
                            .Concat(RandomizedVisitedIn.GetFormattedPlacements())
                    ),
                ]
            );
        }

        if (VanillaReachableOut.Placements.Any() || VanillaReachableIn.Placements.Any())
        {
            subsections.Add(
                [
                    new($"{"Vanilla Reachable".L()}:\n"),
                    .. RunCollection.Join(
                        "\n",
                        VanillaReachableOut.GetFormattedPlacements().Concat(VanillaReachableIn.GetFormattedPlacements())
                    ),
                ]
            );
        }

        if (VanillaUnreachableOut.Placements.Any() || VanillaUnreachableIn.Placements.Any())
        {
            subsections.Add(
                [
                    new($"{"Vanilla Unreachable".L()}:\n"),
                    .. RunCollection.Join(
                        "\n",
                        VanillaUnreachableOut
                            .GetFormattedPlacements()
                            .Concat(VanillaUnreachableIn.GetFormattedPlacements())
                    ),
                ]
            );
        }

        return RunCollection.Join("\n\n", subsections);
    }
}
