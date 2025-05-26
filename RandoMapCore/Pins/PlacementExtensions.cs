using ItemChanger;
using ItemChanger.Placements;
using ItemChanger.Tags;
using MagicUI.Elements;
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

    internal static RunCollection ToText(this IEnumerable<AbstractItem> items, bool? nameOverride)
    {
        return RunCollection.Join("  -  ", items.Select(i => i.ToText(nameOverride)));
    }

    internal static RunCollection ToTextWithCosts(
        this IEnumerable<AbstractItem> items,
        bool? nameOverride,
        bool? costOverride,
        bool canPayPlacement
    )
    {
        return RunCollection.Join(
            "  -  ",
            items.Select(i => i.ToTextWithCosts(nameOverride, costOverride, canPayPlacement))
        );
    }

    internal static RunCollection ToCostText(this Cost cost, bool revealCost)
    {
        var costText = cost.GetCostText()
            ?.Replace("Pay ", "")
            ?.Replace("Once you own ", "")
            ?.Replace(", I'll gladly sell it to you.", "")
            ?.Replace("Requires ", "");

        var canPay = cost.CanPay();

        return
        [
            new Run(revealCost ? costText : "???"),
            new Run(" "),
            new Run(cost.CanPay() ? "☑" : "☒") { Color = canPay ? RmcColors.GetColor(RmcColorSetting.UI_On) : null },
        ];
    }

    internal static bool IsPersistent(this AbstractItem item)
    {
        return item.HasTag<PersistentItemTag>();
    }

    internal static bool AllEverObtained(this AbstractPlacement placement)
    {
        return placement.Items.All(i => i.WasEverObtained());
    }

    private static Run ToText(this AbstractItem item, bool? showNameOverride)
    {
        var text =
            (showNameOverride is true || (showNameOverride is null && item.CanPreviewName()))
                ? item.GetPreviewName().LC()
                : "???";
        return new Run(text);
    }

    private static RunCollection ToTextWithCosts(
        this AbstractItem item,
        bool? showNameOverride,
        bool? showCostOverride,
        bool canPayPlacement
    )
    {
        var itemText = item.ToText(showNameOverride) with { Bold = true };

        if (item.GetTag<CostTag>()?.Cost?.GetInnerCosts() is not IEnumerable<Cost> costs)
        {
            return [itemText with { Color = canPayPlacement ? RmcColors.GetColor(RmcColorSetting.UI_On) : null }];
        }

        var showCost = showCostOverride is true || (showCostOverride is null && item.CanPreviewCost());
        var costText = RunCollection.Join(", ", costs.Select(c => c.ToCostText(showCost)));

        return
        [
            itemText with
            {
                Color =
                    (canPayPlacement && costs.All(c => c.CanPay())) ? RmcColors.GetColor(RmcColorSetting.UI_On) : null,
            },
            new Run(": "),
            .. costText,
        ];
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
