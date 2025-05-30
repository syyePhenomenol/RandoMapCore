using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Locations;
using MapChanger.Defs;
using RandoMapCore.Pins;
using RandoMapCore.Settings;
using RandomizerCore.Logic;
using UnityEngine;

namespace RandoMapCore.UI;

internal class PlacementCompassTarget : CompassTarget
{
    private readonly LogicDef _logic;

    private bool _placementActive;
    private bool _settingActive;
    private ScaledPinSprite _sprite;
    private Color _color;

    internal PlacementCompassTarget(CompassPosition compassPosition, AbstractPlacement placement)
    {
        Position = compassPosition;

        _color = Color.white;

        Placement = placement;

        if (RandoMapCoreMod.Data.PM.lm.LogicLookup.TryGetValue(placement.Name, out var ld))
        {
            _logic = ld;
        }
    }

    internal AbstractPlacement Placement { get; }

    public override bool IsActive()
    {
        return _placementActive && _settingActive;
    }

    public override Sprite GetSprite()
    {
        return _sprite.Value;
    }

    public override Vector3 GetScale()
    {
        return _sprite.Scale;
    }

    public override Color GetColor()
    {
        return _color;
    }

    internal void Update()
    {
        Position.UpdateInfrequent();

        _sprite = RmcPinManager.Psm.GetLocationSprite(Placement);

        _placementActive = !Placement.AllEverObtained();

        _settingActive = RandoMapCoreMod.GS.ItemCompassMode switch
        {
            ItemCompassMode.Reachable => _logic.CanGet(RandoMapCoreMod.Data.PMNoSequenceBreak),
            ItemCompassMode.ReachableOutOfLogic => _logic.CanGet(RandoMapCoreMod.Data.PM),
            ItemCompassMode.All => true,
            _ => true,
        };
    }
}

internal class DualPlacementCompassPosition(
    DualLocation dl,
    CompassPosition truePosition,
    CompassPosition falsePosition
) : CompassPosition
{
    public override void UpdateEveryFrame()
    {
        truePosition.UpdateEveryFrame();
        falsePosition.UpdateEveryFrame();
    }

    public override void UpdateInfrequent()
    {
        if (dl.Test.Value)
        {
            truePosition.UpdateInfrequent();
            Value = truePosition.Value;
            return;
        }

        falsePosition.UpdateInfrequent();
        Value = falsePosition.Value;
    }
}

internal class ObjectLocationCompassPosition(ObjectLocation ol) : GameObjectCompassPosition(ol.objectName, false)
{
    public override bool TryGetGameObject(out GameObject goResult)
    {
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        var objectName = GoPath.Replace('\\', '/');

        if (!objectName.Contains('/'))
        {
            goResult = currentScene.FindGameObjectByName(objectName);
        }
        else
        {
            goResult = currentScene.FindGameObject(objectName);
        }

        return goResult != null;
    }
}
