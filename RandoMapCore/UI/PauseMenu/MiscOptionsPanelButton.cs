using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class MiscOptionsPanelButton() : MainButton(nameof(MiscOptionsPanelButton), RandoMapCoreMod.Data.ModName, 2, 2)
{
    protected override void OnClick()
    {
        MiscOptionsPanel.Instance.Toggle();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Some miscenalleous options.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        if (MiscOptionsPanel.Instance.Grid.Visibility == MagicUI.Core.Visibility.Visible)
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
        }
        else
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        }

        Button.Content = "Misc.\nOptions".L();
    }
}
