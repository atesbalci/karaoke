using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Karaoke.Game.Models.Parsing;

public class CustomLyricsParser : ILyricsParser
{
    private static readonly Regex LineRegex = new ("<([^>]*)>([^<]*)");
    private static readonly Regex ParameterRegex = new ("([a-zA-z]+)=([^ ]+)");
    
    public Lyrics ParseLyrics(string lyricsContentRaw)
    {
        var lyrics = new Lyrics();
        foreach (var lineRaw in lyricsContentRaw.Split('\n'))
        {
            if (string.IsNullOrEmpty(lineRaw)) continue;
            if (lineRaw.StartsWith(">>"))
            {
                var time = float.Parse(ParseParameters(lineRaw.Substring(2))["t"]);
                lyrics.Groups.Add(new Group(time));
            }
            else
            {
                var line = new Line();
                lyrics.Groups[^1].Lines.Add(line);
                foreach (Match match in LineRegex.Matches(lineRaw))
                {
                    var parameters = ParseParameters(match.Groups[1].Value);
                    var start = float.Parse(parameters["ts"]);
                    var end = float.Parse(parameters["te"]);
                    var text = match.Groups[2].Value;
                    line.Segments.Add(new Segment(start, end, text));
                }
            }
        }
        
        return lyrics;
    }

    private IDictionary<string, string> ParseParameters(string parametersRaw)
    {
        var retVal = new Dictionary<string, string>();
        foreach (Match match in ParameterRegex.Matches(parametersRaw))
        {
            retVal[match.Groups[1].Value] = match.Groups[2].Value;
        }
        
        return retVal;
    }
}