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

internal class RoomSelector : Selector
{
    // internal static RoomSelector Instance { get; private set; }

    public override float SelectionRadius { get; } = 2.5f;
    public override float SpriteSize { get; } = 0.6f;

    public override void Initialize(IEnumerable<ISelectable> rooms)
    {
        base.Initialize(rooms);
        ActiveModifiers.AddRange([ActiveByCurrentMode, ActiveByToggle]);
        SpriteObject.SetActive(true);
        // Instance = this;
    }

    protected override void Select(ISelectable selectable)
    {
        selectable.Selected = true;
    }

    protected override void Deselect(ISelectable selectable)
    {
        selectable.Selected = false;
    }

    protected override void OnSelectionChanged()
    {
        RmcUIManager.WorldMap?.Update();
    }

    internal string GetSelectionText()
    {
        if (SelectedObject?.Key is not string key)
        {
            return null;
        }

        var instructions = GetInstructionText();
        var transitions = new TransitionStringDef(key).GetFullText();

        if (transitions is "")
        {
            return instructions;
        }

        return $"{instructions}\n\n{transitions}";
    }

    private bool ActiveByCurrentMode()
    {
        return Conditions.TransitionRandoModeEnabled();
    }

    private bool ActiveByToggle()
    {
        return RandoMapCoreMod.GS.RoomSelectionOn;
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
                && (
                    !RandoMapCoreMod.Data.EnablePinSelection
                    || (RmcPinManager.Selector?.VisitedBenchNotSelected() ?? false)
                )
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
