using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder.Logic;
using SN = ItemChanger.SceneNames;

namespace RandoMapCore.Pathfinder;

internal class RmcProgressionSynchronizer(ProgressionManager reference, RmcLogicExtender logicExtender)
    : ProgressionSynchronizer(reference, logicExtender)
{
    private static readonly (string term, string pdBool)[] _pdBoolTerms =
    [
        ("RMC_Dung_Defender_Wall", nameof(PlayerData.dungDefenderWallBroken)),
        ("RMC_Mawlek_Wall", nameof(PlayerData.crossroadsMawlekWall)),
        ("RMC_Shaman_Pillar", nameof(PlayerData.shamanPillar)),
        ("RMC_Lower_Kingdom's_Edge_Wall", nameof(PlayerData.outskirtsWall)),
        ("RMC_Mantis_Big_Door", nameof(PlayerData.defeatedMantisLords)),
        ("RMC_Archives_Exit_Wall", nameof(PlayerData.oneWayArchive)),
        ("RMC_Gardens_Stag_Exit", nameof(PlayerData.openedGardensStagStation)),
        ("RMC_Left_Elevator", nameof(PlayerData.cityLift1)),
        ("RMC_Resting_Grounds_Floor", nameof(PlayerData.openedRestingGrounds02)),
        ("RMC_Glade_Door", nameof(PlayerData.gladeDoorOpened)),
        ("RMC_Sanctum_Glass_Floor", nameof(PlayerData.brokenMageWindow)),
        ("RMC_Bathhouse_Door", nameof(PlayerData.bathHouseOpened)),
        ("RMC_Elegant_Door", nameof(PlayerData.openedMageDoor_v2)),
        ("RMC_Emilitia_Door", nameof(PlayerData.city2_sewerDoor)),
        ("RMC_Catacombs_Wall", nameof(PlayerData.restingGroundsCryptWall)),
        ("RMC_Bathhouse_Wall", nameof(PlayerData.bathHouseWall)),
        ("RMC_Love_Door", nameof(PlayerData.openedLoveDoor)),
        ("RMC_Bretta_Door", nameof(PlayerData.brettaRescued)),
        ("RMC_Jiji_Door", nameof(PlayerData.jijiDoorUnlocked)),
        ("RMC_Sly_Door", nameof(PlayerData.slyRescued)),
        ("RMC_Dirtmouth_Station_Door", nameof(PlayerData.openedTownBuilding)),
        ("RMC_Dirtmouth_Lift", nameof(PlayerData.mineLiftOpened)),
        ("RMC_Divine_Door", nameof(PlayerData.divineInTown)),
        ("RMC_Grimm_Door", nameof(PlayerData.troupeInTown)),
        ("RMC_Waterways_Manhole", nameof(PlayerData.openedWaterwaysManhole)),
        ("RMC_Waterways_Acid", nameof(PlayerData.waterwaysAcidDrained)),
        ("RMC_Infected", nameof(PlayerData.crossroadsInfected)),
    ];

    protected override void ManuallyUpdateTerms()
    {
        // Update stateless waypoint terms
        var pd = PlayerData.instance;

        foreach ((var term, var pdBool) in _pdBoolTerms)
        {
            LocalPM.Set(term, pd.GetBool(pdBool) ? 1 : 0);
        }

        if (LocalPM.lm.GetTerm("RMC_Not_Infected") is Term notInfected)
        {
            LocalPM.Set(notInfected, !pd.GetBool(nameof(PlayerData.crossroadsInfected)) ? 1 : 0);
        }

        if (LocalPM.lm.GetTerm("RMC_Blue_Room_Door") is Term blueRoomDoor)
        {
            LocalPM.Set(
                blueRoomDoor,
                pd.GetBool(nameof(PlayerData.blueRoomDoorUnlocked))
                || (BossSequenceBindingsDisplay.CountCompletedBindings() >= 8)
                    ? 1
                    : 0
            );
        }

        if (LocalPM.lm.GetTerm("RMC_Godhome_Roof") is Term godhomeRoof)
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
                case SN.Crossroads_10:
                    if (pbd.id is "Battle Scene")
                    {
                        LocalPM.Set("RMC_False_Knight_Gate", pbd.activated ? 1 : 0);
                    }

                    break;
                case SN.Fungus2_14:
                    if (pbd.id is "Mantis Lever (1)")
                    {
                        LocalPM.Set("RMC_Mantis_Big_Floor", pbd.activated ? 1 : 0);
                    }

                    break;
                case SN.RestingGrounds_10:
                    if (pbd.id is "Collapser Small (5)")
                    {
                        LocalPM.Set("RMC_Catacombs_Ceiling", pbd.activated ? 1 : 0);
                    }

                    break;
                case SN.Ruins1_31:
                    if (pbd.id is "Breakable Wall Ruin Lift")
                    {
                        LocalPM.Set("RMC_City_Toll_Wall", pbd.activated ? 1 : 0);
                    }

                    if (pbd.id is "Ruins Lever")
                    {
                        LocalPM.Set("RMC_Shade_Soul_Exit", pbd.activated ? 1 : 0);
                    }

                    break;
                case SN.Waterways_02:
                    if (pbd.id is "Quake Floor")
                    {
                        LocalPM.Set("RMC_Flukemarm_Floor", pbd.activated ? 1 : 0);
                    }

                    if (pbd.id is "Quake Floor (1)")
                    {
                        LocalPM.Set("RMC_Waterways_Bench_Floor", pbd.activated ? 1 : 0);
                    }

                    break;
                // case SN.Waterways_04:
                //     if (pbd.id is "Quake Floor")
                //     {
                //         LocalPM.Set("RMC_Waterways_Bench_Floor_1", pbd.activated ? 1 : 0);
                //     }
                //     if (pbd.id is "Quake Floor (1)")
                //     {
                //         LocalPM.Set("RMC_Waterways_Bench_Floor_2", pbd.activated ? 1 : 0);
                //     }
                //     break;
                case SN.Waterways_05:
                    if (pbd.id is "Quake Floor")
                    {
                        LocalPM.Set("RMC_Dung_Defender_Floor", pbd.activated ? 1 : 0);
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
                LocalPM.Set("RMC_City_Toll_Lift_Up", pid.value % 2 is 1 ? 1 : 0);
                LocalPM.Set("RMC_City_Toll_Lift_Down", pid.value % 2 is 0 ? 1 : 0);
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
