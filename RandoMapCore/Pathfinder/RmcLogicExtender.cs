using System.Collections.ObjectModel;
using RandoMapCore.Transition;
using RandomizerCore;
using RandomizerCore.Logic;
using RCPathfinder.Logic;
using JU = RandomizerCore.Json.JsonUtil;

namespace RandoMapCore.Pathfinder;

internal class RmcLogicExtender(LogicManager referenceLM) : LogicExtender(referenceLM)
{
    internal ReadOnlyDictionary<string, string> PdBoolWaypoints { get; private protected set; }

    internal ReadOnlyDictionary<(string, string), string> SceneBoolWaypoints { get; private protected set; }

    protected override LogicManagerBuilder ModifyReferenceLM(LogicManagerBuilder lmb)
    {
        Dictionary<string, string> pdBoolWaypoints = [];
        Dictionary<(string, string), string> sceneBoolWaypoints = [];

        // Inject new terms and custom logic
        var logicChanges = JU.DeserializeFromEmbeddedResource<LogicChangeDef[]>(
            RandoMapCoreMod.Assembly,
            "RandoMapCore.Resources.Pathfinder.logicChanges.json"
        );

        foreach (var statelessWaypoint in logicChanges.SelectMany(lc => lc.StatelessWaypoints ?? []))
        {
            if (!lmb.Waypoints.Contains(statelessWaypoint.Name))
            {
                lmb.AddWaypoint(new(statelessWaypoint.Name, "FALSE", true));

                if (statelessWaypoint is PlayerDataBoolWaypointDef pdbwd)
                {
                    pdBoolWaypoints[pdbwd.Name] = pdbwd.PlayerDataBool;
                }
                else if (statelessWaypoint is SceneDataBoolWaypointDef sdbwd)
                {
                    sceneBoolWaypoints[(sdbwd.Scene, sdbwd.Id)] = sdbwd.Name;
                }
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not add waypoint {statelessWaypoint}");
            }
        }

        PdBoolWaypoints = new(pdBoolWaypoints);
        SceneBoolWaypoints = new(sceneBoolWaypoints);

        foreach (var stateWaypoint in logicChanges.SelectMany(lc => lc.StateWaypoints ?? []))
        {
            if (!lmb.Waypoints.Contains(stateWaypoint.Key))
            {
                lmb.AddWaypoint(new(stateWaypoint.Key, stateWaypoint.Value));
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not add waypoint {stateWaypoint.Key}");
            }
        }

        foreach (var transition in logicChanges.SelectMany(lc => lc.Transitions ?? []))
        {
            if (!lmb.Waypoints.Contains(transition.Key))
            {
                lmb.AddTransition(new(transition.Key, transition.Value));
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not add transition {transition.Key}");
            }
        }

        foreach (var edit in logicChanges.SelectMany(lc => lc.Edits ?? []))
        {
            if (lmb.LogicLookup.ContainsKey(edit.Key))
            {
                lmb.DoLogicEdit(new(edit.Key, edit.Value));
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not edit logic of {edit.Key}");
            }
        }

        foreach (var substitutionGroup in logicChanges.SelectMany(lc => lc.Substitutions ?? []))
        {
            if (lmb.LogicLookup.ContainsKey(substitutionGroup.Key))
            {
                foreach (
                    var substitution in substitutionGroup.Value.Select(s => new RawSubstDef(
                        substitutionGroup.Key,
                        s.Key,
                        s.Value
                    ))
                )
                {
                    lmb.DoSubst(substitution);
                }
            }
            else
            {
                RandoMapCoreMod.Instance.LogFine($"Could not substitute logic of {substitutionGroup.Key}");
            }
        }

        var startTerm = RandoMapCoreMod.Data.StartTerm;

        // Remove start term from existing logic
        lmb.DoLogicEdit(new(startTerm.Name, "NONE"));

        foreach (var term in lmb.Terms)
        {
            if (lmb.LogicLookup.ContainsKey(term.Name))
            {
                lmb.DoSubst(new(term.Name, startTerm.Name, "NONE"));
            }
        }

        // Link start term to logical descendents
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
