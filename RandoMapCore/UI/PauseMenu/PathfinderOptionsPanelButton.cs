using MagicUI.Elements;
using MapChanger.UI;
using RandoMapCore.Localization;

namespace RandoMapCore.UI;

internal class PathfinderOptionsPanelButton()
    : MainButton(nameof(PathfinderOptionsPanelButton), RandoMapCoreMod.Data.ModName, 2, 1)
{
    protected override void OnClick()
    {
        PathfinderOptionsPanel.Instance.Toggle();
    }

    protected override void OnHover()
    {
        RmcTitle.Instance.HoveredText = "Pathfinder options.".L();
    }

    protected override void OnUnhover()
    {
        RmcTitle.Instance.HoveredText = null;
    }

    public override void Update()
    {
        base.Update();

        Button.BorderColor = RmcColors.GetColor(RmcColorSetting.UI_Borders);

        if (PathfinderOptionsPanel.Instance.Grid.Visibility == MagicUI.Core.Visibility.Visible)
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
        }
        else
        {
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        }

        Button.Content = "Pathfinder\nOptions".L();
    }
}
