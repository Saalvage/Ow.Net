using System.Drawing;

namespace Ow.Net.Data; 

public sealed class Hero : ClassEnum<Hero> {
    public static readonly Hero Ana = new(nameof(Ana), Role.Support, "13B", Color.FromArgb(0x9c, 0x97, 0x8a));
    public static readonly Hero Ashe = new(nameof(Ashe), Role.Damage, "200", Color.FromArgb(0xb3, 0xa0, 0x5f));
    public static readonly Hero Baptiste = new(nameof(Baptiste), Role.Support, "221", Color.FromArgb(0x28, 0x92, 0xa8));
    public static readonly Hero Bastion = new(nameof(Bastion), Role.Damage, "015", Color.FromArgb(0x24, 0xf9, 0xf8));
    public static readonly Hero Brigitte = new(nameof(Brigitte), Role.Support, "195", Color.FromArgb(0xef, 0xb0, 0x16));
    public static readonly Hero Cassidy = new(nameof(Cassidy), Role.Damage, "042", Color.FromArgb(0xc2, 0x3f, 0x46));
    public static readonly Hero DVa = new("D.Va", "dva", Role.Tank, "07A", Color.FromArgb(0xee, 0x4b, 0xb5));
    public static readonly Hero Doomfist = new(nameof(Doomfist), Role.Damage, "12F", Color.FromArgb(0x76, 0x2c, 0x21));
    public static readonly Hero Genji = new(nameof(Genji), Role.Damage, "029", Color.FromArgb(0xab, 0xe5, 0x0b));
    public static readonly Hero Hanzo = new(nameof(Hanzo), Role.Damage, "005", Color.FromArgb(0x83, 0x7c, 0x46));
    public static readonly Hero Junkrat = new(nameof(Junkrat), Role.Damage, "065", Color.FromArgb(0xfb, 0xd7, 0x3a));
    public static readonly Hero Lúcio = new(nameof(Lúcio), "lucio", Role.Support, "079", Color.FromArgb(0xaa, 0xf5, 0x31));
    public static readonly Hero Mei = new(nameof(Mei), Role.Damage, "0DD", Color.FromArgb(0x87, 0xd7, 0xf6));
    public static readonly Hero Mercy = new(nameof(Mercy), Role.Support, "004", Color.FromArgb(0xfc, 0xd8, 0x49));
    public static readonly Hero Moira = new(nameof(Moira), Role.Support, "1A2", Color.FromArgb(0x71, 0x12, 0xf4));
    public static readonly Hero Orisa = new(nameof(Orisa), Role.Tank, "13E", Color.FromArgb(0xcc, 0xb3, 0x70));
    public static readonly Hero Pharah = new(nameof(Pharah), Role.Damage, "008", Color.FromArgb(0x34, 0x61, 0xa4));
    public static readonly Hero Reaper = new(nameof(Reaper), Role.Damage, "002", Color.FromArgb(0x33, 0x33, 0x33));
    public static readonly Hero Reinhardt = new(nameof(Reinhardt), Role.Tank, "007", Color.FromArgb(0xb9, 0xb5, 0xad));
    public static readonly Hero Roadhog = new(nameof(Roadhog), Role.Tank, "040", Color.FromArgb(0x54, 0x51, 0x5a));
    public static readonly Hero Soldier76 = new("Soldier: 76", "soldier-76", Role.Damage, "06E", Color.FromArgb(0x52, 0x5d, 0x9b));
    public static readonly Hero Sombra = new(nameof(Sombra), Role.Damage, "12E", Color.FromArgb(0x97, 0x62, 0xec));
    public static readonly Hero Symmetra = new(nameof(Symmetra), Role.Damage, "016", Color.FromArgb(0x3e, 0x90, 0xb5));
    public static readonly Hero Torbjörn = new(nameof(Torbjörn), "torbjorn", Role.Damage, "006", Color.FromArgb(0xb0, 0x4a, 0x33));
    public static readonly Hero Tracer = new(nameof(Tracer), Role.Damage, "003", Color.FromArgb(0xff, 0xcf, 0x35));
    public static readonly Hero Widowmaker = new(nameof(Widowmaker), Role.Damage, "00A", Color.FromArgb(0xaf, 0x5e, 0x9e));
    public static readonly Hero Winston = new(nameof(Winston), Role.Tank, "009", Color.FromArgb(0x59, 0x59, 0x59));
    public static readonly Hero WreckingBall = new("Wrecking Ball", "wrecking-ball", Role.Tank, "1CA", Color.FromArgb(0x4a, 0x57, 0x5f));
    public static readonly Hero Zarya = new(nameof(Zarya), Role.Tank, "068", Color.FromArgb(0xff, 0x73, 0xc1));
    public static readonly Hero Zenyatta = new(nameof(Zenyatta), Role.Support, "020", Color.FromArgb(0xe1, 0xc9, 0x31));

    public static Hero Lucio => Lúcio;
    public static Hero Torbjorn => Torbjörn;

    public string Name { get; }
    public Role Role { get; }
    public Color Color { get; }
    
    public string InfoUrl { get; }
    public string IconUrl { get; }
    public string BigPictureUrl { get; }
    public string HeroSelectPictureUrl { get; }

    internal Hero(string name, Role role, string iconUrlSuffix, Color color)
        : this(name, name.ToLower(), role, iconUrlSuffix, color) { }

    internal Hero(string name, string urlName, Role role, string iconUrlSuffix, Color color) {
        Name = name;
        InfoUrl = $"https://playoverwatch.com/en-us/heroes/{urlName}/";
        IconUrl = $"https://d1u1mce87gyfbn.cloudfront.net/game/heroes/small/0x02E0000000000{iconUrlSuffix}.png";
        BigPictureUrl = $"https://d1u1mce87gyfbn.cloudfront.net/hero/{urlName}/career-portrait.png";
        HeroSelectPictureUrl = $"https://d1u1mce87gyfbn.cloudfront.net/hero/{urlName}/hero-select-portrait.png";
        Role = role;
        Color = color;
    }

    public override string ToString() => Name;
}
