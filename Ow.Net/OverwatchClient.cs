using AngleSharp;
using Newtonsoft.Json;
using Ow.Net.Data;
using Serilog;
using System.Net;
using System.Web;

namespace Ow.Net;

public sealed class ErrorStatusCodeException : Exception {
    public HttpStatusCode Code { get; }
    public string? Reason { get; }

    internal ErrorStatusCodeException(HttpStatusCode code, string? reason)
        : base($"({(int)code}) {reason}") {
        Code = code;
        Reason = reason;
    }
}

public sealed class OverwatchClient : IDisposable {
    private readonly ILogger _logger;
    private readonly IBrowsingContext _html = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
    private readonly HttpClient _http = new();
    private readonly JsonSerializerSettings _defaultJsonSettings = new() {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
    };

    private bool _disposed = false;

    public async Task<Result<List<SearchResult>, Exception>> SearchProfiles(string term, uint retries = 5) {
        ArgumentNullException.ThrowIfNull(term);

        if (_disposed) { throw new ObjectDisposedException(nameof(OverwatchClient)); }

        _logger.Verbose("Searching for {term}.", term);

        Result<HttpResponseMessage, Exception> res;
        var query = "https://playoverwatch.com/en-us/search/account-by-name/" + HttpUtility.UrlEncode(term);
        do {
            res = await Result.Protect(_http.GetAsync, query).ConfigureAwait(false);
        } while (retries-- > 0 && res.Contains(IsISE));

        bool IsISE(HttpResponseMessage res) {
            if (res.StatusCode != HttpStatusCode.InternalServerError) { return false; }

            res.Dispose();
            _logger.Information("Encountered internal server error for \"{term}\", retrying.", term);
            return true;
        }

        if (res.TryErr(out var err)) {
            return err;
        }
        using var response = res.Ok;

        if (!response.IsSuccessStatusCode) {
            return new ErrorStatusCodeException(response.StatusCode, response.ReasonPhrase);
        }

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = Result.Protect(JsonConvert.DeserializeObject<List<SearchResult>>, content, _defaultJsonSettings);
        if (result.TryErr(out err)) {
            return err;
        }
        result.Ok.RemoveAll(x => {
            if (!Utils.IsFullyDeserialized(x)) {
                _logger.Warning("Searching for {term} produced invalid result {profile}. " +
                                "Please report this to the library maintainer.", term, x);
                return true;
            }

            return false;
        });

        return result;
    }

    public enum GetFailureReason {
        ExtractionFailure,
        NotFound,
        LevelSearchFailure,
    }

    public async Task<Result<BaseProfile, GetFailureReason>> GetProfileAsync(Platform platform, BattleTag battleTag,
        Optional<uint> levelHint) {
        if (_disposed) { throw new ObjectDisposedException(nameof(OverwatchClient)); }

        var url = $"https://playoverwatch.com/en-us/career/{platform.ApiName}/{battleTag.ApiName}/";
        using var website = await _html.OpenAsync(url).ConfigureAwait(false);

        if (!Extractors.ExtractExistence(website, _logger).TryValue(out var exists)) { return GetFailureReason.ExtractionFailure; }
        if (exists == false) { return GetFailureReason.NotFound; }

        if (!Extractors.ExtractName(website, _logger).TryValue(out var name)) { return GetFailureReason.ExtractionFailure; }

        if (!Extractors.ExtractLevel(website, _logger).TryValue(out var level)) { return GetFailureReason.ExtractionFailure; }
        uint actualLevel;
        var levelRes = level.Convert();
        if (levelRes.ContainsErr(x => x == Level.FailureReason.TooHigh)) {
            if (levelHint.HasValue) {
                actualLevel = levelHint.Value;
            } else {
                var search = await SearchProfiles(battleTag.ToString()).ConfigureAwait(false);
                if (search.IsErr) {
                    return GetFailureReason.LevelSearchFailure;
                }
                var profile = search.Ok.FirstOrDefault(x => x.ProfileUrl == url);
                if (profile is null) { return GetFailureReason.LevelSearchFailure; }

                actualLevel = profile.Level;
            }
        } else {
            actualLevel = Math.Max(levelRes.Ok, levelHint.ValueOrDefault());
        }

        if (!Extractors.ExtractPortraitUrl(website, _logger).TryValue(out var portrait)) { return GetFailureReason.ExtractionFailure; }

        if (!Extractors.ExtractEndorsement(website, _logger).TryValue(out var endorsement)) { return GetFailureReason.ExtractionFailure; }

        if (!Extractors.ExtractPublic(website, _logger).TryValue(out var publicity)) { return GetFailureReason.ExtractionFailure; }

        if (!publicity) {
            return new BaseProfile(name, actualLevel, url, portrait, endorsement);
        }



        return null;
    }

    public Task<Result<BaseProfile, GetFailureReason>> GetProfileAsync(Platform platform, BattleTag battleTag)
        => GetProfileAsync(platform, battleTag, Optional.Empty);

    public OverwatchClient(ILogger logger) {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public OverwatchClient() {
        _logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    }

    ~OverwatchClient() {
        Dispose();
    }

    public void Dispose() {
        if (!_disposed) {
            _html.Dispose();
            _http.Dispose();
        }

        _disposed = true;
    }
}
