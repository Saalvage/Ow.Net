using Xunit;

namespace Ow.Net.Tests;

public class InternalTests {
    [Fact]
    public void TestReplace() {
        Assert.Equal("", "____".Remove(new[] { '_' }));
        Assert.Equal("____", "____".Remove(new char[] { }));
        Assert.Equal("", "".Remove(new[] { '_', ' ' }));
        Assert.Equal("a", "_ a _ __".Remove(new[] { '_', ' ' }));
        Assert.Equal("abcd", "a_ b _c __d".Remove(new[] { '_', ' ' }));
        Assert.Equal("abcd", "  ___ ab _cd___".Remove(new[] { '_', ' ' }));
    }
}
