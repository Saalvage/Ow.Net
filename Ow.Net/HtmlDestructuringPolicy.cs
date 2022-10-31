using AngleSharp;
using AngleSharp.Dom;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;

namespace Ow.Net; 

public class HtmlDestructuringPolicy : IDestructuringPolicy {
    public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory,
        [MaybeNullWhen(false)] out LogEventPropertyValue result) {
        var str = value switch {
            IDocument doc => $"({doc.StatusCode}, {doc.Title}, {doc.Url})",
            IElement elem => elem.ToHtml(),
            _ => null,
        };
        result = str is null ? null : propertyValueFactory.CreatePropertyValue(str);
        return result is not null;
    }
}
