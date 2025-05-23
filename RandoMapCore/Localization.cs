using System.Text.RegularExpressions;
using MapChanger;

namespace RandoMapCore;

public static class Localization
{
    /// <summary>
    /// Localize and clean text in transition format A[B]
    /// </summary>
    /// <param name="name"></param>
    public static string LT(this string name)
    {
        var match = Regex.Match(name, @"^(\w+)\[(\w+)\]$");

        if (match.Groups.Count == 3)
        {
            return $"{match.Groups[1].Value.LC()}[{match.Groups[2].Value.LC()}]";
        }

        return name.LC();
    }
}
