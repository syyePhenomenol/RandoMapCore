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
        var logicChanges = JU.DeserializeFromEmbeddedResource<LogicChangeDef[]>(
            RandoMapCoreMod.Assembly,
            "RandoMapCore.Resources.Pathfinder.logicChanges.json"
        );

        foreach (var statelessWaypoint in logicChanges.SelectMany(lc => lc.StatelessWaypoints ?? []))
        {
            if (!lmb.Waypoints.Contains(statelessWaypoint))
            {
                lmb.AddWaypoint(new(statelessWaypoint, "FALSE", true));
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not add waypoint {statelessWaypoint}");
            }
        }

        foreach (var stateWaypoint in logicChanges.SelectMany(lc => lc.StateWaypoints ?? []))
        {
            if (!lmb.Waypoints.Contains(stateWaypoint.name))
            {
                lmb.AddWaypoint(stateWaypoint);
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not add waypoint {stateWaypoint.name}");
            }
        }

        foreach (var transition in logicChanges.SelectMany(lc => lc.Transitions ?? []))
        {
            if (!lmb.Waypoints.Contains(transition.name))
            {
                lmb.AddTransition(transition);
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not add transition {transition.name}");
            }
        }

        foreach (var edit in logicChanges.SelectMany(lc => lc.Edits ?? []))
        {
            if (lmb.LogicLookup.ContainsKey(edit.name))
            {
                lmb.DoLogicEdit(edit);
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not edit logic of {edit.name}");
            }
        }

        foreach (var substitution in logicChanges.SelectMany(lc => lc.Substitutions ?? []))
        {
            if (lmb.LogicLookup.ContainsKey(substitution.name))
            {
                lmb.DoSubst(substitution);
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not substitute logic of {substitution.name}");
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
