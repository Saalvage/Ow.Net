namespace Ow.Net;

internal static class Extensions {
    internal static string Remove(this string str, char[] removeChars)
        => string.Concat(str.Where(c => !removeChars.Contains(c)));

    internal static IEnumerable<(T value, uint index)> WithIndex<T>(this IEnumerable<T> enumerable)
        => enumerable.Select((x, i) => (x, (uint)i));
}
