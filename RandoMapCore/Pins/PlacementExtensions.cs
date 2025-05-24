using ItemChanger;
using ItemChanger.Placements;
using ItemChanger.Tags;
using MapChanger;

namespace RandoMapCore.Pins;

internal static class PlacementExtensions
{
    internal static bool IsRandomizedPlacement(this AbstractPlacement placement)
    {
        return RandoMapCoreMod.Data.RandomizedLocations.ContainsKey(placement.Name);
    }

    internal static AbstractLocation GetAbstractLocation(this AbstractPlacement placement)
    {
        return (placement is IPrimaryLocationPlacement iplp ? iplp.Location : null)
            ?? ItemChanger.Finder.GetLocation(placement.Name);
    }

    internal static string GetSceneName(this AbstractPlacement placement)
    {
        return (
                (
                    RandoMapCoreMod.Data.RandomizedLocations.TryGetValue(placement.Name, out var ld)
                    || RandoMapCoreMod.Data.VanillaLocations.TryGetValue(placement.Name, out ld)
                )
                    ? ld?.SceneName
                    : null
            )
            ?? placement.GetAbstractLocation()?.sceneName
            ?? null;
    }

    internal static bool CanPreviewName(this TaggableObject taggable)
    {
        return !taggable.HasTag<DisableItemPreviewTag>();
    }

    internal static bool CanPreviewCost(this TaggableObject taggable)
    {
        return !taggable.HasTag<DisableCostPreviewTag>();
    }

    internal static IEnumerable<Cost> GetCosts(this AbstractPlacement placement)
    {
        List<Cost> costs = [];

        if (placement is ISingleCostPlacement iscp && iscp.Cost?.GetInnerCosts() is IEnumerable<Cost> iscpCosts)
        {
            costs.AddRange(iscpCosts);
        }

        if (
            placement.GetAbstractLocation()?.GetTag<ImplicitCostTag>()?.Cost?.GetInnerCosts()
            is IEnumerable<Cost> locationCosts
        )
        {
            costs.AddRange(locationCosts);
        }

        return costs.Any() ? costs : null;
    }

    internal static string ToText(this IEnumerable<AbstractItem> items, bool? nameOverride)
    {
        return string.Join(", ", items.Select(i => i.ToText(nameOverride)));
    }

    internal static string ToTextWithCost(this IEnumerable<AbstractItem> items, bool? nameOverride, bool? costOverride)
    {
        return string.Join(", ", items.Select(i => i.ToTextWithCosts(nameOverride, costOverride)));
    }

    internal static string ToCostText(this Cost cost, bool revealCost)
    {
        return $"{(revealCost ? cost.GetCostText() : "???")} {(cost.CanPay() ? "☑" : "☒")}";
    }

    internal static bool IsPersistent(this AbstractItem item)
    {
        return item.HasTag<PersistentItemTag>();
    }

    internal static bool AllEverObtained(this AbstractPlacement placement)
    {
        return placement.Items.All(i => i.WasEverObtained());
    }

    private static string ToText(this AbstractItem item, bool? showNameOverride)
    {
        return (showNameOverride is true || (showNameOverride is null && item.CanPreviewName()))
            ? item.GetPreviewName().LC()
            : "???";
    }

    private static string ToTextWithCosts(this AbstractItem item, bool? showNameOverride, bool? showCostOverride)
    {
        if (item.GetTag<CostTag>()?.Cost?.GetInnerCosts() is not IEnumerable<Cost> costs)
        {
            return item.ToText(showNameOverride);
        }

        var showCost = showCostOverride is true || (showCostOverride is null && item.CanPreviewCost());
        var costText = string.Join(", ", costs.Select(c => c.ToCostText(showCost)));
        return $"{item.ToText(showNameOverride)} - {costText}";
    }

    private static IEnumerable<Cost> GetInnerCosts(this Cost cost)
    {
        if (cost is MultiCost mc)
        {
            return mc;
        }

        return [cost];
    }
}
