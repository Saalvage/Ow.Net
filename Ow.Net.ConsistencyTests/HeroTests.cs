using AngleSharp;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Ow.Net.Data;
using System.Drawing;
using System.Net.Http;
using CssColor = AngleSharp.Css.Values.Color;

namespace Ow.Net.ConsistencyTests; 

public class HeroTests {
    [Fact]
    public void TestHeroExistence() {
        Assert.NotEmpty(Hero.Values);
        Assert.NotEmpty(Role.Values);
    }

    private static readonly Regex CLASS = new("ProgressBar-bar ow-(?<name>.+)-color");

    [Fact]
    public async Task TestCareerProfile() {
        using var ctx = new BrowsingContext(Configuration.Default.WithDefaultLoader(
            new() {
                IsResourceLoadingEnabled = true,
            }).WithCss());
        using var doc = await ctx.OpenAsync("https://playoverwatch.com/en-us/career/pc/Salvage-11179/").ConfigureAwait(false);
        var category = doc.GetElementsByClassName("progress-category").FirstOrDefault();
        Assert.NotNull(category);
        const int HERO_COUNT = 30;
        Assert.Equal(HERO_COUNT, category.ChildElementCount);
        var test = category.Children
            .Select(x => {
                Assert.Equal(2, x.ChildElementCount);
                var thumb = x.FirstElementChild;
                Assert.NotNull(thumb);
                var thumbUrl = thumb.Attributes["src"]?.Value;
                Assert.NotNull(thumbUrl);

                var data = x.Children.ElementAtOrDefault(1)?.FirstElementChild;
                Assert.NotNull(data);
                var name = data.GetAttribute("data-hero");
                Assert.NotNull(name);

                Assert.NotNull(data.ClassName);
                var match = CLASS.Match(data.ClassName);
                Assert.True(match.Success);
                var urlName = match.Groups["name"].Value;

                var colorRaw = doc.StyleSheets.GetRules<ICssStyleRule>()
                    .FirstOrDefault(x => x.Selector?.Text == $".ow-{urlName}-color")
                    ?.Style.GetProperty("background-color");
                Assert.NotNull(colorRaw);

                var color = (CssColor)colorRaw.RawValue;
                return (name, urlName, thumbUrl, color);
            })
            .OrderBy(x => x.name)
            .Zip(Hero.Values);

        foreach (var ((name, urlName, thumbUrl, color), hero) in test) {
            Assert.Equal(name, hero.Name);
            Assert.Equal($"https://playoverwatch.com/en-us/heroes/{urlName}/", hero.InfoUrl);
            Assert.Equal(thumbUrl, hero.IconUrl);
            Assert.Equal(Color.FromArgb(color.R, color.G, color.B), hero.Color);
        }
    }

    [Fact]
    public async Task TestInfoUrls() {
        var ctx = (await SearchResultsFixture.Instance.ConfigureAwait(false)).Html;
        var sites = await Hero.Values
            .Select(async x => (hero: x, doc: await ctx.OpenAsync(x.InfoUrl).ConfigureAwait(false)))
            .WhenAll().ConfigureAwait(false);

        foreach (var (hero, doc) in sites) {
            Assert.True(doc.StatusCode.IsSuccess());
        }

        var mapped = sites.Select(x => (
                x.hero,
                oName: x.doc.GetElementsByClassName("hero-name").FirstOrDefault()?.TextContent,
                oRole: x.doc.GetElementsByClassName("hero-detail-role-name").FirstOrDefault()?.TextContent));

        foreach (var (hero, oName, oRole) in mapped) {
            Assert.Equal(hero.ToString(), oName);
            Assert.Equal(hero.Role.ToString(), oRole);
        }
    }

    [Fact]
    public async Task TestPictures() {
        var client = (await SearchResultsFixture.Instance.ConfigureAwait(false)).Http;

        async Task<(string Url, HttpResponseMessage Res)> Load(string url)
            => (url, await client.GetAsync(url).ConfigureAwait(false));

        var results = await Hero.Values
            .SelectMany(x => new[] {Load(x.BigPictureUrl), Load(x.HeroSelectPictureUrl)})
            .WhenAll().ConfigureAwait(false);

        foreach (var (url, result) in results) {
            Assert.True(result.IsSuccessStatusCode, url);
        }
    }
}
