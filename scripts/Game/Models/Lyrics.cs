using System.Collections.Generic;

namespace Karaoke.Game.Models;

public class Lyrics
{
    public IList<LyricsGroup> Groups { get; } = new List<LyricsGroup>();
}

public class LyricsGroup
{
    public float Time { get; }
    public IList<LyricsLine> Lines { get; } =  new List<LyricsLine>();

    public LyricsGroup(float time)
    {
        Time = time;
    }
}

public class LyricsLine
{
    public IList<LyricsSegment> Segments { get; } = new List<LyricsSegment>();
}

public class LyricsSegment
{
    public float StartTime { get; }
    public float EndTime { get; }
    public string Text { get; }
    public LyricsStyle Style { get; }

    public LyricsSegment(float startTime, float endTime, string text, LyricsStyle style)
    {
        StartTime = startTime;
        EndTime = endTime;
        Text = text;
        Style = style;
    }
}

public enum LyricsStyle
{
    Regular,
    Wiggle,
    ColorWave
}