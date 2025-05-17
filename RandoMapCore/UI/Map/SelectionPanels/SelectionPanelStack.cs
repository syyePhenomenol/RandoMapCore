using MapChanger.UI;

namespace RandoMapCore.UI;

internal class SelectionPanelStack : WorldMapStack
{
    public override WorldMapStackAlignment Alignment => WorldMapStackAlignment.TopRight;

    protected override IEnumerable<WorldMapPanel> GetPanels()
    {
        List<WorldMapPanel> panels = [];

        if (RandoMapCoreMod.Data.EnablePinSelection)
        {
            panels.Add(new PinSelectionPanel());
        }

        if (RandoMapCoreMod.Data.EnableRoomSelection)
        {
            panels.Add(new RoomSelectionPanel());
        }

        return panels;
    }
}
