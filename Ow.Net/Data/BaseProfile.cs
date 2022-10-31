namespace Ow.Net.Data;

public class BaseProfile {
    public string Name { get; }
    public uint Level { get; }
    public string ProfileUrl { get; }
    public string ProfilePictureUrl { get; }
    public Endorsement Endorsement { get; }
    public bool IsPublic { get; }

    protected BaseProfile(string name, uint level, string profileUrl, string profilePictureUrl, Endorsement endorsement, bool isPublic) {
        Name = name;
        Level = level;
        ProfileUrl = profileUrl;
        ProfilePictureUrl = profilePictureUrl;
        Endorsement = endorsement;
        IsPublic = isPublic;
    }

    internal BaseProfile(string name, uint level, string profileUrl, string profilePictureUrl, Endorsement endorsement)
        : this(name, level, profileUrl, profilePictureUrl, endorsement, false) { }

    public override bool Equals(object? obj)
        => obj switch {
            BaseProfile p => ProfileUrl == p.ProfileUrl,
            _ => false,
        };

    public override int GetHashCode()
        => ProfileUrl.GetHashCode();

    public static bool operator ==(BaseProfile lhs, BaseProfile rhs)
        => lhs.Equals(rhs);

    public static bool operator !=(BaseProfile lhs, BaseProfile rhs)
        => !lhs.Equals(rhs);

    public override string ToString()
        => ProfileUrl;
}
