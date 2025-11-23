using MapChanger;
using MapChanger.MonoBehaviours;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;
using UnityEngine;

namespace RandoMapCore.UI;

public class RouteCompassInfo : CompassInfo
{
    public override string Name => "Route Compass";
    public override float Radius => 1.5f;
    public override float Scale => 2.0f;
    public override bool RotateSprite => true;
    public override bool Lerp => false;
    public override float LerpDuration => 0.5f;

    public override bool ActiveCondition()
    {
        return Conditions.TransitionRandoModeEnabled() && RandoMapCoreMod.GS.ShowRouteCompass;
    }

    public override Vector2 GetCompassCenter()
    {
        return (Vector2)HeroController.instance?.gameObject?.transform?.position;
    }

    internal void UpdateCompassTarget()
    {
        CompassTargets.Clear();
    
        if (
            RmcPathfinder.RM?.CurrentRoute is Route route
            && route.CurrentInstruction?.GetCompassObjectPath(Utils.CurrentScene()) is string goPath
        )
        {
            CompassTargets.Add(new TransitionCompassTarget(goPath, RmcColors.GetColor(RmcColorSetting.UI_Compass)));
        }
    }
}
