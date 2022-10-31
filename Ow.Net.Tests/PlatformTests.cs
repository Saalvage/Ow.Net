using Newtonsoft.Json;
using Ow.Net.Data;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Ow.Net.Tests;

public class PlatformTests {
    private static readonly Platform[] Platforms = { Platform.XBoxOne, Platform.NintendoSwitch, Platform.PC, Platform.PlayStation4 };

    [Fact]
    public void TestCompleteness() {
        var fields = typeof(Platform)
            .GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.GetValue(null))
            .OfType<Platform>().ToHashSet();

        Assert.Equal(Platforms.ToHashSet(), fields);
    }

    [Fact]
    public void TestInternalToString() {
        Assert.Equal("pc", Platform.PC.ApiName);
        Assert.Equal("xbl", Platform.XBoxOne.ApiName);
        Assert.Equal("nintendo-switch", Platform.NintendoSwitch.ApiName);
        Assert.Equal("psn", Platform.PlayStation4.ApiName);
    }

    [Fact]
    public void TestAllToString() {
        foreach (var p in Platforms) {
            Assert.NotNull(p);
            Assert.False(string.IsNullOrWhiteSpace(p.ToString()));
        }
    }

    [Fact]
    public void TestFromString() {
        Assert.Throws<ArgumentNullException>(() => {
            Platform.Parse(null!);
        });

        Assert.True(Platform.Parse("Nintendo Game Cube").IsEmpty);
        Assert.False(Platform.Parse("SEGA Genesis").HasValue);

        var opt = Platform.Parse("ps4");
        Assert.True(opt.HasValue);
        Assert.Equal(Platform.PlayStation4, opt.Value);
        Assert.Equal(Platform.XBoxOne, Platform.Parse("XBox One").Value);
        Assert.Equal(Platform.NintendoSwitch, Platform.Parse(" Switch "));
        Assert.Equal(Platform.PC, Platform.Parse("Personal_Computer"));
    }

    [Fact]
    public void TestJson() {
        foreach (var p in Platforms) {
            Assert.Equal(p, JsonConvert.DeserializeObject<Platform>($"\"{p}\""));
            Assert.Equal(p, JsonConvert.DeserializeObject<Platform>(JsonConvert.SerializeObject(p)));
        }

        Assert.Null(JsonConvert.DeserializeObject<Platform>("\"Raspberry PI\""));
    }
}
