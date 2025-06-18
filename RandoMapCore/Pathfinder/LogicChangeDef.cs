using Newtonsoft.Json;

namespace RandoMapCore.Pathfinder;

internal record LogicChangeDef
{
    [JsonProperty]
    internal string Description { get; init; }

    [JsonProperty]
    internal Dictionary<string, string> StateWaypoints { get; init; }

    [JsonProperty]
    internal StatelessWaypointDef[] StatelessWaypoints { get; init; }

    [JsonProperty]
    internal Dictionary<string, string> Transitions { get; init; }

    [JsonProperty]
    internal Dictionary<string, string> Edits { get; init; }

    [JsonProperty]
    internal Dictionary<string, Dictionary<string, string>> Substitutions { get; init; }
}

internal record StatelessWaypointDef
{
    [JsonProperty]
    internal string Name { get; init; }
}

internal record PlayerDataBoolWaypointDef : StatelessWaypointDef
{
    [JsonProperty]
    internal string PlayerDataBool { get; init; }
}

internal record SceneDataBoolWaypointDef : StatelessWaypointDef
{
    [JsonProperty]
    internal string Scene { get; init; }

    [JsonProperty]
    internal string Id { get; init; }
}
