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
    [Export] private LyricsCursorView _cursorView;
    
    private readonly IDictionary<LyricsSegment, LyricsSegmentView> _activeSegments = new Dictionary<LyricsSegment, LyricsSegmentView>();
    
    private ISongRunner _songRunner;
    private Node _linesParent;

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        _songRunner = serviceProvider.GetRequiredService<ISongRunner>();
        
        // Initialize Subnodes
        _linesParent = GetNode("Lines");
        GetNode<Button>("Pause").Toggled += OnPauseToggled;
        GetNode<Button>("FastForward").Toggled += OnFastForwardToggled;
    }

    private void OnFastForwardToggled(bool toggledOn)
    {
        _songRunner.TimeScale = toggledOn ? 4f : 1f;
    }

    private void OnPauseToggled(bool toggledOn)
    {
        _songRunner.IsPaused = toggledOn;
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
                segmentNode.Initialize(segment, i);
                _linesParent.GetChild(i).AddChild(segmentNode);
                _activeSegments[segment] = segmentNode;
            }
        }
    }

    public override void _Process(double _)
    {
        _cursorView.SetVisibility(_activeSegments.Count > 0);
        foreach (var (segment, view) in _activeSegments)
        {
            var progress = segment.GetProgress(_songRunner.Time);
            view.UpdateProgress(progress);
            if (progress is < 1f and > 0f)
            {
                _cursorView.Update(view, view.LineNumber > 0, progress);
            }
        }
    }

    private void ClearLines()
    {
        foreach (var lineContainer in _linesParent.GetChildren())
        {
            foreach (var child in lineContainer.GetChildren())
            {
                child.QueueFree();
            }
        }
    }
}