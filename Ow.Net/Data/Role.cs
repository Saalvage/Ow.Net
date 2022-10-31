namespace Ow.Net.Data; 

public sealed class Role : ClassEnum<Role> {
    public static readonly Role Tank = new(nameof(Tank));
    public static readonly Role Damage = new(nameof(Damage));
    public static readonly Role Support = new(nameof(Support));

    private readonly string _name;

    private Role(string name) {
        _name = name;
    }

    public override string ToString() => _name;
}
