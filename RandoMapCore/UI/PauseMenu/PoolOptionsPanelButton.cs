using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class PoolOptionsPanelButton() : MainButton(nameof(PoolOptionsPanelButton), RandoMapCoreMod.Data.ModName, 1, 3)
{
    protected override void OnClick()
    {
        PoolOptionsPanel.Instance.Toggle();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Customize which item/location pools to display.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        if (PoolOptionsPanel.Instance.Grid.Visibility == MagicUI.Core.Visibility.Visible)
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
        }
        else
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        }

        Button.Content = "Customize\nPools".L();
    }
}
