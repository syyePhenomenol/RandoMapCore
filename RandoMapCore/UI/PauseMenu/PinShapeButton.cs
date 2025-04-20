using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class PinShapeButton() : MainButton(nameof(PinShapeButton), RandoMapCoreMod.Data.ModName, 1, 1)
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.TogglePinShape();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Toggle the shape of the pins.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        var text = $"{"Pin Shape".L()}:\n";

        switch (RandoMapCoreMod.GS.PinShapes)
        {
            case PinShapeSetting.Mixed:
                text += "mixed".L();
                break;

            case PinShapeSetting.All_Circle:
                text += "circles".L();
                break;

            case PinShapeSetting.All_Diamond:
                text += "diamonds".L();
                break;

            case PinShapeSetting.All_Square:
                text += "squares".L();
                break;

            case PinShapeSetting.All_Pentagon:
                text += "pentagons".L();
                break;

            case PinShapeSetting.All_Hexagon:
                text += "hexagons".L();
                break;

            case PinShapeSetting.No_Border:
                text += "no borders".L();
                break;
            default:
                break;
        }

        Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        Button.Content = text;
    }
}
