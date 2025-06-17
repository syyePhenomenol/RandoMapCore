using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder.Logic;
using SN = ItemChanger.SceneNames;

namespace RandoMapCore.Pathfinder;

internal class RmcProgressionSynchronizer(RmcLogicExtender logicExtender, RandoContext ctx, bool sequenceBreak)
    : ProgressionSynchronizer(logicExtender, ctx)
{
    private static readonly (string term, string pdBool)[] _pdBoolTerms =
    [
        ("RMC-Activated_Left_Elevator", nameof(PlayerData.cityLift1)),
        ("RMC-Broke_Sanctum_Glass_Floor", nameof(PlayerData.brokenMageWindow)),
        ("RMC-Defeated_Broken_Vessel", nameof(PlayerData.killedInfectedKnight)),
        ("RMC-Defeated_Hornet_2", nameof(PlayerData.hornetOutskirtsDefeated)),
        ("RMC-Defeated_Mantis_Lords", nameof(PlayerData.defeatedMantisLords)),
        ("RMC-Lever_City_Above_Lemm_Left", nameof(PlayerData.ruins1_5_tripleDoor)),
        ("RMC-Lever_Dirtmouth_Elevator", nameof(PlayerData.mineLiftOpened)),
        ("RMC-Lever_Dung_Defender", nameof(PlayerData.waterwaysAcidDrained)),
        ("RMC-Lever_Emilitia", nameof(PlayerData.city2_sewerDoor)),
        ("RMC-Lever_Failed_Tramway_Left", nameof(PlayerData.deepnest26b_switch)),
        ("RMC-Lever_Waterways_Hwurmp_Arena", nameof(PlayerData.waterwaysGate)),
        ("RMC-Lever_Palace_Right", nameof(PlayerData.whitePalace05_lever)),
        ("RMC-Plank_Archives_Outer", nameof(PlayerData.oneWayArchive)),
        ("RMC-Opened_Divine_Door", nameof(PlayerData.divineInTown)),
        ("RMC-Plank_Dung_Defender", nameof(PlayerData.dungDefenderWallBroken)),
        ("RMC-Opened_Elegant_Door", nameof(PlayerData.openedMageDoor_v2)),
        ("RMC-Opened_Glade_Door", nameof(PlayerData.gladeDoorOpened)),
        ("RMC-Opened_Grimm_Door", nameof(PlayerData.troupeInTown)),
        ("RMC-Opened_Jiji_Door", nameof(PlayerData.jijiDoorUnlocked)),
        ("RMC-Opened_Love_Door", nameof(PlayerData.openedLoveDoor)),
        ("RMC-Plank_Edge_Tram_Exit", nameof(PlayerData.outskirtsWall)),
        ("RMC-Plank_Brooding_Mawlek", nameof(PlayerData.crossroadsMawlekWall)),
        ("RMC-Wall_Pleasure_House", nameof(PlayerData.bathHouseWall)),
        ("RMC-Opened_Pleasure_House", nameof(PlayerData.bathHouseOpened)),
        ("RMC-Plank_Catacombs_Elevator", nameof(PlayerData.restingGroundsCryptWall)),
        ("RMC-Opened_Waterways_Manhole", nameof(PlayerData.openedWaterwaysManhole)),
        ("RMC-Rescued_Bretta", nameof(PlayerData.brettaRescued)),
        ("RMC-Rescued_Sly", nameof(PlayerData.slyRescued)),
        ("RMC-Switch_Dirtmouth_Stag", nameof(PlayerData.openedTownBuilding)),
        ("RMC-Switch_Lower_Resting_Grounds", nameof(PlayerData.openedRestingGrounds02)),
        ("RMC-Switch_Queen's_Gardens_Stag", nameof(PlayerData.openedGardensStagStation)),
    ];

    private static readonly Dictionary<(string, string), string> _sceneBoolTerms =
        new()
        {
            { ("Abyss_19", "One Way Wall"), "RMC-Plank_Broken_Vessel_Shortcut" },
            { ("Cliffs_02", "One Way Wall"), "RMC-Plank_Cliff's_Pass" },
            { ("Crossroads_03", "Toll Gate Switch"), "RMC-Switch_Crossroads_East" },
            { ("Crossroads_10", "Battle Scene"), "RMC-Opened_False_Knight_Gate" },
            { ("Crossroads_11_alt", "Blocker"), "RMC-Defeated_Crossroads_Baldur" },
            { ("Crossroads_21", "Collapser Small"), "RMC-Collapser_Glowing_Womb_Tunnel" },
            { ("Deepnest_01", "One Way Wall"), "RMC-Plank_Deepnest_Exit" },
            { ("Deepnest_01b", "One Way Wall"), "RMC-Plank_Upper_Deepnest" },
            { ("Deepnest_14", "Collapser Small"), "RMC-Collapser_Failed_Tramway_Bench" },
            { ("Deepnest_16", "Collapser Small (1)"), "RMC-Collapser_Deepnest_By_Mantis_Lords_Path" },
            { ("Deepnest_26", "Ruins Lever"), "RMC-Lever_Failed_Tramway_Right" },
            { ("Deepnest_31", "One Way Wall"), "RMC-Plank_Nosk_Exit" },
            { ("Deepnest_33", "Collapser Small"), "RMC-Collapser_Deepnest_Zote" },
            { ("Deepnest_41", "One Way Wall (1)"), "RMC-Plank_Village_Upper" },
            { ("Deepnest_41", "One Way Wall (2)"), "RMC-Plank_Village_Lower" },
            { ("Deepnest_East_01", "One Way Wall"), "RMC-Plank_Hive_Exit" },
            { ("Deepnest_East_02", "Quake Floor"), "RMC-Dive_Floor_Lower_Edge" },
            { ("Deepnest_East_06", "One Way Wall"), "RMC-Plank_Quickslash_Exit" },
            { ("Deepnest_East_09", "One Way Wall"), "RMC-Plank_Colo_Shortcut" },
            { ("Deepnest_East_14", "Quake Floor (1)"), "RMC-Dive_Floor_Oro_2" },
            { ("Deepnest_East_14", "Quake Floor (2)"), "RMC-Dive_Floor_Oro_3" },
            { ("Fungus1_04", "Break Floor 1"), "RMC-Plank_Hornet_Exit" },
            { ("Fungus1_22", "Gate Switch"), "RMC-Switch_Greenpath_Stag" },
            { ("Fungus2_04", "Mantis Lever (1)"), "RMC-Lever_Fungal_Wastes_Below_Shrumal_Ogres" },
            { ("Fungus2_14", "Mantis Lever (1)"), "RMC-Lever_Mantis_Lords_Access" },
            { ("Fungus2_18", "Mantis Lever"), "RMC-Lever_Fungal_Wastes_Bouncy_Grub" },
            { ("Fungus2_21", "Quake Floor"), "RMC-Dive_Floor_Pilgrim's_Way" },
            { ("Fungus2_29", "Break Floor 1"), "RMC-Plank_Fungal_Core" },
            { ("Fungus3_04", "Ruins Lever"), "RMC-Lever_Queen's_Gardens_Ground_Block" },
            { ("Fungus3_05", "Gate Switch"), "RMC-Switch_Petra_Arena" },
            { ("Fungus3_39", "One Way Wall"), "RMC-Plank_Moss_Prophet" },
            { ("Fungus3_44", "Ruins Lever"), "RMC-Lever_Below_Overgrown_Mound" },
            { ("Hive_03_c", "Break Floor 1"), "RMC-Plank_Hive_Mask" },
            { ("Mines_01", "mine_1_quake_floor"), "RMC-Dive_Floor_Peak_Entrance" },
            { ("Mines_04", "Mines Lever"), "RMC-Lever_Crystal_Peak_Below_Chest" },
            { ("Mines_06", "Collapser Small"), "RMC-Collapser_Deep_Focus" },
            { ("Mines_20", "Mines Lever"), "RMC-Lever_Crystal_Peak_Tall_Room_Lower" },
            { ("Mines_20", "Mines Lever (2)"), "RMC-Lever_Crystal_Peak_Tall_Room_Middle" },
            { ("Mines_20", "Mines Lever (1)"), "RMC-Lever_Crystal_Peak_Tall_Room_Upper" },
            { ("Mines_37", "Mines Lever New"), "RMC-Lever_Crystal_Peak_Crushers_Chest" },
            { ("RestingGrounds_10", "Collapser Small (5)"), "RMC-Collapser_Catacombs_Entrance" },
            { ("Ruins1_04", "One Way Wall"), "RMC-Plank_Nailsmith" },
            { ("Ruins1_05", "Ruins Lever"), "RMC-Lever_City_Above_Lemm_Upper" },
            { ("Ruins1_05b", "Ruins Lever 1"), "RMC-Lever_City_Lemm" },
            { ("Ruins1_05c", "Ruins Lever 2"), "RMC-Lever_City_Above_Lemm_Right" },
            { ("Ruins1_17", "Ruins Lever"), "RMC-Lever_City_Storerooms" },
            { ("Ruins1_18", "Ruins Lever"), "RMC-Lever_City_Bridge_Above_Fountain" },
            { ("Ruins1_23", "Ruins Lever"), "RMC-Lever_Sanctum_Soul_Warrior" },
            { ("Ruins1_23", "Ruins Lever (1)"), "RMC-Lever_Sanctum_Bottom" },
            { ("Ruins1_23", "Battle Scene v2"), "RMC-Defeated_Sanctum_Warrior" },
            { ("Ruins1_25", "Ruins Lever"), "RMC-Lever_Sanctum_East" },
            { ("Ruins1_27", "Ruins Lever"), "RMC-Lever_City_Fountain" },
            { ("Ruins1_30", "Quake Floor Glass"), "RMC-Dive_Floor_Sanctum_Escape_1" },
            { ("Ruins1_30", "Quake Floor Glass (1)"), "RMC-Dive_Floor_Sanctum_Escape_2" },
            { ("Ruins1_30", "Ruins Lever"), "RMC-Lever_Below_Spell_Twister" },
            { ("Ruins1_31", "Breakable Wall Ruin Lift"), "RMC-Wall_Shade_Soul_Shortcut" },
            { ("Ruins1_31", "Ruins Lever"), "RMC-Lever_Shade_Soul_Exit" },
            { ("Ruins1_32", "Quake Floor Glass (1)"), "RMC-Dive_Floor_Inner_Sanctum_2" },
            { ("Ruins1_32", "Quake Floor Glass (2)"), "RMC-Dive_Floor_Inner_Sanctum_Seal" },
            { ("Ruins1_32", "Quake Floor Glass (3)"), "RMC-Dive_Floor_Inner_Sanctum_Lever" },
            { ("Ruins1_32", "Quake Floor Glass"), "RMC-Dive_Floor_Inner_Sanctum_1" },
            { ("Ruins2_01", "Ruins Lever"), "RMC-Lever_City_Spire_Sentry_Lower" },
            { ("Ruins2_01", "Ruins Lever (1)"), "RMC-Lever_City_Spire_Sentry_Upper" },
            { ("Ruins2_11_b", "Ruins Lever"), "RMC-Lever_Tower_of_Love" },
            { ("Ruins2_15", "Mantis Lever (4)"), "RMC-Lever_Mantis_Lords_Middle_Right" },
            { ("Waterways_02", "Quake Floor (1)"), "RMC-Dive_Floor_Above_Waterways_Bench" },
            { ("Waterways_02", "Quake Floor"), "RMC-Dive_Floor_Flukemarm" },
            { ("Waterways_04", "Quake Floor"), "RMC-Dive_Floor_Waterways_Bench_1" },
            { ("Waterways_04", "Quake Floor (1)"), "RMC-Dive_Floor_Waterways_Bench_2" },
            { ("Waterways_05", "Quake Floor"), "RMC-Dive_Floor_Dung_Defender" },
            { ("Waterways_07", "One Way Wall"), "RMC-Plank_Isma's_Grove" },
            { ("Waterways_08", "Break Floor 1"), "RMC-Plank_Junk_Pit_Exit" },
            { ("White_Palace_03_hub", "WP Lever"), "RMC-Lever_Palace_Atrium" },
            { ("White_Palace_12", "WP Lever"), "RMC-Lever_Palace_Final" },
            { ("White_Palace_17", "WP Lever"), "RMC-Lever_Path_of_Pain" },
        };

    public override ProgressionManager ReferencePM =>
        sequenceBreak ? RandoMapCoreMod.Data.PM : RandoMapCoreMod.Data.PMNoSequenceBreak;

    protected override void ManuallyUpdateTerms()
    {
        try
        {
            // Update stateless waypoint terms
            var pd = PlayerData.instance;

            foreach ((var term, var pdBool) in _pdBoolTerms)
            {
                LocalPM.Set(term, pd.GetBool(pdBool) ? 1 : 0);
            }

            LocalPM.Set(
                "RMC-Switch_Outside_Ancestral_Mound",
                pd.GetBool(nameof(PlayerData.shamanPillar)) || pd.GetBool(nameof(PlayerData.crossroadsInfected)) ? 1 : 0
            );

            LocalPM.Set(
                "RMC-Opened_Blue_Room_Door",
                pd.GetBool(nameof(PlayerData.blueRoomDoorUnlocked))
                || (BossSequenceBindingsDisplay.CountCompletedBindings() >= 8)
                    ? 1
                    : 0
            );

            LocalPM.Set(
                "RMC-Opened_Godhome_Roof",
                pd.GetVariable<BossSequenceDoor.Completion>("bossDoorStateTier5").unlocked ? 1 : 0
            );

            foreach (var pbd in SceneData.instance.persistentBoolItems)
            {
                if (_sceneBoolTerms.TryGetValue((pbd.sceneName, pbd.id), out var term))
                {
                    LocalPM.Set(term, pbd.activated ? 1 : 0);
                }
            }

            foreach (var pid in SceneData.instance.persistentIntItems)
            {
                if (pid.sceneName is SN.Ruins1_31 && pid.id is "Ruins Lift")
                {
                    LocalPM.Set("RMC-City_Toll_Lift_Up", pid.value % 2 is 1 ? 1 : 0);
                    LocalPM.Set("RMC-City_Toll_Lift_Down", pid.value % 2 is 0 ? 1 : 0);
                }
            }

            if (Interop.HasBenchwarp)
            {
                foreach (var bench in BenchwarpInterop.GetVisitedBenchNames())
                {
                    if (LocalPM.lm.GetTerm(bench) is Term benchTerm && LocalPM.GetState(benchTerm) is null)
                    {
                        LocalPM.SetState(benchTerm, StateUnion.Empty);
                    }
                }
            }
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError($"Failed to update terms in ProgressionSynchronizer:\n{e}");
        }
    }
}
