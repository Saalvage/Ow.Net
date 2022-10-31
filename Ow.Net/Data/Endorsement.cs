using System.Collections;
using System.Drawing;

namespace Ow.Net.Data; 

public sealed class Endorsement : IReadOnlyDictionary<Endorsement.Type, decimal> {
    public sealed class Type : ClassEnum<Type> {
        public static readonly Type ShotCaller = new(Color.FromArgb(0xf1, 0x95, 0x12), "Shot Caller");
        public static readonly Type GoodTeammate = new(Color.FromArgb(0xc8, 0x1a, 0xf5), "Good Teammate");
        public static readonly Type Sportsmanship = new(Color.FromArgb(0x40, 0xce, 0x44), "Sportsmanship");

        public Color Color { get; }

        private readonly string _name;

        private Type(Color color, string name) {
            Color = color;
            _name = name;
        }

        public override string ToString()
            => _name;
    }

    public byte Level { get; }

    private readonly decimal _shotCallerPercentage;
    private readonly decimal _goodTeammatePercentage;
    private readonly decimal _sportsmanShipPercentage;

    public string BackgroundImageUrl => $"https://static.playoverwatch.com/svg/icons/endorsement-frames-3c9292c49d.svg#_{Level}";

    internal Endorsement(byte level, decimal shotCallerPercentage, decimal goodTeammatePercentage,
        decimal sportsmanShipPercentage) {
        Level = level;
        _shotCallerPercentage = shotCallerPercentage;
        _goodTeammatePercentage = goodTeammatePercentage;
        _sportsmanShipPercentage = sportsmanShipPercentage;
    }

    public bool ContainsKey(Type key)
        => key is not null;

    public bool TryGetValue(Type key, out decimal value) {
        value = key is not null ? this[key] : default;
        return key is not null;
    }

    public decimal this[Type type] {
        get => type == Type.ShotCaller ? _shotCallerPercentage
             : type == Type.GoodTeammate ? _goodTeammatePercentage
             : type == Type.Sportsmanship ? _sportsmanShipPercentage
             : throw new ArgumentOutOfRangeException(nameof(type), $"\"{type}\" is not a valid endorsement type.");
    }

    public IEnumerable<Type> Keys => Type.Values;

    public IEnumerable<decimal> Values {
        get {
            yield return _shotCallerPercentage;
            yield return _goodTeammatePercentage;
            yield return _sportsmanShipPercentage;
        }
    }

    public IEnumerator<KeyValuePair<Type, decimal>> GetEnumerator() {
        yield return KeyValuePair.Create(Type.ShotCaller, _shotCallerPercentage);
        yield return KeyValuePair.Create(Type.GoodTeammate, _goodTeammatePercentage);
        yield return KeyValuePair.Create(Type.Sportsmanship, _sportsmanShipPercentage);
    }

    public override bool Equals(object? obj)
        => obj switch {
            Endorsement e => e.Level == Level
                             && e._shotCallerPercentage == _shotCallerPercentage
                             && e._goodTeammatePercentage == _goodTeammatePercentage
                             && e._sportsmanShipPercentage == _sportsmanShipPercentage,
            _ => false,
        };

    public override int GetHashCode()
        => HashCode.Combine(Level, _shotCallerPercentage, _goodTeammatePercentage, _sportsmanShipPercentage);

    public static bool operator ==(Endorsement lhs, Endorsement rhs)
        => lhs.Equals(rhs);

    public static bool operator !=(Endorsement lhs, Endorsement rhs)
        => !lhs.Equals(rhs);

    public override string ToString()
        => $"{Level} (sc: {_shotCallerPercentage}, gtm: {_goodTeammatePercentage}, ss: {_sportsmanShipPercentage})";

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public int Count => 3;
}
