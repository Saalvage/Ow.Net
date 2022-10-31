using AngleSharp.Dom;
using Ow.Net.Data;
using Serilog;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Ow.Net; 

internal static class Extractors {
    internal static Optional<bool> ExtractExistence(IDocument doc, ILogger logger) {
        Optional<bool> ret = doc.Title switch {
            "Career Overview - Overwatch" => true,
            "Overwatch" or "" or null => false,
            _ => Optional.Empty,
        };

        if (ret.IsEmpty) {
            logger.Error("Could not determine whether profile in document {@doc} exists. "
                + "Please report this to the library maintainer.", doc);
        }

        return ret;
    }

    internal static Optional<string> ExtractName(IDocument doc, ILogger logger) {
        var ret = (doc.GetElementsByClassName("header-masthead").FirstOrDefault()?.TextContent).ToOptional();
        if (ret.IsEmpty) {
            logger.Error("Could not find name in document {@doc}. "
                + "Please report this to the library maintainer.", doc);
        }
        return ret;
    }

    private static readonly Regex UrlStyle = new(
        @"background-image:url\( (?<url>.*) \)");

    private static Optional<string> ExtractUrl(IElement elem) {
        var style = elem.GetAttribute("style");
        if (style is null) { return Optional.Empty; }
        var match = UrlStyle.Match(style);
        if (!match.Success) { return Optional.Empty; }

        return match.Groups["url"].Value;
    }

    internal static Optional<Level> ExtractLevel(IDocument doc, ILogger logger) {
        const string STAR_CLASS_NAME = "player-rank";

        var rootElement = doc.GetElementsByClassName("player-level").FirstOrDefault();
        if (rootElement is null) {
            logger.Error("Could not find root level node in document {@doc}. " +
                         "Please report this to the library maintainer.", doc);
            return Optional.Empty;
        }
        var frameUrl = ExtractUrl(rootElement);
        if (frameUrl.IsEmpty) {
            logger.Error("Could not determine frame url from element {@elem} in document {@doc}. " +
                         "Please report this to the library maintainer.", rootElement, doc);
            return Optional.Empty;
        }

        if (rootElement.ChildElementCount == 0) {
            logger.Error("Root level node did not have sufficient children in document {@doc}. " +
                         "Please report this to the library maintainer.", doc);
            return Optional.Empty;
        }

        var lvlTextElement = rootElement.FirstElementChild;
        var lvlText = lvlTextElement!.TextContent;
        if (!byte.TryParse(lvlText, NumberStyles.Any, CultureInfo.InvariantCulture, out var rest)) {
            logger.Error("Rest node content did not parse \"{@cnt}\" in document {@doc}. " +
                         "Please report this to the library maintainer.", lvlText, doc);
            return Optional.Empty;
        }

        IElement? starElement = null;
        bool tryFallback = true;
        if (rootElement.ChildElementCount < 2) {

        } else if ((starElement = lvlTextElement.NextElementSibling!).ClassName != STAR_CLASS_NAME) {
            logger.Warning("Potential star node has incorrect class name \"{@name}\", trying to use fallback in document {@doc}. " +
                           "Please report this to the library maintainer.", starElement.ClassName, doc);
        } else {
            tryFallback = false;
        }

        if (tryFallback) {
            starElement = doc.GetElementsByClassName(STAR_CLASS_NAME).FirstOrDefault() ?? starElement;
        }

        var starUrl = starElement is null ? "" : ExtractUrl(starElement);
        if (starUrl.IsEmpty) {
            logger.Error("Could not determine star url from element {@elem} in document {@doc}. " +
                         "Please report this to the library maintainer.", starElement, doc);
            return Optional.Empty;
        }

        return new Level(starUrl.Value, frameUrl.Value, rest);
    }

    internal static Optional<string> ExtractPortraitUrl(IDocument doc, ILogger logger) {
        var opt = doc.GetElementsByClassName("player-portrait").FirstOrDefault().ToOptional();
        if (opt.IsEmpty) {
            logger.Error("Could not find portrait in document {@doc}. " +
                         "Please report this to the library maintainer.", doc);
            return Optional.Empty;
        }

        var ret = opt.Bind(x => x.GetAttribute("src").ToOptional());
        if (ret.IsEmpty) {
            logger.Error("Could not determine portrait url from element {@elem} in document {@doc}. " +
                         "Please report this to the library maintainer.", opt.Value, doc);
        }
        return ret;
    }

    internal static Optional<Endorsement> ExtractEndorsement(IDocument doc, ILogger logger) {
        var levelElement = doc.GetElementsByClassName("u-center").FirstOrDefault(); // Class name scares me.
        if (levelElement is null) {
            logger.Error("Could not find endorsement level node in document {@doc}. " +
                         "Please report this to the library maintainer.", doc);
            return Optional.Empty;
        }

        if (!byte.TryParse(levelElement.TextContent, NumberStyles.Any, CultureInfo.InvariantCulture, out var level)) {
            logger.Error("Could not extract endorsement level from {@elem} in document {@doc}. " +
                         "Please report this to the library maintainer.", levelElement, doc);
            return Optional.Empty;
        }

        var element = doc.GetElementsByClassName("EndorsementIcon-inner").FirstOrDefault();
        if (element is null) {
            logger.Error("Could not find root endorsement node in document {@doc}. " +
                         "Please report this to the library maintainer.", doc);
            return Optional.Empty;
        }

        var sc = ClassNameToEndorsement("EndorsementIcon-border--shotcaller");
        if (sc.IsEmpty) { return Optional.Empty; }

        var gtm = ClassNameToEndorsement("EndorsementIcon-border--teammate");
        if (gtm.IsEmpty) { return Optional.Empty; }

        var ss = ClassNameToEndorsement("EndorsementIcon-border--sportsmanship");
        if (ss.IsEmpty) { return Optional.Empty; }

        Optional<decimal> ClassNameToEndorsement(string className) {
            var end = element.GetElementsByClassName(className).FirstOrDefault();
            if (end is null) {
                logger.Error("Could not find \"{className}\" class in document {@doc}. " +
                             "Please report this to the library maintainer.", className, doc);
                return Optional.Empty;
            }

            var str = end.GetAttribute("data-value");
            if (str is null) {
                logger.Error("Could not find data value attribute in element {@elem} in document {@doc}. " +
                             "Please report this to the library maintainer.", end, doc);
                return Optional.Empty;
            }

            if (!decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var ret)) {
                logger.Error("Could not parse data from {@elem} to a percentage in document {@doc}. " +
                             "Please report this to the library maintainer.", end, doc);
                return Optional.Empty;
            }
            return ret;
        }

        return new Endorsement(level, sc.Value, gtm.Value, ss.Value);
    }

    internal static Optional<bool> ExtractPublic(IDocument doc, ILogger logger) {
        var element = doc.GetElementsByClassName("masthead-permission-level-text").FirstOrDefault();
        if (element is null) {
            logger.Error("Could not find privacy level in document {@doc}. " +
                         "Please report this to the library maintainer.", doc);
            return Optional.Empty;
        }

        Optional<bool> result = element.TextContent switch {
            "Public Profile" => true,
            "Private Profile" => false,
            _ => Optional.Empty,
        };

        if (result.IsEmpty) {
            logger.Error("Could not determine public status from {str} in document {@doc}. " +
                         "Please report this to the library maintainer.", element.TextContent, doc);
        }

        return result;
    }
}
