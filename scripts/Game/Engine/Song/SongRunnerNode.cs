using System;
using System.Collections.Generic;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Engine.Song;

public partial class SongRunnerNode : Node, ISongRunner, IInjectable
{
    [Export] private PackedScene _segmentScene;
    [Export] private Node[] _lineContainers;

    [Export] private string _testSongResource;

    private Lyrics _lyrics;
    private int _groupIndex;
    private readonly IDictionary<Segment, Label> _activeSegments = new Dictionary<Segment, Label>();

    public float Time { get; private set; }

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        var lyricsParser = serviceProvider.GetRequiredService<ILyricsParser>();
        var lyrics = lyricsParser.ParseLyrics(FileAccess.GetFileAsString(_testSongResource));
        GD.Print(lyrics.Groups.Count);
        RunSong(lyrics);
    }

    public void RunSong(Lyrics lyrics)
    {
        Time = 0f;
        _lyrics = lyrics;
        _groupIndex = 0;
        _activeSegments.Clear();
    }

    public override void _Process(double delta)
    {
        Time += (float) delta;

        if (_activeSegments.Count > 0)
        {
            // TODO: Segment processing
        }
        
        if (_groupIndex < _lyrics.Groups.Count)
        {
            var group = _lyrics.Groups[_groupIndex];
            if (Time > group.Time)
            {
                ClearLines();
                for (int i = 0; i < group.Lines.Count; i++)
                {
                    foreach (var segment in group.Lines[i].Segments)
                    {
                        var segmentNode = _segmentScene.Instantiate<Label>();
                        segmentNode.Text = segment.Text;
                        _lineContainers[i].AddChild(segmentNode);
                        _activeSegments[segment] = segmentNode;
                    }
                }

                _groupIndex++;
            } 
        }
    }

    private void ClearLines()
    {
        foreach (var lineContainer in _lineContainers)
        {
            foreach (var child in lineContainer.GetChildren())
            {
                child.QueueFree();
            }
        }
    }
}