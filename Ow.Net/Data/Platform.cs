using Newtonsoft.Json;

namespace Ow.Net.Data;

[JsonConverter(typeof(PlatformJsonConverter))]
public sealed class Platform : ClassEnum<Platform> {
    public static readonly Platform PC = new("pc", nameof(PC));
    public static readonly Platform XBoxOne = new("xbl", nameof(XBoxOne));
    public static readonly Platform NintendoSwitch = new("nintendo-switch", nameof(NintendoSwitch));
    public static readonly Platform PlayStation4 = new("psn", nameof(PlayStation4));

    private static readonly IReadOnlyDictionary<string, Platform> StringToPlatform = new Dictionary<string, Platform> {
        { "pc", PC },
        { "computer", PC },
        { "personalcomputer", PC },
        { "battlenet", PC },

        { "xbox", XBoxOne },
        { "xboxone", XBoxOne },
        { "x1", XBoxOne },
        { "xb1", XBoxOne },
        { "one", XBoxOne },
        { "xboxlive", XBoxOne },
        { "xbl", XBoxOne },

        { "nintendoswitch", NintendoSwitch },
        { "switch", NintendoSwitch },
        { "nintendonx", NintendoSwitch },
        { "nx", NintendoSwitch },

        { "playstation4", PlayStation4 },
        { "ps4", PlayStation4 },
        { "playstation", PlayStation4 },
        { "ps", PlayStation4 },
        { "playstationnetwork", PlayStation4 },
        { "psn", PlayStation4 },
    };
    private static readonly char[] TrimChars = { '-', '_', ' ' };

    internal string ApiName { get; }
    private readonly string _name;

    private Platform(string apiName, string name) {
        ApiName = apiName;
        _name = name;
    }

    public static Optional<Platform> Parse(string str) {
        ArgumentNullException.ThrowIfNull(str);

        str = str.ToLowerInvariant().Remove(TrimChars);

        if (StringToPlatform.TryGetValue(str, out var platform)) {
            return platform;
        }

        return Optional.Empty;
    }

    public override string ToString() => _name;
}
