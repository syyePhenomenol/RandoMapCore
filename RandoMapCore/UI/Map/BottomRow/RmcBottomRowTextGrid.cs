using MapChanger.UI;

namespace RandoMapCore.UI;

internal class RmcBottomRowTextGrid : BottomRowTextGrid
{
    protected override IEnumerable<BottomRowText> GetTexts()
    {
        List<BottomRowText> texts = [];

        if (RandoMapCoreMod.Data.EnableSpoilerToggle)
        {
            texts.Add(new SpoilersText());
        }

        texts.AddRange([new RandomizedText(), new VanillaText()]);

        if (RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            texts.AddRange([new ShapeText(), new SizeText()]);
        }

        return texts;
    }
}
