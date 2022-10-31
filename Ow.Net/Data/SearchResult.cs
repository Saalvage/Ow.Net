using Newtonsoft.Json;

namespace Ow.Net.Data;

public sealed class SearchResult {
    [JsonProperty]
    public string Name { get; private init; }

    [JsonProperty]
    // Not unique!!!
    private string UrlName { get; init; }
    [JsonIgnore]
    public string ProfileUrl => $"https://playoverwatch.com/en-us/career/{Platform.ApiName}/{UrlName}/";

    [JsonProperty]
    // THIS IS ONLY UNIQUE PER PLATFORM!!!
    // https://playoverwatch.com/en-us/career/pc/Bastion-11735/
    // https://playoverwatch.com/en-us/career/nintendo-switch/Bastion-ddb5b70a869c1610bfb3b00f7a2d3788/
    public uint Id { get; private init; }

    [JsonProperty]
    public uint Level { get; private init; }

    [JsonProperty]
    public bool IsPublic { get; private init; }

    [JsonProperty]
    public Platform Platform { get; private init; }

    [JsonProperty]
    private string Portrait { get; init; }
    [JsonIgnore]
    public string PortraitUrl => $"https://blzgdapipro-a.akamaihd.net/game/unlocks/{Portrait}.png";

    private SearchResult() { }

    public override bool Equals(object? obj)
        => obj is SearchResult sr && Id == sr.Id && Platform == sr.Platform;

    public override int GetHashCode() => HashCode.Combine(Platform, Id);

    public static bool operator ==(SearchResult lhs, SearchResult rhs)
        => lhs.Equals(rhs);

    public static bool operator !=(SearchResult lhs, SearchResult rhs)
        => !lhs.Equals(rhs);

    public override string ToString() => Name;
}
