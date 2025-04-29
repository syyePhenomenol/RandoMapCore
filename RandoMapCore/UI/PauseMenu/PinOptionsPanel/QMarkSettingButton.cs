using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class QMarkSettingButton() : BorderlessExtraButton(nameof(QMarkSettingButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RandoMapCoreMod.GS.ToggleQMarkSetting();
            ItemCompass.Info.UpdateCurrentCompassTargets();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "Toggle question mark sprites on/off.".L();
    }

    public override void Update()
    {
        var text = $"{"Question\nmarks".L()}: ";

        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            Button.Content = text + "off".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        switch (RandoMapCoreMod.GS.QMarks)
        {
            case QMarkSetting.Off:
                text += "off".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
                break;

            case QMarkSetting.Red:
                text += "red".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;

            case QMarkSetting.Mix:
                text += "mixed".L();
                Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
                break;
            default:
                break;
        }

        Button.Content = text;
    }
}
