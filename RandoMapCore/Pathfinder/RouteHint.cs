using Newtonsoft.Json;

namespace RandoMapCore.Pathfinder;

internal record RouteHint
{
    [JsonConstructor]
    internal RouteHint(string[][] termPairs, string[] pdBools, string text)
    {
        TermPairs = [.. termPairs.Select(p => (p[0], p[1]))];
        PDBools = pdBools;
        Text = text;
    }

    internal (string, string)[] TermPairs { get; }

    internal string[] PDBools { get; }

    internal string Text { get; }

    internal bool IsActive()
    {
        return !PDBools.All(PlayerData.instance.GetBool);
    }
}
