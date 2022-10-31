using Ow.Net.Data;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Ow.Net.ConsistencyTests; 

public static class Extensions {
    public static void WriteLine(this ITestOutputHelper output) => output.WriteLine("");
    public static void WriteLine(this ITestOutputHelper output, object o) => output.WriteLine(o.ToString());
    public static Task WhenAll(this IEnumerable<Task> enumerable) => Task.WhenAll(enumerable);
    public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> enumerable) => Task.WhenAll(enumerable);

    public static async Task<Optional<SearchResult>> Research(this SearchResult res, OverwatchClient client)
        => (await client.SearchProfiles(res.Name).ConfigureAwait(false))
            .Except($"Research failed {res.ProfileUrl}")
            .FirstOrDefault(x => x.ProfileUrl == res.ProfileUrl);

    public static bool IsSuccess(this HttpStatusCode code)
        => (int)code is >= 200 and < 300;
}
