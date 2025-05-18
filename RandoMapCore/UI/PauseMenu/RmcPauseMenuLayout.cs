using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

public class RmcPauseMenuLayout : PauseMenuLayout
{
    public RmcPauseMenuLayout()
        : base(RandoMapCoreMod.Data.ModName, nameof(RmcPauseMenuLayout)) { }

    protected override bool ActiveCondition()
    {
        return base.ActiveCondition() && MapChangerMod.IsEnabled() && ModeManager.CurrentMode() is RmcMapMode;
    }

    protected override PauseMenuTitle GetTitle()
    {
        return new RmcTitle();
    }

    protected override IEnumerable<MainButton> GetMainButtons()
    {
        return new List<MainButton>
        {
            new ModEnabledButton(),
            new RandomizedButton(),
            new VanillaButton(),
            new SpoilersButton(),
            new ModeButton(),
            new PinShapeButton(),
            new PinSizeButton(),
            new PoolOptionsGridButton(),
            new PinOptionsGridButton(),
            new PathfinderOptionsGridButton(),
            new MiscOptionsGridButton(),
        }.Concat(RandoMapCoreMod.Data.GetPauseMenuMainButtons());
    }

    protected override IEnumerable<ExtraButtonGrid> GetExtraButtonGrids()
    {
        return new List<ExtraButtonGrid>
        {
            new PoolOptionsGrid(),
            new PinOptionsGrid(),
            new PathfinderOptionsGrid(),
            new MiscOptionsGrid(),
        }.Concat(RandoMapCoreMod.Data.GetPauseMenuExtraButtonGrids());
    }
}
