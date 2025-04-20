using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class RandomizedButton() : MainButton(nameof(RandomizedButton), RandoMapCoreMod.Data.ModName, 0, 1)
{
    protected override void OnClick()
    {
        RandoMapCoreMod.LS.ToggleRandomized();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Toggle pins for randomized locations on/off.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        var text = $"{"Randomized".L()}:\n";

        if (RandoMapCoreMod.LS.RandomizedOn)
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
            text += "On".L();
        }
        else
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
            text += "Off".L();
        }

        if (RandoMapCoreMod.LS.IsRandomizedCustom())
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
            text += $" ({"Custom".L()})";
        }

        Button.Content = text;
    }
}
