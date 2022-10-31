using AngleSharp.Dom;
using Ow.Net.Data;
using Serilog;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ow.Net.ConsistencyTests; 

public abstract class BaseChecker : IAsyncLifetime {
    private SearchResultsFixture _results;
    protected readonly ILogger _logger;
    protected readonly ITestOutputHelper _output;

    protected BaseChecker(ITestOutputHelper output) {
        _logger = new LoggerConfiguration().Destructure.With<HtmlDestructuringPolicy>()
            .AuditTo.Sink(new TestFailSink(output))
            .CreateLogger();
        _output = output;
    }

    protected Task Test(int count)
        => _results.Assert(count, _ => true, this, _output);

    protected Task Test(int count, Func<SearchResult, bool> pred)
        => _results.Assert(count, pred, this, _output);

    public abstract Task<bool> Check(SearchResult res, IDocument doc, OverwatchClient client);

    public async Task InitializeAsync() {
        _results = await SearchResultsFixture.Instance.ConfigureAwait(false);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
