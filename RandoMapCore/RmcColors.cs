using GlobalEnums;
using MapChanger;
using UnityEngine;

namespace RandoMapCore;

public enum RmcColorSetting
{
    None,

    UI_On,
    UI_Neutral,
    UI_Custom,
    UI_Disabled,
    UI_Special,
    UI_Borders,

    UI_Compass,

    Pin_Normal,
    Pin_Previewed,
    Pin_Out_of_logic,
    Pin_Persistent,
    Pin_Cleared,

    Map_Ancient_Basin,
    Map_City_of_Tears,
    Map_Crystal_Peak,
    Map_Deepnest,
    Map_Dirtmouth,
    Map_Fog_Canyon,
    Map_Forgotten_Crossroads,
    Map_Fungal_Wastes,
    Map_Godhome,
    Map_Greenpath,
    Map_Howling_Cliffs,
    Map_Kingdoms_Edge,
    Map_Queens_Gardens,
    Map_Resting_Grounds,
    Map_Royal_Waterways,
    Map_White_Palace,

    Map_Abyss,
    Map_Hive,
    Map_Ismas_Grove,
    Map_Mantis_Village,
    Map_Queens_Station,
    Map_Soul_Sanctum,
    Map_Watchers_Spire,

    Room_Normal,
    Room_Current,
    Room_Adjacent,
    Room_Sequence_break,
    Room_Out_of_logic,
    Room_Selected,
    Room_Debug,
}

public class RmcColors : HookModule
{
    internal static readonly RmcColorSetting[] PinBorderColors =
    [
        RmcColorSetting.Pin_Normal,
        RmcColorSetting.Pin_Previewed,
        RmcColorSetting.Pin_Out_of_logic,
        RmcColorSetting.Pin_Persistent,
        RmcColorSetting.Pin_Cleared,
    ];

    internal static readonly RmcColorSetting[] RoomColors =
    [
        RmcColorSetting.Room_Normal,
        RmcColorSetting.Room_Current,
        RmcColorSetting.Room_Adjacent,
        RmcColorSetting.Room_Sequence_break,
        RmcColorSetting.Room_Out_of_logic,
        RmcColorSetting.Room_Selected,
    ];

    private static Dictionary<RmcColorSetting, Vector4> _customColors = [];

    private static readonly Dictionary<RmcColorSetting, Vector4> _defaultColors =
        new()
        {
            { RmcColorSetting.Pin_Normal, Color.white },
            { RmcColorSetting.Pin_Previewed, Color.green },
            { RmcColorSetting.Pin_Out_of_logic, Color.red },
            { RmcColorSetting.Pin_Persistent, Color.cyan },
            { RmcColorSetting.Pin_Cleared, Color.magenta },
            { RmcColorSetting.Room_Normal, new(1f, 1f, 1f, 0.3f) }, // white
            { RmcColorSetting.Room_Current, new(0, 1f, 0, 0.4f) }, // green
            { RmcColorSetting.Room_Adjacent, new(0, 1f, 1f, 0.4f) }, // cyan
            { RmcColorSetting.Room_Sequence_break, new(1f, 0.5f, 0f, 0.3f) }, // orange
            { RmcColorSetting.Room_Out_of_logic, new(1f, 0, 0, 0.3f) }, // red
            { RmcColorSetting.Room_Selected, new(1f, 1f, 0, 0.7f) }, // yellow
            { RmcColorSetting.Room_Debug, new(0, 0, 1f, 0.5f) }, // blue
            { RmcColorSetting.UI_Compass, new(1f, 1f, 1f, 0.83f) },
        };

    public static bool HasCustomColors { get; private set; } = false;

    public override void OnEnterGame()
    {
        Dictionary<string, float[]> customColorsRaw;

        try
        {
            customColorsRaw = JsonUtil.DeserializeFromExternalFile<Dictionary<string, float[]>>(
                Path.Combine(Path.GetDirectoryName(RandoMapCoreMod.Assembly.Location), "colors.json")
            );
        }
        catch (Exception)
        {
            RandoMapCoreMod.Instance.LogDebug("colors.json file doesn't exist or is invalid. Using default colors");
            return;
        }

        if (customColorsRaw is not null)
        {
            foreach (var colorSettingRaw in customColorsRaw.Keys)
            {
                if (
                    !Enum.TryParse(colorSettingRaw, out RmcColorSetting colorSetting)
                    || _customColors.ContainsKey(colorSetting)
                )
                {
                    continue;
                }

                var rgba = customColorsRaw[colorSettingRaw];

                if (rgba is null || rgba.Length < 4)
                {
                    continue;
                }

                Vector4 color = new(rgba[0] / 256f, rgba[1] / 256f, rgba[2] / 256f, rgba[3]);

                _customColors.Add(colorSetting, color);
            }

            MapChangerMod.Instance.Log("Custom colors loaded");
            HasCustomColors = true;
        }
        else
        {
            MapChangerMod.Instance.Log("No colors.json found. Using default colors");
        }
    }

    public override void OnQuitToMenu()
    {
        _customColors = [];
    }

    public static Vector4 GetColor(RmcColorSetting rmcColor)
    {
        if (_customColors.TryGetValue(rmcColor, out var customColor))
        {
            return customColor;
        }

        if (_defaultColors.TryGetValue(rmcColor, out var defaultColor))
        {
            return defaultColor;
        }

        if (Enum.TryParse(rmcColor.ToString(), out ColorSetting mcColor))
        {
            return Colors.GetColor(mcColor);
        }

        return Vector4.negativeInfinity;
    }

    public static Vector4 GetColor(ColorSetting mcColor)
    {
        if (Enum.TryParse(mcColor.ToString(), out RmcColorSetting rmcColor))
        {
            return GetColor(rmcColor);
        }

        return Vector4.negativeInfinity;
    }

    public static Vector4 GetColorFromMapZone(MapZone mapZone)
    {
        return mapZone switch
        {
            MapZone.ABYSS => GetColor(RmcColorSetting.Map_Ancient_Basin),
            MapZone.CITY => GetColor(RmcColorSetting.Map_City_of_Tears),
            MapZone.CLIFFS => GetColor(RmcColorSetting.Map_Howling_Cliffs),
            MapZone.CROSSROADS => GetColor(RmcColorSetting.Map_Forgotten_Crossroads),
            MapZone.MINES => GetColor(RmcColorSetting.Map_Crystal_Peak),
            MapZone.DEEPNEST => GetColor(RmcColorSetting.Map_Deepnest),
            MapZone.TOWN => GetColor(RmcColorSetting.Map_Dirtmouth),
            MapZone.FOG_CANYON => GetColor(RmcColorSetting.Map_Fog_Canyon),
            MapZone.WASTES => GetColor(RmcColorSetting.Map_Fungal_Wastes),
            MapZone.GREEN_PATH => GetColor(RmcColorSetting.Map_Greenpath),
            MapZone.OUTSKIRTS => GetColor(RmcColorSetting.Map_Kingdoms_Edge),
            MapZone.ROYAL_GARDENS => GetColor(RmcColorSetting.Map_Queens_Gardens),
            MapZone.RESTING_GROUNDS => GetColor(RmcColorSetting.Map_Resting_Grounds),
            MapZone.WATERWAYS => GetColor(RmcColorSetting.Map_Royal_Waterways),
            MapZone.WHITE_PALACE => GetColor(RmcColorSetting.Map_White_Palace),
            MapZone.GODS_GLORY => GetColor(RmcColorSetting.Map_Godhome),
            _ => Color.white,
        };
    }
}
