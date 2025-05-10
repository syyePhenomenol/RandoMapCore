using MagicUI.Elements;
using RandoMapCore.Localization;
using RandoMapCore.Pathfinder;

namespace RandoMapCore.UI;

internal class PathfinderOolButton() : BorderlessExtraButton(nameof(OffRouteButton))
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.TogglePathfinderOutOfLogic();
            RmcPathfinder.RM.ResetRoute();
        }
    }

    protected override void OnHover()
    {
        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            RmcTitle.Instance.HoveredText = "Toggle disabled".L();
            return;
        }

        RmcTitle.Instance.HoveredText = "Allow sequence breaks in pathfinder search.".L();
    }

    public override void Update()
    {
        var text = $"{"Pathfinder\nOOL".L()}: ";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            Button.Content = text + "Disabled".L();
            Button.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Disabled);
            return;
        }

        this.SetButtonBoolToggle(text, RandoMapCoreMod.GS.PathfinderOutOfLogic);
    }
}
