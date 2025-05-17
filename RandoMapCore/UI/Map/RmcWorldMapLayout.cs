using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;

namespace RandoMapCore.UI;

internal class RmcWorldMapLayout : WorldMapLayout
{
    private readonly RouteSummaryText _routeSummaryText;

    public RmcWorldMapLayout()
        : base(RandoMapCoreMod.Data.ModName, nameof(RmcWorldMapLayout))
    {
        if (RandoMapCoreMod.Data.EnableRoomSelection && RandoMapCoreMod.Data.EnablePathfinder)
        {
            _routeSummaryText = new();
            _routeSummaryText.Initialize(this);
        }
    }

    public override void Update()
    {
        base.Update();
        _routeSummaryText?.Update();
    }

    public override void Destroy()
    {
        _routeSummaryText?.Destroy();
        base.Destroy();
    }

    protected override bool ActiveCondition()
    {
        return base.ActiveCondition() && ModeManager.CurrentMode() is RmcMapMode;
    }

    protected override IEnumerable<WorldMapStack> GetStacks()
    {
        return [new TopLeftPanelStack(), new ControlPanelStack(), new SelectionPanelStack()];
    }
}
