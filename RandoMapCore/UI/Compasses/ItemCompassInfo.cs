using ItemChanger;
using ItemChanger.Locations;
using MapChanger;
using MapChanger.Defs;
using MapChanger.MonoBehaviours;
using RandoMapCore.Modes;
using RandoMapCore.Pins;
using UnityEngine;
using SM = ConnectionMetadataInjector.SupplementalMetadata;

namespace RandoMapCore.UI;

public class ItemCompassInfo : CompassInfo
{
    private readonly Dictionary<string, List<CompassTarget>> _compassTargetLookup = [];

    public ItemCompassInfo()
    {
        foreach (var placement in ItemChanger.Internal.Ref.Settings.Placements.Values)
        {
            var hasInteropCompassLocations = false;
            if (
                SM.Of(placement).Get(InteropProperties.GameObjectCompassLocations)
                is Dictionary<string, (string, bool)[]> gameObjLocations
            )
            {
                hasInteropCompassLocations = true;
                foreach (var kvp in gameObjLocations)
                {
                    Add(
                        kvp.Key,
                        kvp.Value.Select(goInfo => new PlacementCompassTarget(
                            new GameObjectCompassPosition(goInfo.Item1, goInfo.Item2),
                            placement
                        ))
                    );
                }
            }

            if (
                SM.Of(placement).Get(InteropProperties.FixedCompassLocations)
                is Dictionary<string, (float x, float y)[]> fixedLocations
            )
            {
                hasInteropCompassLocations = true;
                foreach (var kvp in fixedLocations)
                {
                    Add(
                        kvp.Key,
                        kvp.Value.Select(coords => new PlacementCompassTarget(
                            new FixedCompassPosition(coords),
                            placement
                        ))
                    );
                }
            }

            if (
                !hasInteropCompassLocations
                && placement.GetSceneName() is string scene
                && TryGetDefaultCompassTarget(placement, scene, out var target)
            )
            {
                Add(scene, [target]);
            }
        }
    }

    public override string Name => "Item Compass";
    public override float Radius => 1.5f;
    public override float Scale => 1.8f;
    public override bool RotateSprite => false;
    public override bool Lerp => true;
    public override float LerpDuration => 0.5f;

    public override bool ActiveCondition()
    {
        return Conditions.RandoCoreMapModEnabled() && RandoMapCoreMod.GS.ItemCompassOn;
    }

    public override Vector2 GetCompassCenter()
    {
        return (Vector2)HeroController.instance?.gameObject?.transform?.position;
    }

    internal void UpdateCompassTargets()
    {
        CompassTargets.Clear();

        if (_compassTargetLookup.TryGetValue(Utils.CurrentScene(), out var targets))
        {
            RandoMapCoreMod.Instance.LogFine(
                $"New compass targets: {string.Join(", ", targets.Select(t => ((PlacementCompassTarget)t).Placement.Name))}"
            );
            CompassTargets.AddRange(targets);
            UpdateCurrentCompassTargets();
        }
    }

    internal void UpdateCurrentCompassTargets()
    {
        foreach (var target in CompassTargets)
        {
            ((PlacementCompassTarget)target).Update();
        }
    }

    private void Add(string scene, IEnumerable<CompassTarget> targets)
    {
        if (_compassTargetLookup.TryGetValue(scene, out var list))
        {
            list.AddRange(targets);
        }
        else
        {
            _compassTargetLookup[scene] = [.. targets];
        }
    }

    private bool TryGetDefaultCompassTarget(AbstractPlacement placement, string sceneName, out CompassTarget target)
    {
        CompassPosition position = null;

        if (MapChanger.Finder.TryGetLocation(placement.Name, out var mld) && mld.WorldPosition is WorldPositionDef wpd)
        {
            position = new FixedCompassPosition(wpd);
            // RandoMapCore.Instance.LogFine($"Got {position.GetType().Name} for {placement.Name} from internal coordinates");
        }
        else if (
            placement.GetAbstractLocation() is AbstractLocation al
            && TryGetAbstractLocationCompassPosition(al, out position)
        )
        {
            // RandoMapCore.Instance.LogFine($"Got {position.GetType().Name} for {placement.Name} from {al.GetType().Name}");
        }
        else if (
            SM.Of(placement).Get(InteropProperties.WorldMapLocation) is (string, float, float) worldMapLocation
            && worldMapLocation.scene == sceneName
        )
        {
            position = new FixedCompassPosition((worldMapLocation.x, worldMapLocation.y));
            // RandoMapCore.Instance.LogFine($"Got {position.GetType().Name} for {placement.Name} from WorldMapLocations");
        }
        else if (
            SM.Of(placement).Get(InteropProperties.WorldMapLocations)
                is (string scene, float x, float y)[] worldMapLocations
            && worldMapLocations.FirstOrDefault(wml => wml.scene == sceneName)
                is
                (string, float, float) obsoleteWorldMapLocation
            && obsoleteWorldMapLocation.scene == sceneName
        )
        {
            position = new FixedCompassPosition((obsoleteWorldMapLocation.x, obsoleteWorldMapLocation.y));
        }
        else
        {
            RandoMapCoreMod.Instance.LogFine($"No default compass position for {placement.Name} found");
        }

        if (position is not null)
        {
            target = new PlacementCompassTarget(position, placement);
            return true;
        }

        target = null;
        return false;
    }

    private static bool TryGetAbstractLocationCompassPosition(AbstractLocation al, out CompassPosition compassPosition)
    {
        string objectName = null;

        switch (al)
        {
            case DualLocation dl:
                if (
                    TryGetAbstractLocationCompassPosition(dl.trueLocation, out var truePosition)
                    && TryGetAbstractLocationCompassPosition(dl.falseLocation, out var falsePosition)
                )
                {
                    compassPosition = new DualPlacementCompassPosition(dl, truePosition, falsePosition);
                    return true;
                }

                break;
            case CoordinateLocation cl:
                compassPosition = new FixedCompassPosition((cl.x, cl.y));
                return true;
            case EnemyFsmLocation efl:
                objectName = efl.enemyObj;
                break;
            case EnemyLocation el:
                objectName = el.objectName;
                break;
            case ExistingFsmContainerLocation efcl:
                if (efcl.replacePath is not null)
                {
                    objectName = efcl.replacePath;
                }
                else
                {
                    objectName = efcl.objectName;
                }

                break;
            case ObjectLocation ol:
                compassPosition = new ObjectLocationCompassPosition(ol);
                return true;
            case ShopLocation sl:
                objectName = sl.objectName;
                break;
            default:
                break;
        }

        if (objectName is not null)
        {
            compassPosition = new GameObjectCompassPosition(objectName, false);
            return true;
        }

        compassPosition = null;
        return false;
    }
}
