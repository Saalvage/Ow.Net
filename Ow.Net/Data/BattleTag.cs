using System.Text.RegularExpressions;

namespace Ow.Net.Data;

public sealed class BattleTag {
    // Allowed characters vary by region and are quite complex, which is why the client side checking only takes into account length.
    // 7 digit tags exist! https://playoverwatch.com/en-us/career/pc/deadlykitten-2175595/
    private static readonly Regex Regex = new(@"^(?<name>\D.{2,11})[#\-_ ](?<id>\d{4,7})$");

    internal string ApiName { get; }
    private string Name { get; }

    public BattleTag(string name, uint id) {
        ArgumentNullException.ThrowIfNull(name);

        if (name.Length < 3) {
            throw new ArgumentOutOfRangeException(nameof(name), name,
                "BattleTag names must be at least 3 characters.");
        }

        if (name.Length > 12) {
            throw new ArgumentOutOfRangeException(nameof(name), name,
                "BattleTag names must be at most 12 characters.");
        }

        if (id < 1000) {
            throw new ArgumentOutOfRangeException(nameof(id), id,
                "BattleTag ids must be at least 4 digits.");
        }

        if (id >= 10_000_000) {
            throw new ArgumentOutOfRangeException(nameof(id), id,
                "BattleTag ids must be at most 7 digits.");
        }

        ApiName = $"{name}-{id}";
        Name = $"{name}#{id}";
    }

    public static Optional<BattleTag> Parse(string str) {
        var match = Regex.Match(str);
        if (!match.Success) { return Optional.Empty; }


        return new BattleTag(match.Groups["name"].Value,
            uint.Parse(match.Groups["id"].ValueSpan));
    }

    public override string ToString() => Name;
}
