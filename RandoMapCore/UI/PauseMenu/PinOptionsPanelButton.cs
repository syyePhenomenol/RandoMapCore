using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class PinOptionsPanelButton() : MainButton(nameof(PinOptionsPanelButton), RandoMapCoreMod.Data.ModName, 2, 0)
{
    protected override void OnClick()
    {
        PinOptionsPanel.Instance.Toggle();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "More options for pin behavior.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        if (PinOptionsPanel.Instance.Grid.Visibility == MagicUI.Core.Visibility.Visible)
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
        }
        else
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        }

        Button.Content = "More Pin\nOptions".L();
    }
}
