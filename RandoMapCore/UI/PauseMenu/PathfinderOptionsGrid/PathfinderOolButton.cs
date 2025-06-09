using MapChanger;
using MapChanger.UI;
using RandoMapCore.Pathfinder;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class PathfinderOolButton : ExtraButton
{
    protected override void OnClick()
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            RandoMapCoreMod.GS.TogglePathfinderSequenceBreaks();
            RmcPathfinder.RM.ResetRoute();
        }
    }

    protected override TextFormat GetTextFormat()
    {
        var text = $"{"Pathfinder\nOOL".L()}: ";

        if (!RandoMapCoreMod.Data.EnableRoomSelection || !RandoMapCoreMod.Data.EnablePathfinder)
        {
            return (text + "Disabled".L(), RmcColorSetting.UI_Disabled).ToTextFormat();
        }

        return (
            RandoMapCoreMod.GS.PathfinderSequenceBreaks switch
            {
                SequenceBreakSetting.Off => (text + "Off".L(), RmcColorSetting.UI_Neutral),
                SequenceBreakSetting.SequenceBreaks => (text + "On".L(), RmcColorSetting.UI_On),
                SequenceBreakSetting.SuperSequenceBreaks => (text + "Extra".L(), RmcColorSetting.UI_On),
                _ => (text, RmcColorSetting.UI_Neutral),
            }
        ).ToTextFormat();
    }

    protected override TextFormat? GetHoverTextFormat()
    {
        return (
            RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder
                ? "Allow sequence breaks in pathfinder search.".L()
                : "Toggle disabled".L()
        ).ToNeutralTextFormat();
    }
}
