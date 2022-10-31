using Xunit;

namespace Ow.Net.Tests; 

public class LevelTests {
    private const uint HIGHEST_CONVERTIBLE_LEVEL = 2990;

    [Fact]
    public void TestErrors() {
        var valid = Level.From(1337);

        Assert.True(new Level("The Gloop", "Goes Off", 128).Convert().IsErr);
        Assert.Equal(Level.FailureReason.HighRest, (valid with { Rest = 255 }).Convert());
        Assert.Equal(Level.FailureReason.NonExistentFrame, (valid with { FrameUrl = "https://google.com" }).Convert());
        Assert.Equal(Level.FailureReason.NonExistentStar, (valid with { StarUrl = "https://google.com" }).Convert());
        for (var i = HIGHEST_CONVERTIBLE_LEVEL + 1; i < 6666; i++) {
            Assert.Equal(Level.FailureReason.TooHigh, Level.From(i).Convert());
        }
    }

    [Fact]
    public void TestConversions() {
        for (uint i = 0; i <= HIGHEST_CONVERTIBLE_LEVEL; i++) {
            Assert.Equal(i, Level.From(i).Convert());
        }
    }
}
