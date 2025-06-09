namespace RandoMapCore.Pathfinder.Actions;

internal interface IInstruction : IEquatable<IInstruction>
{
    internal string SourceText { get; }
    internal string TargetText { get; }

    internal string GetCompassObjectPath(string scene);
    internal bool IsFinished(string lastTransition);
}
