using ItemChanger;
using MapChanger;
using MapChanger.UI;
using RandoMapCore.UI;
using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;

namespace RandoMapCore.Data;

public abstract class RmcDataModule : HookModule
{
    public abstract string ModName { get; }

    /// <summary>
    /// Used to determined if this data module should be loaded for the current save.
    /// </summary>
    public abstract bool IsCorrectSaveType { get; }

    /// <summary>
    /// Whether or not it is possible to toggle spoiler mode.
    /// If false, disables spoiler behaviour even if the "Spoiler" setting is on.
    /// Also disables the "Group By" toggle for pools.
    /// </summary>
    public virtual bool EnableSpoilerToggle { get; } = true;

    /// <summary>
    /// Whether or not it is possible to select pins in the world map (and display information).
    /// If false, disables related toggles.
    /// </summary>
    public virtual bool EnablePinSelection { get; } = true;

    /// <summary>
    /// Whether or not it is possible to select rooms in the world map (and display information).
    /// Only affects Transition Modes.
    /// If false, disables related toggles.
    /// </summary>
    public virtual bool EnableRoomSelection { get; } = true;

    /// <summary>
    /// Whether or not it is possible to reveal location hints.
    /// If false, disables related toggles.
    /// </summary>
    public virtual bool EnableLocationHints { get; } = true;

    /// <summary>
    /// Whether or not it is possible to reveal progression hints.
    /// If false, disables related toggles.
    /// </summary>
    public virtual bool EnableProgressionHints { get; } = true;

    /// <summary>
    /// Whether or not it is possible to customize the appearance of the map mod.
    /// This includes pin shape/size, colors and other visual elements on the map.
    /// If false, disables related toggles and uses default settings.
    /// </summary>
    public virtual bool EnableVisualCustomization { get; } = true;

    /// <summary>
    /// Whether or not it is possible to benchwarp from map.
    /// Needs either pin selection or room selection enabled to function.
    /// If false, disables related toggles.
    /// </summary>
    public virtual bool EnableMapBenchwarp { get; } = true;

    /// <summary>
    /// Whether or not it is possible to use the pathfinder.
    /// If false, disables related toggles.
    /// </summary>
    public virtual bool EnablePathfinder { get; } = true;

    /// <summary>
    /// Whether or not it is possible to use the item compass.
    /// If false, disables related toggles.
    /// </summary>
    public virtual bool EnableItemCompass { get; } = true;

    /// <summary>
    /// Whether or not to force the map mod to use a certain mode.
    /// If null, all modes are available as usual.
    /// </summary>
    public virtual string ForceMapMode { get; } = null;

    public abstract IReadOnlyDictionary<string, RmcTransitionDef> RandomizedTransitions { get; }
    public abstract IReadOnlyDictionary<string, RmcTransitionDef> VanillaTransitions { get; }
    public abstract IReadOnlyDictionary<RmcTransitionDef, RmcTransitionDef> RandomizedTransitionPlacements { get; }
    public abstract IReadOnlyDictionary<RmcTransitionDef, RmcTransitionDef> VanillaTransitionPlacements { get; }

    /// <summary>
    /// Is coupled transition rando (randomized transition placements go the same way back and forth).
    /// </summary>
    public abstract bool IsCoupledRando { get; }

    public abstract IReadOnlyDictionary<string, RmcLocationDef> RandomizedLocations { get; }
    public abstract IReadOnlyDictionary<string, RmcLocationDef> VanillaLocations { get; }

    /// <summary>
    /// The ProgressionManager for the save that includes sequence breaks.
    /// </summary>
    public abstract ProgressionManager PM { get; }

    /// <summary>
    /// The ProgressionManager for the save that excludes sequence breaks.
    /// </summary>
    public abstract ProgressionManager PMNoSequenceBreak { get; }

    /// <summary>
    /// A state-valued term that corresponds to the start position.
    /// </summary>
    public abstract Term StartTerm { get; }

    /// <summary>
    /// The state-valued terms that are logically equivalent to the start term.
    /// </summary>
    public abstract IReadOnlyCollection<Term> StartStateLinkedTerms { get; }

    /// <summary>
    /// The state modifier for warping to a bench.
    /// </summary>
    public abstract StateModifier WarpToBenchReset { get; }

    /// <summary>
    /// The state modifier for warping to the start position.
    /// </summary>
    public abstract StateModifier WarpToStartReset { get; }

    /// <summary>
    /// Unchecked reachable transitions, based on all the acquired progression.
    /// Is a superset of UncheckedReachableTransitionsNoSequenceBreak.
    /// </summary>
    public abstract IReadOnlyCollection<string> UncheckedReachableTransitions { get; }

    /// <summary>
    /// Unchecked reachable transitions, based on only the progression that doesn't involve logical sequence breaks.
    /// Is a subset of UncheckedReachableTransitions.
    /// </summary>
    public abstract IReadOnlyCollection<string> UncheckedReachableTransitionsNoSequenceBreak { get; }

    /// <summary>
    /// All randomized transition placements that have been checked.
    /// Is a superset of OutOfLogicVisitedTransitions.
    /// </summary>
    public abstract IReadOnlyDictionary<string, string> VisitedTransitions { get; }

    /// <summary>
    /// The randomized transition sources that have been checked, but cannot be reached without sequence breaking.
    /// Is a subset of VisitedTransitions.
    /// </summary>
    public abstract IReadOnlyCollection<string> OutOfLogicVisitedTransitions { get; }

    public abstract RandoContext Context { get; }
    public abstract IEnumerable<RandoPlacement> RandomizedPlacements { get; }
    public abstract IEnumerable<GeneralizedPlacement> VanillaPlacements { get; }

    /// <summary>
    /// Translates the input text into a target language.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public abstract string Localize(string text);

    public abstract string GetMapArea(string scene);

    /// <summary>
    /// Fetches the RandomizerCore placement from the tags of the AbstractItem.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public abstract RandoPlacement GetItemRandoPlacement(AbstractItem item);

    /// <summary>
    /// If the randomizer overrides benches from the default benches, provide the new RmcBenchKeys/bench names.
    /// Otherwise, return null.
    /// </summary>
    public abstract IReadOnlyDictionary<RmcBenchKey, string> GetCustomBenches();

    /// <summary>
    /// Add extra buttons to the main pause menu.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<RmcMainButton> GetPauseMenuMainButtons()
    {
        return [];
    }

    /// <summary>
    /// Add extra collapsable button grids to the pause menu.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<ExtraButtonGrid> GetPauseMenuExtraButtonGrids()
    {
        return [];
    }
}
