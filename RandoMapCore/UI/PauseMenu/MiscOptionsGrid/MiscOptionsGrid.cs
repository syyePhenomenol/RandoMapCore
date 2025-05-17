using MapChanger.UI;

namespace RandoMapCore.UI;

internal class MiscOptionsGrid : RmcOptionsGrid
{
    public override int RowSize => 5;

    protected override IEnumerable<ExtraButton> GetButtons()
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
            new UIScaleButton(),
            new ResetGlobalSettingsButton(),
        ];
    }
}
