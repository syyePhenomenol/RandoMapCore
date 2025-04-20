using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class VanillaButton() : MainButton(nameof(VanillaButton), RandoMapCoreMod.Data.ModName, 0, 2)
{
    protected override void OnClick()
    {
        RandoMapCoreMod.LS.ToggleVanilla();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Toggle pins for vanilla locations on/off.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        var text = $"{"Vanilla".L()}:\n";

        if (RandoMapCoreMod.LS.VanillaOn)
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
            text += "On".L();
        }
        else
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
            text += "Off".L();
        }

        if (RandoMapCoreMod.LS.IsVanillaCustom())
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
            text += $" ({"Custom".L()})";
        }

        Button.Content = text;
    }
}
