using System.Collections.Immutable;

namespace Ow.Net;

public class ClassEnum<T> where T : ClassEnum<T> {
    private static readonly Lazy<ImmutableArray<T>> _values = new(Enumerate);
    public static IReadOnlyCollection<T> Values => _values.Value;

    private static ImmutableArray<T> Enumerate()
        => typeof(T).GetFields()
            .Where(x => x.IsPublic && x.IsStatic && x.FieldType == typeof(T))
            .Select(x => x.GetValue(null)).Cast<T>().ToImmutableArray();
}
