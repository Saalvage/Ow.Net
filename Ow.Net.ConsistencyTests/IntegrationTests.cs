using Ow.Net.Data;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Ow.Net.ConsistencyTests; 

public class IntegrationTests {
    [Fact]
    public async Task TestSanity() {
        var client = (await SearchResultsFixture.Instance.ConfigureAwait(false)).Client;
        var profile = (await client.GetProfileAsync(Platform.PC, new("Salvage", 11179)).ConfigureAwait(false))
            .MapErr(x => new Exception($"{x}")).Except();
        //Assert.True(profile.IsPublic);
        Assert.True(profile.Endorsement.Level > 0);
        Assert.Equal(1m,
            profile.Endorsement[Endorsement.Type.ShotCaller]
                + profile.Endorsement[Endorsement.Type.GoodTeammate]
                + profile.Endorsement[Endorsement.Type.Sportsmanship]);
    }
}
