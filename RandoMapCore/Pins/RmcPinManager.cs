using System.Collections.ObjectModel;
using ItemChanger;
using MapChanger;
using MapChanger.MonoBehaviours;
using RandoMapCore.Modes;
using UnityEngine;
using SM = ConnectionMetadataInjector.SupplementalMetadata;

namespace RandoMapCore.Pins;

internal class RmcPinManager : HookModule
{
    internal const float OVERLAP_THRESHOLD = 0.15f;
    internal const float OVERLAP_THRESHOLD_SQUARED = OVERLAP_THRESHOLD * OVERLAP_THRESHOLD;

    private static Dictionary<string, RmcPin> _normalPins;
    private static Dictionary<string, PinCluster> _pinClusters;
    private static Dictionary<string, GridPin> _gridPins;

    private static Dictionary<string, List<RmcPin>> _tempPinGroups;
    private static HashSet<string> _tempPinNames;

    internal static DefaultPropertyManager Dpm { get; private set; }
    internal static PinSpriteManager Psm { get; private set; }
    internal static PinArranger PA { get; private set; }

    internal static MapObject MoPins { get; private set; }

    internal static ReadOnlyDictionary<string, RmcPin> Pins { get; private set; }

    internal static PinSelector Selector { get; private set; }

    public override void OnEnterGame()
    {
        Dpm = new();
        Psm = new();
        PA = new();

        Data.PlacementTracker.Update += OnTrackerUpdate;
        MapChanger.Events.OnWorldMap += PA.ArrangeWorldMapPinGrid;
        MapChanger.Events.OnQuickMap += PA.ArrangeQuickMapPinGrid;
    }

    public override void OnQuitToMenu()
    {
        Data.PlacementTracker.Update -= OnTrackerUpdate;
        MapChanger.Events.OnWorldMap -= PA.ArrangeWorldMapPinGrid;
        MapChanger.Events.OnQuickMap -= PA.ArrangeQuickMapPinGrid;

        Dpm = null;
        Psm = null;
        PA = null;
        MoPins = null;
        Selector = null;
        _normalPins = null;
        _pinClusters = null;
        _gridPins = null;
    }

    internal static void Make(GameObject goMap)
    {
        _normalPins = [];
        _pinClusters = [];
        _gridPins = [];

        _tempPinGroups = [];
        _tempPinNames = [];

        MoPins = Utils.MakeMonoBehaviour<MapObject>(goMap, "RandoMapCore Pins");
        MoPins.Initialize();
        MoPins.ActiveModifiers.Add(Conditions.RandoCoreMapModEnabled);

        MapObjectUpdater.Add(MoPins);

        foreach (var placement in ItemChanger.Internal.Ref.Settings.Placements.Values)
        {
            if (placement.Name is "Start")
            {
                RandoMapCoreMod.Instance.LogDebug($"Start placement detected - not including as a pin");
                continue;
            }

            if (SM.Of(placement).Get(InteropProperties.DoNotMakePin))
            {
                continue;
            }

            try
            {
                if (GetICPinDef(placement) is ICPinDef def)
                {
                    TryAddPin(def);
                }
            }
            catch (Exception e)
            {
                RandoMapCoreMod.Instance.LogError($"Failed to add pin for IC placement {placement.Name}");
                RandoMapCoreMod.Instance.LogError(e);
            }
        }

        // GeneralizedPlacements are one-to-one, but some may share the same location (e.g. shops)
        foreach (var vanillaLocation in RandoMapCoreMod.Data.VanillaLocations.Values)
        {
            if (vanillaLocation.Name == "Start")
            {
                RandoMapCoreMod.Instance.LogDebug($"Start vanilla placement detected - not including as a pin");
                continue;
            }

            try
            {
                TryAddPin(
                    new VanillaPinDef(vanillaLocation, RandoMapCoreMod.Data.PM, RandoMapCoreMod.Data.PMNoSequenceBreak)
                );
            }
            catch (Exception e)
            {
                RandoMapCoreMod.Instance.LogError($"Failed to add vanilla pin {vanillaLocation.Name}");
                RandoMapCoreMod.Instance.LogError(e);
            }
        }

        if (Interop.HasBenchwarp)
        {
            try
            {
                TryAddPin(new StartPinDef());
            }
            catch (Exception e)
            {
                RandoMapCoreMod.Instance.LogError($"Failed to add start warp pin");
                RandoMapCoreMod.Instance.LogError(e);
            }

            foreach (
                var kvp in BenchwarpInterop.BenchKeys.Where(kvp => kvp.Key is not BenchwarpInterop.BENCH_WARP_START)
            )
            {
                try
                {
                    TryAddPin(new BenchPinDef(kvp.Key, kvp.Value.SceneName));
                }
                catch (Exception e)
                {
                    RandoMapCoreMod.Instance.LogError($"Failed to add benchwarp pin {kvp.Key}");
                    RandoMapCoreMod.Instance.LogError(e);
                }
            }
        }

        foreach (var group in _tempPinGroups.Values)
        {
            if (group.Count() > 1)
            {
                _pinClusters[group.First().Name] = new PinCluster(group);
            }
            else
            {
                _normalPins[group.First().Name] = group.First();
            }
        }

        Pins = new(
            _normalPins
                .Values.Concat(_pinClusters.Values.SelectMany(pc => pc.Selectables))
                .Concat(_gridPins.Values)
                .ToDictionary(p => p.Name, p => p)
        );

        PA.InitializeGridPins(_gridPins.Values);

        OnTrackerUpdate();
        Update();

        if (RandoMapCoreMod.Data.EnablePinSelection)
        {
            Selector = Utils.MakeMonoBehaviour<PinSelector>(null, "RandoMapCore Pin Selector");
            Selector.Initialize(
                _normalPins.Values.Cast<ISelectable>().Concat(_pinClusters.Values).Concat(_gridPins.Values)
            );
        }

        _tempPinGroups = null;
        _tempPinNames = null;
    }

