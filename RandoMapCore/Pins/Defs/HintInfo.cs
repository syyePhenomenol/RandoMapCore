using MagicUI.Elements;
using MapChanger;
using RandoMapCore.Input;
using RandoMapCore.UI;
using RandomizerCore.Logic;

namespace RandoMapCore.Pins;

public class HintInfo
{
    private readonly ProgressionManager _pm;
    private readonly List<LogicDef> _logicDefs = [];

    private string _text;

    internal HintInfo(IEnumerable<RawLogicDef> rawLogicDefs, ProgressionManager pm)
    {
        _pm = pm;

        foreach (var def in rawLogicDefs)
        {
            try
            {
                _logicDefs.Add(pm.lm.CreateDNFLogicDef(def));
            }
            catch
            {
                RandoMapCoreMod.Instance.LogWarn($"Failed to make HintDef: {def}");
            }
        }

        Update();
    }

    internal void Update()
    {
        _text = string.Join("\n", _logicDefs.Where(ld => ld.CanGet(_pm)).Select(ld => ld.Name.L()));
    }

    internal RunCollection GetHintText()
    {
        if (_text == string.Empty)
        {
            return null;
        }

        if (PinSelectionPanel.Instance?.ShowHint ?? false)
        {
            return [new Run(_text)];
        }

        var bindingsText = LocationHintInput.Instance.GetBindingsText();
        return [
            new Run($"{"Press".L()} "),
            new Run(bindingsText),
            new Run($" {"to reveal location hint".L()}."),
        ];
    }
}
