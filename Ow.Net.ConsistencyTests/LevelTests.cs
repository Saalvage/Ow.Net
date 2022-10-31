using AngleSharp.Dom;
using Microsoft.VisualBasic;
using Ow.Net.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ow.Net.ConsistencyTests;

public class LevelTests : BaseChecker {
    public LevelTests(ITestOutputHelper output) : base(output) { }

    public override async Task<bool> Check(SearchResult res, IDocument doc, OverwatchClient client) {
        // Caching mismatch
        var lvl = Extractors.ExtractLevel(doc, _logger);
        if (res.Level % 100 != lvl.Except(res.ProfileUrl).Rest) {
            return false;
        }

        _output.WriteLine($"({res.Level}) {res.ProfileUrl}");
        var converted = lvl.Value.Convert();
        if (res.Level > 2990) {
            Assert.Equal(Level.FailureReason.TooHigh, converted);
        } else {
            try {
                Assert.Equal(res.Level, converted);
            } catch (EqualException) {
                var profile = await res.Research(client).ConfigureAwait(false);

                if (profile.HasValueAnd(x => x.Level == res.Level)) {
                    throw;
                }

                _output.WriteLine($"Caching issue! {res.ProfileUrl} was {res.Level}, got {converted}, rechecked {profile.Map(x => x.Level)}");
                return false; // Just take another.
            }
        }
        return true; // Successful test!
    }

    [Fact]
    public Task TestRandom() => Test(500);

    [Fact]
    public Task TestHigh() => Test(50, x => x.Level > 2990);

    [Fact]
    public Task TestZero() => Test(5, x => x.Level == 0);

    [Fact]
    public Task TestAll()
        => Enumerable.Range(0, 300)
            .Select(x => x * 10)
            .Select(i => Test(1, x => x.Level > i && x.Level <= i + 10)
                .ContinueWith(_ => _output.WriteLine($"Finished: ({i}, {i + 10}]")))
            .WhenAll();
}
