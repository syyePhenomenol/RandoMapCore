using ItemChanger;
using ItemChanger.Placements;

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
            ?? Finder.GetLocation(placement.Name);
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

    internal static bool CanPreview(this TaggableObject taggable)
    {
        return !taggable.HasTag<ItemChanger.Tags.DisableItemPreviewTag>();
    }

    internal static string[] GetTagPreviewText(this AbstractPlacement placement)
    {
        List<string> texts = [];

        if (!placement.CheckVisitedAny(VisitState.Previewed))
        {
            return [.. texts];
        }

        if (
            placement.GetTag<ItemChanger.Tags.MultiPreviewRecordTag>() is ItemChanger.Tags.MultiPreviewRecordTag mprt
            && mprt.previewTexts is not null
        )
        {
            for (var i = 0; i < mprt.previewTexts.Length; i++)
            {
                var t = mprt.previewTexts[i];
                if (!string.IsNullOrEmpty(t) && i < placement.Items.Count && !placement.Items[i].WasEverObtained())
                {
                    texts.Add(t);
                }
            }
        }
        else if (
            placement.GetTag<ItemChanger.Tags.PreviewRecordTag>() is ItemChanger.Tags.PreviewRecordTag prt
            && !string.IsNullOrEmpty(prt.previewText)
            && !placement.Items.All(i => i.WasEverObtained())
        )
        {
            texts.Add(prt.previewText);
        }

        return [.. texts];
    }

    internal static bool IsPersistent(this AbstractItem item)
    {
        return item.HasTag<ItemChanger.Tags.PersistentItemTag>();
    }

    internal static bool AllEverObtained(this AbstractPlacement placement)
    {
        return placement.Items.All(i => i.WasEverObtained());
    }
}
