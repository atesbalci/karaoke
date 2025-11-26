using System;
using System.Collections.Generic;
using System.Globalization;
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
                var time = ParseParameters(lineRaw.Substring(2)).GetParameter<float>("t");
                lyrics.Groups.Add(new LyricsGroup(time));
            }
            else
            {
                var line = new LyricsLine();
                lyrics.Groups[^1].Lines.Add(line);
                foreach (Match match in LineRegex.Matches(lineRaw))
                {
                    var parameters = ParseParameters(match.Groups[1].Value);
                    var start = parameters.GetParameter<float>("ts");
                    var end = parameters.GetParameter<float>("te");
                    var text = match.Groups[2].Value;
                    line.Segments.Add(new LyricsSegment(start, end, text));
                }
            }
        }
        
        return lyrics;
    }

    private Parameters ParseParameters(string parametersRaw)
    {
        var retVal = new Parameters();
        foreach (Match match in ParameterRegex.Matches(parametersRaw))
        {
            retVal.SetParameter(match.Groups[1].Value, match.Groups[2].Value);
        }
        
        return retVal;
    }

    private class Parameters
    {
        private readonly IDictionary<string, string> _parameters = new Dictionary<string, string>();

        public void SetParameter(string key, string val)
        {
            _parameters[key] = val;
        }

        public T GetParameter<T>(string key)
        {
            var type = typeof(T);
            if (type.IsEnum)
            {
                return (T)Enum.Parse(type, _parameters[key]);
            }

            return (T)Convert.ChangeType(_parameters[key], type, CultureInfo.InvariantCulture);
        }
    }
}