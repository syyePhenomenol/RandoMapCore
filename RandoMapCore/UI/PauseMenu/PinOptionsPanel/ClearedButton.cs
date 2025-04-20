using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class ClearedButton() : BorderlessExtraButton(nameof(ClearedButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.GS.ToggleShowClearedPins();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Show pins for persistent or all cleared locations.".L();
    }

    public override void Update()
    {
        var text = $"{"Show cleared".L()}:\n";

        switch (RandoMapCoreMod.GS.ShowClearedPins)
        {
            case ClearedPinsSetting.Off:
                text += "off".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
                break;

            case ClearedPinsSetting.Persistent:
                text += "persistent".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;

            case ClearedPinsSetting.All:
                text += "all".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;
            default:
                break;
        }

        Button.Content = text;
    }
}
