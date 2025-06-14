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
        ("RMC-Opened_Dung_Defender_Wall", nameof(PlayerData.dungDefenderWallBroken)),
        ("RMC-Opened_Mawlek_Wall", nameof(PlayerData.crossroadsMawlekWall)),
        ("RMC-Opened_Lower_Kingdom's_Edge_Wall", nameof(PlayerData.outskirtsWall)),
        ("RMC-Opened_Mantis_Lords_Door", nameof(PlayerData.defeatedMantisLords)),
        ("RMC-Opened_Archives_Exit_Wall", nameof(PlayerData.oneWayArchive)),
        ("RMC-Opened_Gardens_Stag_Exit", nameof(PlayerData.openedGardensStagStation)),
        ("RMC-Activated_Left_Elevator", nameof(PlayerData.cityLift1)),
        ("RMC-Opened_Resting_Grounds_Floor", nameof(PlayerData.openedRestingGrounds02)),
        ("RMC-Opened_Glade_Door", nameof(PlayerData.gladeDoorOpened)),
        ("RMC-Broke_Sanctum_Glass_Floor", nameof(PlayerData.brokenMageWindow)),
        ("RMC-Opened_Pleasure_House", nameof(PlayerData.bathHouseOpened)),
        ("RMC-Opened_Elegant_Door", nameof(PlayerData.openedMageDoor_v2)),
        ("RMC-Opened_Emilitia_Door", nameof(PlayerData.city2_sewerDoor)),
        ("RMC-Opened_Resting_Grounds_Catacombs_Wall", nameof(PlayerData.restingGroundsCryptWall)),
        ("RMC-Opened_Pleasure_House_Wall", nameof(PlayerData.bathHouseWall)),
        ("RMC-Opened_Love_Door", nameof(PlayerData.openedLoveDoor)),
        ("RMC-Opened_Bretta_Door", nameof(PlayerData.brettaRescued)),
        ("RMC-Opened_Jiji_Door", nameof(PlayerData.jijiDoorUnlocked)),
        ("RMC-Opened_Sly_Door", nameof(PlayerData.slyRescued)),
        ("RMC-Opened_Dirtmouth_Stag_Door", nameof(PlayerData.openedTownBuilding)),
        ("RMC-Activated_Dirtmouth_Lift", nameof(PlayerData.mineLiftOpened)),
        ("RMC-Opened_Divine_Door", nameof(PlayerData.divineInTown)),
        ("RMC-Opened_Grimm_Door", nameof(PlayerData.troupeInTown)),
        ("RMC-Opened_Waterways_Manhole", nameof(PlayerData.openedWaterwaysManhole)),
        ("RMC-Drained_Waterways_Acid", nameof(PlayerData.waterwaysAcidDrained)),
    ];

    public override ProgressionManager ReferencePM =>
        sequenceBreak ? RandoMapCoreMod.Data.PM : RandoMapCoreMod.Data.PMNoSequenceBreak;

    protected override void ManuallyUpdateTerms()
    {
        // Update stateless waypoint terms
        var pd = PlayerData.instance;

        foreach ((var term, var pdBool) in _pdBoolTerms)
        {
            LocalPM.Set(term, pd.GetBool(pdBool) ? 1 : 0);
        }

        if (LocalPM.lm.GetTerm("RMC-Opened_Shaman_Pillar") is Term openedShamanPillar)
        {
            LocalPM.Set(
                openedShamanPillar,
                pd.GetBool(nameof(PlayerData.shamanPillar)) || pd.GetBool(nameof(PlayerData.crossroadsInfected)) ? 1 : 0
            );
        }

        if (LocalPM.lm.GetTerm("RMC-Blue_Room_Door") is Term blueRoomDoor)
        {
            LocalPM.Set(
                blueRoomDoor,
                pd.GetBool(nameof(PlayerData.blueRoomDoorUnlocked))
                || (BossSequenceBindingsDisplay.CountCompletedBindings() >= 8)
                    ? 1
                    : 0
            );
        }

        if (LocalPM.lm.GetTerm("RMC-Godhome_Roof") is Term godhomeRoof)
        {
            LocalPM.Set(
                godhomeRoof,
                pd.GetVariable<BossSequenceDoor.Completion>("bossDoorStateTier5").unlocked ? 1 : 0
            );
        }

        foreach (var pbd in SceneData.instance.persistentBoolItems)
        {
            switch (pbd.sceneName)
            {
                // case SN.Cliffs_02:
                // if (pbd.id is "One Way Wall")
                // {
                //     LocalPM.Set("RMC-Plank_Cliff's_Pass", pbd.activated ? 1 : 0);
                // }

                // break;
                case SN.Crossroads_10:
                    if (pbd.id is "Battle Scene")
                    {
                        LocalPM.Set("RMC-Opened_False_Knight_Gate", pbd.activated ? 1 : 0);
                    }

                    break;
                case SN.Fungus2_14:
                    if (pbd.id is "Mantis Lever (1)")
                    {
                        LocalPM.Set("RMC-Opened_Mantis_Village", pbd.activated ? 1 : 0);
                    }

                    break;
                case SN.RestingGrounds_10:
                    if (pbd.id is "Collapser Small (5)")
                    {
                        LocalPM.Set("RMC-Broke_Crypts_One_Way_Floor", pbd.activated ? 1 : 0);
                    }

                    break;
                // case SN.Ruins1_27:
                //     if (pbd.id is "Ruins Lever")
                //     {
                //         LocalPM.Set("RMC-Lever_Fountain", pbd.activated ? 1 : 0);
                //     }

                //     break;
                case SN.Ruins1_31:
                    if (pbd.id is "Breakable Wall Ruin Lift")
                    {
                        LocalPM.Set("RMC-Broke_City_Toll_Wall", pbd.activated ? 1 : 0);
                    }

                    if (pbd.id is "Ruins Lever")
                    {
                        LocalPM.Set("RMC-Opened_Shade_Soul_Exit", pbd.activated ? 1 : 0);
                    }

                    break;
                case SN.Waterways_02:
                    if (pbd.id is "Quake Floor")
                    {
                        LocalPM.Set("RMC-Broke_Flukemarm_Quake_Floor", pbd.activated ? 1 : 0);
                    }

                    if (pbd.id is "Quake Floor (1)")
                    {
                        LocalPM.Set("RMC-Broke_Waterways_Bench_Floor", pbd.activated ? 1 : 0);
                    }

                    break;
                // case SN.Waterways_04:
                //     if (pbd.id is "Quake Floor")
                //     {
                //         LocalPM.Set("RMC-Broke_Waterways_Bench_Floor_1", pbd.activated ? 1 : 0);
                //     }
                //     if (pbd.id is "Quake Floor (1)")
                //     {
                //         LocalPM.Set("RMC-Broke_Waterways_Bench_Floor_2", pbd.activated ? 1 : 0);
                //     }
                //     break;
                case SN.Waterways_05:
                    if (pbd.id is "Quake Floor")
                    {
                        LocalPM.Set("RMC-Broke_Dung_Defender_Quake_Floor", pbd.activated ? 1 : 0);
                    }

                    break;
                default:
                    break;
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
}
