using Newtonsoft.Json;
using System.Reflection;

namespace Ow.Net; 

public static class Utils {
    internal static bool IsFullyDeserialized<T>(T t)
        => typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .All(x => x.GetCustomAttribute(typeof(JsonPropertyAttribute)) == null || x.GetValue(t) != null);
}
