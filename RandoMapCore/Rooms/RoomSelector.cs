using MagicUI.Elements;
using MapChanger;
using MapChanger.MonoBehaviours;
using RandoMapCore.Input;
using RandoMapCore.Modes;
using RandoMapCore.Pathfinder;
using RandoMapCore.Pins;
using RandoMapCore.Transition;
using RandoMapCore.UI;

namespace RandoMapCore.Rooms;

internal class RoomSelector : Selector
{
    public override float SelectionRadius { get; } = 2.5f;
    public override float SpriteSize { get; } = 0.6f;

    public override void Initialize(IEnumerable<ISelectable> rooms)
    {
        base.Initialize(rooms);
        ActiveModifiers.AddRange([ActiveByCurrentMode, ActiveByToggle]);
        SpriteObject.SetActive(true);
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

    internal RunCollection GetSelectionText()
    {
        if (SelectedObject?.Key is not string key)
        {
            return null;
        }

        return RunCollection.Join(
            "\n\n",
            new List<RunCollection>() { GetInstructionText(key), new TransitionStringDef(key).GetFullText() }
        );
    }

    private bool ActiveByCurrentMode()
    {
        return Conditions.TransitionRandoModeEnabled();
    }

    private bool ActiveByToggle()
    {
        return RandoMapCoreMod.GS.RoomSelectionOn;
    }

    private RunCollection GetInstructionText(string scene)
    {
        var roomColor = RmcPathfinder.Slt.GetRoomColor(scene);

        RunCollection text =
        [
            new(scene.LC())
            {
                Color = roomColor with { w = 1f },
                Bold = true,
                Size = (int)(14f * MapChangerMod.GS.UIScale * 1.2f),
            },
        ];

        if (scene == Utils.CurrentScene())
        {
            text.Add(new($" - {"You are here".L()}"));
        }

        if (RandoMapCoreMod.Data.EnablePathfinder)
        {
            text.Add(
                new(
                    $"\n\n{"Press".L()} {SelectRoomRouteInput
                        .Instance
                        .GetBindingsText()} {(RmcPathfinder.RM.CanCycleRoute(scene)
                            ? "to change starting / final transitions of current route"
                            : "to find a new route").L()}."
                )
            );

            if (
                RandoMapCoreMod.Data.EnableMapBenchwarp
                && (
                    !RandoMapCoreMod.Data.EnablePinSelection
                    || (RmcPinManager.Selector?.VisitedBenchNotSelected() ?? false)
                )
                && BenchwarpInput.TryGetBenchwarpFromRoute(out var _)
            )
            {
                text.Add(new($" {"Hold".L()} {BenchwarpInput.Instance.GetBindingsText()} {"to benchwarp".L()}."));
            }
        }

        return text;
    }
}
