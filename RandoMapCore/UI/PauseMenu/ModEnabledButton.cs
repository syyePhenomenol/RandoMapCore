using MagicUI.Core;
using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class ModEnabledButton() : MainButton(nameof(ModEnabledButton), RandoMapCoreMod.Data.ModName, 0, 0)
{
    protected override void OnClick()
    {
        MapChanger.Settings.ToggleModEnabled();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Toggle all map mod behavior on/off.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        if (MapChanger.Settings.CurrentMode().Mod == RandoMapCoreMod.Data.ModName)
        {
            Button.Visibility = Visibility.Visible;
        }
        else
        {
            Button.Visibility = Visibility.Hidden;
        }

        if (MapChanger.Settings.MapModEnabled())
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
            Button.Content = $"{"Map Mod".L()}\n{"Enabled".L()}";
        }
        else
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            Button.Content = $"{"Map Mod".L()}\n{"Disabled".L()}";
        }
    }
}
