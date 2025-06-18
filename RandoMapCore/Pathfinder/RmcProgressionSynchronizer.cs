using RandomizerCore;
using RandomizerCore.Logic;
using RandomizerCore.Logic.StateLogic;
using RCPathfinder.Logic;
using SN = ItemChanger.SceneNames;

namespace RandoMapCore.Pathfinder;

internal class RmcProgressionSynchronizer(RmcLogicExtender logicExtender, RandoContext ctx, bool sequenceBreak)
    : ProgressionSynchronizer(logicExtender, ctx)
{
    public override ProgressionManager ReferencePM =>
        sequenceBreak ? RandoMapCoreMod.Data.PM : RandoMapCoreMod.Data.PMNoSequenceBreak;

    protected override void ManuallyUpdateTerms()
    {
        try
        {
            // Update stateless waypoint terms
            var pd = PlayerData.instance;

            foreach (var kvp in logicExtender.PdBoolWaypoints)
            {
                LocalPM.Set(kvp.Key, pd.GetBool(kvp.Value) ? 1 : 0);
            }

            foreach (var pbd in SceneData.instance.persistentBoolItems)
            {
                if (logicExtender.SceneBoolWaypoints.TryGetValue((pbd.sceneName, pbd.id), out var term))
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
        }
        catch (Exception e)
        {
            RandoMapCoreMod.Instance.LogError($"Failed to update terms in ProgressionSynchronizer:\n{e}");
        }
    }
}
