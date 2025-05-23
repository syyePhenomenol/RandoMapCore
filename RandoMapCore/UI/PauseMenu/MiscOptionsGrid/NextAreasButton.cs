using MapChanger;
using MapChanger.UI;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class NextAreasButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleNextAreas();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Show Next\nAreas".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return (text + "Full".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return (
            RandoMapCoreMod.GS.ShowNextAreas switch
            {
                NextAreaSetting.Off => (text + "Off".L(), RmcColorSetting.UI_Neutral),
                NextAreaSetting.Arrows => (text + "Arrows".L(), RmcColorSetting.UI_On),
                NextAreaSetting.Full => (text + "Full".L(), RmcColorSetting.UI_On),
                _ => (text, RmcColorSetting.UI_Neutral),
            }
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableVisualCustomization
                ? "Show next area indicators (text/arrow) on the quick map.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
