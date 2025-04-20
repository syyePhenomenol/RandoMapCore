using MapChanger.MonoBehaviours;

namespace RandoMapCore.Pins;

internal interface IPinSelectable : ISelectable
{
    string GetText();
}
