using MapChanger.UI;

namespace RandoMapCore.UI;

internal class TopLeftPanelStack : WorldMapStack
{
    public override WorldMapStackAlignment Alignment => WorldMapStackAlignment.TopLeft;

    protected override IEnumerable<WorldMapPanel> GetPanels()
    {
        return RandoMapCoreMod.Data.EnableProgressionHints
            ? [new ProgressHintPanel(), new MapKeyPanel()]
            : [new MapKeyPanel()];
    }
}
