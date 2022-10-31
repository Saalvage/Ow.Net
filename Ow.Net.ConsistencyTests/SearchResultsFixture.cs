using AngleSharp;
using Ow.Net.Data;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Ow.Net.ConsistencyTests;

public class SearchResultsFixture : IDisposable {
    private static readonly string[] _heroes = {
        "Ana" , "Ashe", "Baptiste", "Bastion", "Brigitte", "McCree", "Cassidy", "Dva", "D.VA", "Doomfist", "Echo", "Genji",
        "Hanzo", "Junkrat", "Lucio", "Lúcio", "Mei", "Mercy", "Moira", "Orisa", "Pharah", "Reaper", "Reinhardt", "Roadhog",
        "Sigma", "Soldier76", "Soldier:76", "Sombra", "Symmetra", "Torbjorn", "Torbjoern", "Torbjörn", "Tracer",
        "Widowmaker", "Winston", "Wrecking Ball", "Zarya", "Zenyatta",
    };

    private static readonly string[] _common = {
        "GhostDragon", "Joe", "DeadlyKitten", "SneakyTurtle", "BnetPlayer", "Bob", "NoMercy", "Potato", "Mommy", "SilentStorm",
        "SneakySquid", "TrickyHunter", "Pogchamp", "Wolf", "GoldenPants", "Hawkeye", "ShadowDragon", "ShadowStorm", "Depression",
        "Lucky", "EpicPants", "MagicTurtle", "Jam", "Jelly", "Panda", "David", "Alex", "Marco", "Root", "James", "Robert",
        "John", "Mike", "Toast", "Shadow", "Dark", "Dan", "Doggo", "Dog",
    };

    public static readonly Task<SearchResultsFixture> Instance = CreateAsync();

    public HttpClient Http { get; } = new();
    public IBrowsingContext Html { get; } = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
    public OverwatchClient Client { get; }
    private readonly SearchResult[] _results;

    private bool _disposed = false;

    private SearchResultsFixture(OverwatchClient client, SearchResult[] results) {
        Client = client;
        _results = results;
    }

    private static async Task<SearchResultsFixture> CreateAsync() {
        var client = new OverwatchClient(new LoggerConfiguration()
            .Destructure.With<HtmlDestructuringPolicy>()
            .AuditTo.Sink(new TestFailSink())
            .CreateLogger());

        return new(client, (await _heroes.Concat(_common)
                .Select(async x => (String: x, Result: await client.SearchProfiles(x).ConfigureAwait(false)))
                .WhenAll().ConfigureAwait(false))
                .SelectMany(x => x.Result.Except())
                .OrderBy(_ => Guid.NewGuid()).ToArray());
    }

    public async Task Assert(int count, Func<SearchResult, bool> predicate, BaseChecker checker, ITestOutputHelper output) {
        using var enumerator = _results.Where(predicate).GetEnumerator();

        bool ThreadSafeNext([MaybeNullWhen(false)] out SearchResult res) {
            lock (enumerator) {
                res = enumerator.MoveNext() ? enumerator.Current : null;
            }

            return res != null;
        }

        var i = 0;

        await Enumerable.Range(0, count).Select(_ => enumerator.MoveNext() ? enumerator.Current : null)
            .Select(async x => {
                if (x is null) {
                    output.WriteLine("None found!");
                    return;
                }
                do {
                    using var doc = await Html.OpenAsync(x.ProfileUrl).ConfigureAwait(false);
                    // Empty
                    if (doc.Title is null or "Overwatch" or "") {
                        continue;
                    }

                    if (await checker.Check(x, doc, Client).ConfigureAwait(false)) {
                        output.WriteLine($"Done: {Interlocked.Increment(ref i)}");
                        break;
                    }
                } while (ThreadSafeNext(out x));
            }).WhenAll().ConfigureAwait(false);
    }

    ~SearchResultsFixture() {
        Dispose();
    }

    public void Dispose()
    {
        if (!_disposed) {
            Http.Dispose();
            Html.Dispose();
            Client.Dispose();
        }
        _disposed = true;
    }
}
