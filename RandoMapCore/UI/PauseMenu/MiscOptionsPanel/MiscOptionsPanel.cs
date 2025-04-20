using MapChanger.UI;

namespace RandoMapCore.UI;

internal class MiscOptionsPanel : RmcOptionsPanel
{
    internal MiscOptionsPanel()
        : base(nameof(MiscOptionsPanel), GetButtons())
    {
        Instance = this;
    }

    internal static MiscOptionsPanel Instance { get; private set; }

    private static IEnumerable<ExtraButton> GetButtons()
    {
        return
        [
            new ItemCompassModeButton(),
            new QuickMapCompassesButton(),
            new AreaNamesButton(),
            new NextAreasButton(),
            new MapMarkersButton(),
            new QuillButton(),
            new DefaultItemModeButton(),
            new DefaultTransitionModeButton(),
            new DefaultSettingsButton(),
        ];
    }
}
