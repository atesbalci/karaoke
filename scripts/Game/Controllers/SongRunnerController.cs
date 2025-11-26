using System;
using System.Collections.Generic;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Parsing;
using Karaoke.Game.Views;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Controllers;

public partial class SongRunnerController : Node, IInjectable, ISongRunner
{
    [Export] private LyricsView _lyricsView;
    [Export] private string _testSongResource;

    private Lyrics _lyrics;
    private int _groupIndex;

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
    }

    public bool IsPaused { get; set; }
    public float TimeScale { get; set; } = 1f;

    public override void _Process(double delta)
    {
        if (IsPaused) return;
        Time += (float) delta * TimeScale;
        
        if (_groupIndex < _lyrics.Groups.Count)
        {
            var group = _lyrics.Groups[_groupIndex];
            if (Time > group.Time)
            {
                _lyricsView.NewLyrics(group);
                _groupIndex++;
            } 
        }
    }
}