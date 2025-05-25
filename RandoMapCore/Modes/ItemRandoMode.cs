using MapChanger;
using MapChanger.Defs;
using MapChanger.MonoBehaviours;
using RandoMapCore.Settings;
using UnityEngine;

namespace RandoMapCore.Modes;

public abstract class ItemRandoMode : RmcMapMode
{
    public override Vector4? RoomColorOverride(RoomSprite roomSprite)
    {
        return GetCustomColor(roomSprite.Rsd.ColorSetting);
    }

    public override bool DisableAreaNames =>
        RandoMapCoreMod.Data.EnableVisualCustomization && !RandoMapCoreMod.GS.ShowAreaNames;

    public override Vector4? AreaNameColorOverride(AreaName areaName)
    {
        return GetCustomColor(areaName.MiscObjectDef.ColorSetting);
    }

    public override bool? NextAreaNameActiveOverride(NextAreaName nextAreaName)
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return null;
        }

        return RandoMapCoreMod.GS.ShowNextAreas switch
        {
            NextAreaSetting.Off or NextAreaSetting.Arrows => false,
            NextAreaSetting.Full or _ => null,
        };
    }

    public override bool? NextAreaArrowActiveOverride(NextAreaArrow nextAreaArrow)
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return null;
        }

        return RandoMapCoreMod.GS.ShowNextAreas switch
        {
            NextAreaSetting.Off => false,
            NextAreaSetting.Arrows or NextAreaSetting.Full or _ => null,
        };
    }

    public override Vector4? NextAreaColorOverride(MiscObjectDef miscObjectDef)
    {
        return GetCustomColor(miscObjectDef.ColorSetting);
    }

    private Vector4? GetCustomColor(ColorSetting colorSetting)
    {
        var customColor = RmcColors.GetColor(colorSetting);

        if (!customColor.Equals(Vector4.negativeInfinity))
        {
            return customColor.ToOpaque();
        }

        return null;
    }

    public override Vector4? QuickMapTitleColorOverride(QuickMapTitle qmt)
    {
        var customColor = RmcColors.GetColorFromMapZone(Finder.GetCurrentMapZone());

        if (!customColor.Equals(Vector4.negativeInfinity))
        {
            return customColor.ToOpaque();
        }

        return null;
    }
}

public class FullMapMode : ItemRandoMode
{
    public override string ModeName => RmcMode.Full_Map.ToString().ToCleanName();

    public override bool? NextAreaNameActiveOverride(NextAreaName nextAreaName)
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return true;
        }

        return RandoMapCoreMod.GS.ShowNextAreas switch
        {
            NextAreaSetting.Off or NextAreaSetting.Arrows => false,
            NextAreaSetting.Full or _ => true,
        };
    }

    public override bool? NextAreaArrowActiveOverride(NextAreaArrow nextAreaArrow)
    {
        if (!RandoMapCoreMod.Data.EnableVisualCustomization)
        {
            return true;
        }

        return RandoMapCoreMod.GS.ShowNextAreas switch
        {
            NextAreaSetting.Off => false,
            NextAreaSetting.Arrows or NextAreaSetting.Full or _ => true,
        };
    }
}

public class AllPinsMode : ItemRandoMode
{
    public override string ModeName => RmcMode.All_Pins.ToString().ToCleanName();
    public override bool FullMap => false;
}

public class PinsOverAreaMode : ItemRandoMode
{
    public override string ModeName => RmcMode.Pins_Over_Area.ToString().ToCleanName();
    public override bool FullMap => false;
}

public class PinsOverRoomMode : ItemRandoMode
{
    public override string ModeName => RmcMode.Pins_Over_Room.ToString().ToCleanName();
    public override bool FullMap => false;
}
