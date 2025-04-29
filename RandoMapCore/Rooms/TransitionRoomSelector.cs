using MapChanger;
using MapChanger.MonoBehaviours;
using RandoMapCore.Input;
using RandoMapCore.Localization;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;
using RandoMapCore.Pins;
using RandoMapCore.Transition;
using RandoMapCore.UI;

namespace RandoMapCore.Rooms;

internal class TransitionRoomSelector : RoomSelector
{
    internal static TransitionRoomSelector Instance { get; private set; }

    public override void Initialize(IEnumerable<ISelectable> rooms)
    {
        base.Initialize(rooms);

        Instance = this;
    }

    private protected override bool ActiveByCurrentMode()
    {
        return Conditions.TransitionRandoModeEnabled();
    }

    private protected override bool ActiveByToggle()
    {
        return RandoMapCoreMod.GS.RoomSelectionOn;
    }

    protected override void OnSelectionChanged()
    {
        RoomSelectionPanel.Instance.Update();
    }

    internal string GetText()
    {
        var instructions = GetInstructionText();
        var transitions = new TransitionStringDef(SelectedObject.Key).GetFullText();

        if (transitions is "")
        {
            return instructions;
        }

        return $"{instructions}\n\n{transitions}";
    }

    private string GetInstructionText()
    {
        var selectedScene = SelectedObject.Key;
        var text = "";

        text += $"{"Selected room".L()}: {selectedScene.LC()}.";

        if (selectedScene == Utils.CurrentScene())
        {
            text += $" {"You are here".L()}.";
        }

        if (RandoMapCoreMod.Data.EnablePathfinder)
        {
            var selectBindingText = SelectRoomRouteInput.Instance.GetBindingsText();
            text += $"\n\n{"Press".L()} {selectBindingText}";

            if (RmcPathfinder.RM.CanCycleRoute(selectedScene))
            {
                text += $" {"to change starting / final transitions of current route".L()}.";
            }
            else
            {
                text += $" {"to find a new route".L()}.";
            }

            if (
                RandoMapCoreMod.Data.EnableMapBenchwarp
                && (!RandoMapCoreMod.Data.EnablePinSelection || PinSelector.Instance.VisitedBenchNotSelected())
                && BenchwarpInput.TryGetBenchwarpFromRoute(out var _)
            )
            {
                var benchBindingText = BenchwarpInput.Instance.GetBindingsText();
                text += $" {"Hold".L()} {benchBindingText} {"to benchwarp".L()}.";
            }
        }

        return text;
    }
}
