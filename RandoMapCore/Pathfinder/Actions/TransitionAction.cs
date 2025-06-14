using RandoMapCore.Settings;
using RandoMapCore.Transition;
using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder;
using RCPathfinder.Actions;

namespace RandoMapCore.Pathfinder.Actions;

internal class TransitionAction(Term sourceTerm, Term targetTerm, string compassObj)
    : PlacementAction(sourceTerm, targetTerm),
        IInstruction
{
    public string SourceText =>
        RandoMapCoreMod.Data.OutOfLogicVisitedTransitions.Contains(Source.Name) ? $"*{Source.Name}" : Source.Name;
    public string TargetText => Target.Name;

    internal string CompassObj { get; } = compassObj;

    public override bool TryDo(Node node, ProgressionManager pm, out StateUnion satisfiableStates)
    {
        if (IsInvalidTransition(node, pm))
        {
            satisfiableStates = default;
            return false;
        }

        return base.TryDo(node, pm, out satisfiableStates);
    }

    public override bool TryDoStateless(Node node, ProgressionManager pm)
    {
        if (IsInvalidTransition(node, pm))
        {
            return false;
        }

        return base.TryDoStateless(node, pm);
    }

    private protected virtual bool IsInvalidTransition(Node node, ProgressionManager pm)
    {
        if (TransitionData.IsVanillaTransition(node.Term.Name) || TransitionData.IsExtraTransition(node.Term.Name))
        {
            return false;
        }

        if (
            RandoMapCoreMod.GS.PathfinderSequenceBreaks is SequenceBreakSetting.Off
            && RandoMapCoreMod.Data.OutOfLogicVisitedTransitions.Contains(node.Term.Name)
        )
        {
            return true;
        }

        return !RandoMapCoreMod.Data.VisitedTransitions.ContainsKey(node.Term.Name);
    }

    public string GetCompassObjectPath(string scene)
    {
        if (!Source.Name.StartsWith(scene))
        {
            return null;
        }

        return CompassObj;
    }

    public bool IsFinished(string lastTransition)
    {
        // Fix for big mantis village transition
        return lastTransition.ToString() switch
        {
            "Fungus2_15[top2]" => Target.Name is "Fungus2_15[top3]",
            "Fungus2_14[bot1]" => Target.Name is "Fungus2_14[bot3]",
            _ => Target.Name == lastTransition.ToString(),
        };
    }

    public bool Equals(IInstruction other)
    {
        if (other is SuperSequenceBreakAction ssba)
        {
            return ReferenceEquals(this, ssba.OrigAction);
        }

        return ReferenceEquals(this, other);
    }
}

internal class TopFallTransitionAction(TransitionAction ta, LogicDef logic)
    : TransitionAction(ta.Source, ta.Target, ta.CompassObj)
{
    internal LogicDef Logic { get; } = logic;

    private protected override bool IsInvalidTransition(Node node, ProgressionManager pm)
    {
        return base.IsInvalidTransition(node, pm)
            || ((node.Depth is 0 || node.Actions.Last() is TransitionAction) && !Logic.CanGet(pm));
    }
}

internal class InfectionTransitionAction(TransitionAction ta) : TransitionAction(ta.Source, ta.Target, ta.CompassObj)
{
    private protected override bool IsInvalidTransition(Node node, ProgressionManager pm)
    {
        return base.IsInvalidTransition(node, pm) || PlayerData.instance.GetBool(nameof(PlayerData.crossroadsInfected));
    }
}
