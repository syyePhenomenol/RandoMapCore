using Newtonsoft.Json;

namespace RandoMapCore.Pathfinder.Actions;

internal record WaypointActionDef
{
    [JsonProperty]
    internal string Start { get; init; }

    [JsonProperty]
    internal string Destination { get; init; }

    [JsonProperty]
    internal string Text { get; init; }
}
