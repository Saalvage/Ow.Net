using AngleSharp;
using Newtonsoft.Json;
using Ow.Net;
using Ow.Net.Data;
using Serilog;
using System.Diagnostics.CodeAnalysis;

/*
    Findings:
    "widowmaker" xbl dupe
    lots of id dupes
    url dupes
    level 0

    example multi-profile https://playoverwatch.com/en-us/career/pc/DVA-13659/
*/

var heroes = new[] {
    "Ana" , "Ashe", "Baptiste", "Bastion", "Brigitte", "McCree", "Cassidy", "Dva", "D.VA", "Doomfist", "Echo", "Genji",
    "Hanzo", "Junkrat", "Lucio", "Lúcio", "Mei", "Mercy", "Moira", "Orisa", "Pharah", "Reaper", "Reinhardt", "Roadhog",
    "Sigma", "Soldier76", "Soldier:76", "Sombra", "Symmetra", "Torbjorn", "Torbjoern", "Torbjörn", "Tracer",
    "Widowmaker", "Winston", "Wrecking Ball", "Zarya", "Zenyatta",
};

var common = new[] {
    "GhostDragon", "Joe", "DeadlyKitten", "SneakyTurtle", "BnetPlayer", "Bob", "NoMercy", "Potato", "Mommy", "SilentStorm",
    "SneakySquid", "TrickyHunter", "Pogchamp", "Wolf", "GoldenPants", "Hawkeye", "ShadowDragon", "ShadowStorm", "Depression",
    "Lucky", "EpicPants", "MagicTurtle", "Jam", "Jelly", "Panda", "David", "Alex", "Marco", "Root", "James", "Robert",
    "John", "Mike", "Toast", "Shadow", "Dark", "Dan", "Doggo", "Dog",
};

var client = new OverwatchClient();

using var html = BrowsingContext.New(Configuration.Default.WithDefaultLoader());

var rawData = (await Task.WhenAll(heroes.Concat(common).Select(async x => (String: x, Result: await client.SearchProfiles(x)))))
    .SelectMany(x => x.Result.Except());

var test = rawData.Where(x => x.Level == 0);

string json = JsonConvert.SerializeObject(rawData);

File.WriteAllText("test.json", json);


using var enumerator = rawData.OrderBy(_ => Guid.NewGuid()).GetEnumerator();

bool ThreadSafeNext([MaybeNullWhen(false)] out SearchResult res) {
    lock (enumerator) {
        res = enumerator.MoveNext() ? enumerator.Current : null;
    }

    return res != null;
}


int done = 0;
var logger = new LoggerConfiguration().WriteTo.Console().Destructure.With<HtmlDestructuringPolicy>().CreateLogger();

await Task.WhenAll(Enumerable.Range(0, 1000).Select(_ => enumerator.MoveNext() ? enumerator.Current : null)
    .Select(async x => {
        if (x is null) { return; }
        do {
            Console.WriteLine($"Started! {x.ProfileUrl}");
            using var doc = await html.OpenAsync(x.ProfileUrl);
            if (doc.Title is null or "Overwatch" or "") {
                continue;
            }

            // Empty
            var lvl = Extractors.ExtractLevel(doc, logger);
            if (x.Level % 100 != lvl.Except(x.ProfileUrl).Rest) {
                continue;
            }

            // Caching mismatch
            Console.WriteLine($"({x.Level}) {x.ProfileUrl}");
            if (x.Level != lvl.Value.Convert()) {

            }
            Console.WriteLine(Interlocked.Increment(ref done));
            break; // Successful test!
        } while (ThreadSafeNext(out x));
    }));


return;

var groups = rawData
    .GroupBy(x => (x.Level - 1) / 600)
    .Where(x => x.Key < 5)
    .ToArray();

void PrintProfile(SearchResult res, Func<SearchResult, uint> mapper) {
    Console.WriteLine($"{mapper(res)}: {res.ProfileUrl} ({res.Level})");
}

// Stars
/*var stars = (await groups
    .Select(x => x.GroupBy(ToStars))
    .SelectMany(x => x.Select(y => y.First()))
    .Select(async x => {
        using var doc = await html.OpenAsync(x.ProfileUrl);
        return (profile: x, starUrl: Extractors.ExtractRank(doc));
    }).WhenAll())
    .OrderBy(x => x.profile.Level);

foreach (var (profile, url) in stars) {
    PrintProfile(profile, ToStars);
    Console.WriteLine(url);
}

uint ToStars(SearchResult x) => (x.Level - 1) / 100;

Console.WriteLine();

// Frames
var frames = groups
    .Select(x => x.GroupBy(ToFrame))
    .Select(x => x
        .Select(y => y.First())
        .OrderBy(y => (y.Level - 1) % 100));

foreach (var profiles in frames) {
    foreach (var profile in profiles) {
        PrintProfile(profile, ToFrame);
        using var doc = await html.OpenAsync(profile.ProfileUrl);
        //Console.WriteLine(Extractors.ExtractLevel(doc));
    }
    Console.WriteLine("//");
}

uint ToFrame(SearchResult x) => ((x.Level - 1) % 100) / 10;
*/