    private static ICPinDef GetICPinDef(AbstractPlacement placement)
    {
        if (placement.IsRandomizedPlacement() && !SM.Of(placement).Get(InteropProperties.MakeVanillaPin))
        {
            if (Interop.HasBenchwarp && BenchwarpInterop.BenchKeys.ContainsKey(placement.Name))
            {
                return new RandomizedBenchPinDef(
                    placement,
                    RandoMapCoreMod.Data.PM,
                    RandoMapCoreMod.Data.PMNoSequenceBreak
                );
            }
            else
            {
                return new RandomizedPinDef(placement, RandoMapCoreMod.Data.PM, RandoMapCoreMod.Data.PMNoSequenceBreak);
            }
        }

        return new ICVanillaPinDef(placement, RandoMapCoreMod.Data.PM, RandoMapCoreMod.Data.PMNoSequenceBreak);
    }

    private static void TryAddPin(PinDef def)
    {
        if (_tempPinNames.Contains(def.Name))
        {
            RandoMapCoreMod.Instance.LogFine($"Pin with name {def.Name} already in lookup. Skipping");
            return;
        }

        if (def.MapPosition is not null)
        {
            var normalPin = Utils.MakeMonoBehaviour<RmcPin>(MoPins.gameObject, def.Name);
            normalPin.Initialize(def);
            if (
                _tempPinGroups.Values.FirstOrDefault(list => AreOverlapping(normalPin, list))
                is List<RmcPin> overlappingPins
            )
            {
                // RandoMapCore.Instance.LogFine(
                //     $"{normalPin} overlaps with {string.Join(", ", overlappingPins.Select(p => p.Name))}"
                // );
                _tempPinGroups[overlappingPins.First().Name].Add(normalPin);
            }
            else
            {
                _tempPinGroups.Add(normalPin.Name, [normalPin]);
            }

            _ = _tempPinNames.Add(normalPin.Name);
            MoPins.AddChild(normalPin);
            return;
        }

        var gridPin = Utils.MakeMonoBehaviour<GridPin>(MoPins.gameObject, def.Name);
        gridPin.Initialize(def);
        _gridPins.Add(gridPin.Name, gridPin);
        _ = _tempPinNames.Add(gridPin.Name);
        MoPins.AddChild(gridPin);
    }

    internal static void Update()
    {
        foreach (var pin in Pins.Values)
        {
            pin.MainUpdate();
        }

        PA.UpdateZOffsets();

        foreach (var pc in _pinClusters.Values)
        {
            pc.UpdateSelectablePins();
        }

        Selector?.MainUpdate();
    }

    private static void OnTrackerUpdate()
    {
        foreach (var pin in Pins.Values)
        {
            if (pin.Def is ICPinDef icpd)
            {
                icpd.ICPlacementTracker.EnqueueUpdateItems();
            }

            if (pin.Def is ILogicPinDef ilpd)
            {
                ilpd.Logic?.Update();
                ilpd.Hint?.Update();
            }
        }
    }

    private static bool AreOverlapping(RmcPin pin, List<RmcPin> others)
    {
        var centroid = new Vector2(others.Sum(p => p.MapPosition.X), others.Sum(p => p.MapPosition.Y)) / others.Count();

        return Math.Pow(pin.MapPosition.X - centroid.x, 2) + Math.Pow(pin.MapPosition.Y - centroid.y, 2)
            < OVERLAP_THRESHOLD_SQUARED;
    }
}
