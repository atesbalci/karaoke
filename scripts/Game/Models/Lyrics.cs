using System.Collections.Generic;

namespace Karaoke.Game.Models;

public class Lyrics
{
    public IList<Group> Groups { get; } = new List<Group>();
}

public class Group
{
    public float Time { get; }
    public IList<Line> Lines { get; } =  new List<Line>();

    public Group(float time)
    {
        Time = time;
    }
}

public class Line
{
    public IList<Segment> Segments { get; } = new List<Segment>();
}

public class Segment
{
    public float StartTime { get; }
    public float EndTime { get; }
    public string Text { get; }

    public Segment(float startTime, float endTime, string text)
    {
        StartTime = startTime;
        EndTime = endTime;
        Text = text;
    }
}