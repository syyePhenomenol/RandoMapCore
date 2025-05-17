using MapChanger.UI;

namespace RandoMapCore.UI;

internal class ControlPanelStack : WorldMapStack
{
    public override WorldMapStackAlignment Alignment => WorldMapStackAlignment.BottomLeft;

    protected override IEnumerable<WorldMapPanel> GetPanels()
    {
        return [new ControlPanel()];
    }
}
