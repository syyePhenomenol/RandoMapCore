using Newtonsoft.Json;
using RandomizerCore.Logic;

namespace RandoMapCore.Pathfinder;

internal record LogicChangeDef
{
    [JsonProperty]
    internal string Description { get; init; }

    [JsonProperty]
    internal RawWaypointDef[] StateWaypoints { get; init; }

    [JsonProperty]
    internal string[] StatelessWaypoints { get; init; }

    [JsonProperty]
    internal RawLogicDef[] Transitions { get; init; }

    [JsonProperty]
    internal RawLogicDef[] Edits { get; init; }

    [JsonProperty]
    internal RawSubstDef[] Substitutions { get; init; }
}
