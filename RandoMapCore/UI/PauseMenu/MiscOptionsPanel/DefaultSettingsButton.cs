using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class DefaultSettingsButton() : BorderlessExtraButton(nameof(DefaultSettingsButton))
{
    protected override void OnClick()
    {
        RandoMapCoreMod.ResetToDefaultSettings();
        MapUILayerUpdater.Update();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = $"Resets all global settings of {RandoMapCoreMod.Data.ModName}.".L();
    }

    public override void Update()
    {
        Button.Content = "Reset global\nsettings".L();

        Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Special);
    }
}
