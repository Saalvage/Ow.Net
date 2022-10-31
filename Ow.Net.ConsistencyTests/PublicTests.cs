using AngleSharp.Dom;
using Ow.Net.Data;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Ow.Net.ConsistencyTests; 

public class PublicTests : BaseChecker {
    public PublicTests(ITestOutputHelper output) : base(output) { }

    public override async Task<bool> Check(SearchResult res, IDocument doc, OverwatchClient client) {
        var isPublic = Extractors.ExtractPublic(doc, _logger).Except(res.ProfileUrl);
        _output.WriteLine($"({res.IsPublic}) {res.ProfileUrl}");
        try {
            Assert.Equal(res.IsPublic, isPublic);
        } catch (EqualException) {
            var profile = await res.Research(client).ConfigureAwait(false);

            if (profile.HasValueAnd(x => x.IsPublic == res.IsPublic)) {
                throw;
            }

            // There was a caching issue, but it matches now!
        }

        return true;
    }

    [Fact]
    public Task TestRandom() => Test(100);

    [Fact]
    public Task TestPublic() => Test(25, x => x.IsPublic);

    [Fact]
    public Task TestPrivate() => Test(25, x => !x.IsPublic);
}
