using System;
using System.Collections.Generic;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Views;

public partial class LyricsView : Node, IInjectable
{
    [Export] private PackedScene _segmentScene;
    [Export] private Node[] _lineContainers;
    
    private readonly IDictionary<LyricsSegment, LyricsSegmentView> _activeSegments = new Dictionary<LyricsSegment, LyricsSegmentView>();
    
    private ISongRunner _songRunner;

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        _songRunner = serviceProvider.GetRequiredService<ISongRunner>();
    }

    public void NewLyrics(LyricsGroup group)
    {
        _activeSegments.Clear();
        ClearLines();
        for (int i = 0; i < group.Lines.Count; i++)
        {
            foreach (var segment in group.Lines[i].Segments)
            {
                var segmentNode = _segmentScene.Instantiate<LyricsSegmentView>();
                segmentNode.Initialize(segment);
                _lineContainers[i].AddChild(segmentNode);
                _activeSegments[segment] = segmentNode;
            }
        }
    }

    public override void _Process(double _)
    {
        foreach (var activeSegment in _activeSegments)
        {
            var progress = (_songRunner.Time - activeSegment.Key.StartTime) /
                           (activeSegment.Key.EndTime - activeSegment.Key.StartTime);
            activeSegment.Value.UpdateProgress(Mathf.Clamp(progress, 0f, 1f));
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