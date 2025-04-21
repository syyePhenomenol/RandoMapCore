using InControl;
using MapChanger.Input;
using RandoMapCore.Modes;

namespace RandoMapCore.Input;

internal abstract class RmcWorldMapInput(string name, Func<PlayerAction> getPlayerAction, float holdMilliseconds = 0)
    : WorldMapInput(name, "MapMod", getPlayerAction, holdMilliseconds)
{
    public override bool UseCondition()
    {
        return RandoMapCoreMod.Data is not null;
    }

    public override bool ActiveCondition()
    {
        return base.ActiveCondition() && Conditions.RandoCoreMapModEnabled();
    }
}
