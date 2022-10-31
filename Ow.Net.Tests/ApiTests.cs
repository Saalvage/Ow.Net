using System.Net.Http;
using Xunit;

namespace Ow.Net.Tests; 

public class ApiTests {
    [Fact]
    public async void TestImageServers() {
        using var http = new HttpClient();
        var response = await http
            .GetAsync("https://blzgdapipro-a.akamaihd.net/game/unlocks/0x0250000000002A5E.png")
            .ConfigureAwait(false);
        Assert.True(response.IsSuccessStatusCode);
    }
}
