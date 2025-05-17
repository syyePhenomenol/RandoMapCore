using GlobalEnums;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class RmcBottomRowLayout : MapLayout
{
    private readonly RmcBottomRowTextGrid _grid;

    internal RmcBottomRowLayout()
        : base(RandoMapCoreMod.Data.ModName, nameof(RmcBottomRowLayout))
    {
        _grid = new();
        _grid.Initialize(this);
    }

    public override void Update()
    {
        _grid?.Update();
    }

    public override void Destroy()
    {
        _grid?.Destroy();
        base.Destroy();
    }

    protected override bool ActiveCondition()
    {
        return MapChangerMod.IsEnabled()
            && ModeManager.CurrentMode() is RmcMapMode
            && (States.WorldMapOpen || States.QuickMapOpen);
    }

    protected override void OnOpenWorldMap(GameMap obj)
    {
        _grid?.Update();
    }

    protected override void OnOpenQuickMap(GameMap gameMap, MapZone mapZone)
    {
        _grid?.Update();
    }

    protected override void OnCloseMap(GameMap obj) { }
}
