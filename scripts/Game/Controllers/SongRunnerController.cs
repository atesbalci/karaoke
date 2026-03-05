using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Providers;
using Karaoke.Game.Views;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Controllers;

public partial class SongRunnerController : AudioStreamPlayer, IInjectable, ISongRunner
{
    [Export] private LyricsView _lyricsView;
    [Export] private LyricsNextTimerView _lyricsNextTimerView;
    [Export] private string _testSongResource;

    private int _groupIndex;
    private CancellationTokenSource _cancellationTokenSource;
    private float _timeScale = 1f;
    private float _pitch = 1f;

    public float Time { get; private set; }

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        var lyricsProvider = serviceProvider.GetRequiredService<ILyricsProvider>();
        var settings = serviceProvider.GetRequiredService<SongRunnerSettings>();
        var lyrics = lyricsProvider.GetLyrics(settings.SongId);
        RunSong(lyrics, settings.SongId);
        RefreshPitch();
    }

    public void RunSong(Lyrics lyrics, string songPath)
    {
        _cancellationTokenSource?.Cancel();
        Time = 0f;
        Stream = GD.Load<AudioStream>("res://sounds/" + songPath + ".mp3");
        Play();
        _cancellationTokenSource = new CancellationTokenSource();
        _ = RunSongAsync(lyrics, _cancellationTokenSource.Token);
    }

    private async Task RunSongAsync(Lyrics lyrics, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        bool isEmpty = true;
        foreach (var group in lyrics.Groups)
        {
            if (cancellationToken.IsCancellationRequested) return;
            float remainingTime = group.Time - Time;
            if (remainingTime > 3f && isEmpty) _ = _lyricsNextTimerView.Countdown(remainingTime, cancellationToken);
            await ScaledDelay(remainingTime, cancellationToken);
            _lyricsView.NewLyrics(group);
            isEmpty = group.Lines.Count == 0;
        }

        GetTree().ReloadCurrentScene();
    }

    public float Pitch
    {
        get => _pitch;
        set
        {
            _pitch = value;
            RefreshPitch();
        }
    }

    public async Task ScaledDelay(float delay, CancellationToken token)
    {
        float startTime = Time;
        while (!token.IsCancellationRequested && Time - startTime < delay)
        {
            await Task.Delay(1, token);
        }
    }

    public bool IsPaused { get; set; }

    public float TimeScale
    {
        get => _timeScale;
        set
        {
            _timeScale = value;
            RefreshPitch();
        }
    }

    private void RefreshPitch()
    {
        PitchScale = TimeScale;
        ((AudioEffectPitchShift)AudioServer.GetBusEffect(0, 0)).PitchScale = (1f / _timeScale) * Pitch;
    }

    public override void _Process(double delta)
    {
        if (IsPaused) return;
        Time += (float) delta * TimeScale;
    }
}