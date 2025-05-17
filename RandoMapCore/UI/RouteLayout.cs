using GlobalEnums;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.Modes;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal class RouteLayout : MapLayout
{
    private readonly RouteText _route;

    internal RouteLayout()
        : base(RandoMapCoreMod.Data.ModName, nameof(RouteLayout))
    {
        _route = new();
        _route.Initialize(this);
        On.HeroController.UnPause += OnUnpause;
    }

    public override void Update()
    {
        _route?.Update();
    }

    public override void Destroy()
    {
        On.HeroController.UnPause -= OnUnpause;
        _route?.Destroy();
        base.Destroy();
    }

    protected override bool ActiveCondition()
    {
        return Conditions.TransitionRandoModeEnabled()
            && (
                States.WorldMapOpen
                || States.QuickMapOpen
                || (
                    !GameManager.instance.IsGamePaused()
                    && RandoMapCoreMod.GS.RouteTextInGame
                        is RouteTextInGame.NextTransition
                            or RouteTextInGame.AllTransitions
                )
            );
    }

    protected override void OnOpenWorldMap(GameMap obj)
    {
        Update();
    }

    protected override void OnOpenQuickMap(GameMap gameMap, MapZone mapZone)
    {
        Update();
    }

    protected override void OnCloseMap(GameMap obj)
    {
        Update();
    }

    private void OnUnpause(On.HeroController.orig_UnPause orig, HeroController self)
    {
        orig(self);
        Update();
    }
}
