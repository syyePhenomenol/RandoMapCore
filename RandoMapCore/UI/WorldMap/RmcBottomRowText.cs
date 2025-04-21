using MapChanger.UI;
using RandoMapCore.Input;
using RandoMapCore.Localization;
using RandoMapCore.Modes;
using RandoMapCore.Settings;

namespace RandoMapCore.UI;

internal sealed class RmcBottomRowText : BottomRowText
{
    protected override float MinSpacing => 250f;
    protected override string[] TextNames =>
        RandoMapCoreMod.Data.EnableSpoilerToggle
            ? ["Spoilers", "Randomized", "Vanilla", "Shape", "Size"]
            : ["Randomized", "Vanilla", "Shape", "Size"];

    protected override bool Condition()
    {
        return base.Condition() && Conditions.RandoCoreMapModEnabled();
    }

    public override void Update()
    {
        UpdateSpoilers();
        UpdateRandomized();
        UpdateVanilla();
        UpdateShape();
        UpdateSize();
    }

    private void UpdateSpoilers()
    {
        if (!MapTexts.TryGetValue("Spoilers", out var textObj))
        {
            return;
        }

        var text = $"{"Spoilers".L()} {ToggleSpoilersInput.Instance.GetBindingsText()}: ";

        if (RandoMapCoreMod.LS.SpoilerOn)
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
            text += "on".L();
        }
        else
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
            text += "off".L();
        }

        textObj.Text = text;
    }

    private void UpdateRandomized()
    {
        if (!MapTexts.TryGetValue("Randomized", out var textObj))
        {
            return;
        }

        var text = $"{"Randomized".L()} {ToggleRandomizedInput.Instance.GetBindingsText()}: ";

        if (RandoMapCoreMod.LS.RandomizedOn)
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
            text += "on".L();
        }
        else
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
            text += "off".L();
        }

        if (RandoMapCoreMod.LS.IsRandomizedCustom())
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
        }

        textObj.Text = text;
    }

    private void UpdateVanilla()
    {
        if (!MapTexts.TryGetValue("Vanilla", out var textObj))
        {
            return;
        }

        var text = $"{"Vanilla".L()} {ToggleVanillaInput.Instance.GetBindingsText()}: ";

        if (RandoMapCoreMod.LS.VanillaOn)
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_On);
            text += "on".L();
        }
        else
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
            text += "off".L();
        }

        if (RandoMapCoreMod.LS.IsVanillaCustom())
        {
            textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Custom);
        }

        textObj.Text = text;
    }

    private void UpdateShape()
    {
        if (!MapTexts.TryGetValue("Shape", out var textObj))
        {
            return;
        }

        var text = $"{"Shape".L()} {ToggleShapeInput.Instance.GetBindingsText()}: ";

        switch (RandoMapCoreMod.GS.PinShapes)
        {
            case PinShapeSetting.Mixed:
                text += "mixed".L();
                break;

            case PinShapeSetting.All_Circle:
                text += "circles".L();
                break;

            case PinShapeSetting.All_Diamond:
                text += "diamonds".L();
                break;

            case PinShapeSetting.All_Square:
                text += "squares".L();
                break;

            case PinShapeSetting.All_Pentagon:
                text += "pentagons".L();
                break;

            case PinShapeSetting.All_Hexagon:
                text += "hexagons".L();
                break;

            case PinShapeSetting.No_Border:
                text += "no borders".L();
                break;
            default:
                break;
        }

        textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        textObj.Text = text;
    }

    private void UpdateSize()
    {
        if (!MapTexts.TryGetValue("Size", out var textObj))
        {
            return;
        }

        var text = $"{"Size".L()} {ToggleSizeInput.Instance.GetBindingsText()}: ";

        switch (RandoMapCoreMod.GS.PinSize)
        {
            case PinSize.Tiny:
                text += "tiny".L();
                break;

            case PinSize.Small:
                text += "small".L();
                break;

            case PinSize.Medium:
                text += "medium".L();
                break;

            case PinSize.Large:
                text += "large".L();
                break;

            case PinSize.Huge:
                text += "huge".L();
                break;
            default:
                break;
        }

        textObj.ContentColor = RmcColors.GetColor(RmcColorSetting.UI_Neutral);
        textObj.Text = text;
    }
}
