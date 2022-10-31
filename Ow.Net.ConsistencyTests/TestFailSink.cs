using Serilog.Core;
using Serilog.Events;
using Xunit.Abstractions;

namespace Ow.Net.ConsistencyTests;

public class TestFailSink : ILogEventSink {
    private readonly LogEventLevel _level;
    private readonly ITestOutputHelper? _output;

    public TestFailSink(ITestOutputHelper? output = null) {
        _output = output;
        _level = LogEventLevel.Warning;
    }

    public TestFailSink(ITestOutputHelper? output, LogEventLevel minErrorLevel) {
        _output = output;
        _level = minErrorLevel;
    }

    public void Emit(LogEvent logEvent) {
        var msg = logEvent.RenderMessage();
        _output?.WriteLine(msg);
        if (logEvent.Level >= _level) {
            throw new(msg);
        }
    }
}
