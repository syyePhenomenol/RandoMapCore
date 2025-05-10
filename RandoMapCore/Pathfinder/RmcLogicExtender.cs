using RandoMapCore.Transition;
using RandomizerCore;
using RandomizerCore.Logic;
using RCPathfinder.Logic;
using JU = RandomizerCore.Json.JsonUtil;

namespace RandoMapCore.Pathfinder;

internal class RmcLogicExtender(LogicManager referenceLM) : LogicExtender(referenceLM)
{
    protected override LogicManagerBuilder ModifyReferenceLM(LogicManagerBuilder lmb)
    {
        // Inject new terms and custom logic
        foreach (
            var rld in JU.DeserializeFromEmbeddedResource<RawLogicDef[]>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.Pathfinder.Logic.transitions.json"
            )
        )
        {
            if (!lmb.Transitions.Contains(rld.name))
            {
                lmb.AddTransition(rld);
            }
        }

        foreach (
            var rwd in JU.DeserializeFromEmbeddedResource<RawWaypointDef[]>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.Pathfinder.Logic.waypoints.json"
            )
        )
        {
            if (!lmb.Waypoints.Contains(rwd.name))
            {
                lmb.AddWaypoint(rwd);
            }
        }

        foreach (
            var rld in JU.DeserializeFromEmbeddedResource<RawLogicDef[]>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.Pathfinder.Logic.edits.json"
            )
        )
        {
            if (lmb.LogicLookup.ContainsKey(rld.name))
            {
                lmb.DoLogicEdit(rld);
            }
        }

        foreach (
            var rsd in JU.DeserializeFromEmbeddedResource<RawSubstDef[]>(
                RandoMapCoreMod.Assembly,
                "RandoMapCore.Resources.Pathfinder.Logic.substitutions.json"
            )
        )
        {
            if (lmb.LogicLookup.ContainsKey(rsd.name))
            {
                lmb.DoSubst(rsd);
            }
        }

        var startTerm = RandoMapCoreMod.Data.StartTerm;

        // Remove Start_State from existing logic
        foreach (var term in lmb.Terms)
        {
            if (lmb.LogicLookup.ContainsKey(term.Name))
            {
                lmb.DoSubst(new(term.Name, startTerm.Name, "NONE"));
            }
        }

        // Link Start_State with start terms
        foreach (var term in RandoMapCoreMod.Data.StartStateLinkedTerms)
        {
            if (lmb.LogicLookup.ContainsKey(term.Name))
            {
                lmb.DoLogicEdit(new(term.Name, $"ORIG | {startTerm.Name}"));
            }
        }

        return lmb;
    }

    protected override IEnumerable<GeneralizedPlacement> GetLocalPlacements()
    {
        foreach (var source in LocalLM.TransitionLookup.Values.Where(t => !ReferenceLM.Terms.Contains(t.term)))
        {
            if (
                TransitionData.TryGetPlacementTarget(source.Name, out var targetDef)
                && LocalLM.TransitionLookup.TryGetValue(targetDef.Name, out var target)
            )
            {
                yield return new GeneralizedPlacement(source, target);
            }
        }
    }
}
