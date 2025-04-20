using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class PinSizeButton() : MainButton(nameof(PinSizeButton), RandoMapCoreMod.Data.ModName, 1, 2)
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.TogglePinSize();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Toggle overall size of pins.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        var text = $"{"Pin Size".L()}:\n";

        switch (RandoMapCoreMod.GS.PinSize)
        {
            case PinSize.Tiny:
                text += "tiny".L();
                break;

            case PinSize.Small:
                text += "small".L();
                break;

            case PinSize.Medium:
                text += "medium".L();
                break;

            case PinSize.Large:
                text += "large".L();
                break;

            case PinSize.Huge:
                text += "huge".L();
                break;
            default:
                break;
        }

        Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        Button.Content = text;
    }
}
