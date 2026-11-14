using ItemChanger;
using MapChanger;

namespace RandoMapCore;

internal class MoreStagsInterop: HookModule
{
    internal static List<string> StagNames { get; private set; }
        
    public override void OnEnterGame()
    {
        MoreStags.LocalData ld = MoreStags.MoreStags.localData;
        StagNames = ld.enabled ? [.. ld.activeStags.Select(data => FormatStagName(data.name))] : [];
    }

    public override void OnQuitToMenu()
    {
        StagNames = null;
    }

    internal static string FormatStagName(string name)
    {
        return name switch {
            "Dirtmouth" => LocationNames.Dirtmouth_Stag,
            "Crossroads" => LocationNames.Crossroads_Stag,
            "Greenpath" => LocationNames.Greenpath_Stag,
            "Fungal Wastes" => LocationNames.Queens_Station_Stag,
            "Royal Gardens" => LocationNames.Queens_Gardens_Stag,
            "City 1" => LocationNames.City_Storerooms_Stag,
            "Kings Station" => LocationNames.Kings_Station_Stag,
            "Resting Grounds" => LocationNames.Resting_Grounds_Stag,
            "Deepnest" => LocationNames.Distant_Village_Stag,
            "Hidden Station" => LocationNames.Hidden_Station_Stag,
            "Stag Nest" => LocationNames.Stag_Nest_Stag,
            _ => $"Stag-{name.Replace(" ", "_")}"
        };
    }
}
