using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Providers;
using Karaoke.Game.Views;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Controllers;

public partial class SongRunnerController : Node, IInjectable, ISongRunner
{
    [Export] private LyricsView _lyricsView;
    [Export] private LyricsNextTimerView _lyricsNextTimerView;
    [Export] private string _testSongResource;

    private int _groupIndex;
    private CancellationTokenSource _cancellationTokenSource;
    private IList<Tuple<float, TaskCompletionSource>> _waits = new List<Tuple<float, TaskCompletionSource>>();

    public float Time { get; private set; }

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        var lyricsProvider = serviceProvider.GetRequiredService<ILyricsProvider>();
        var settings = serviceProvider.GetRequiredService<SongRunnerSettings>();
        var lyrics = lyricsProvider.GetLyrics(settings.SongId);
        RunSong(lyrics);
    }

    public void RunSong(Lyrics lyrics)
    {
        _cancellationTokenSource?.Cancel();
        Time = 0f;
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

    public Task ScaledDelay(float delay, CancellationToken token)
    {
        var taskCompletionSource = new TaskCompletionSource(token);
        _waits.Add(new Tuple<float, TaskCompletionSource>(Time + delay, taskCompletionSource));
        return taskCompletionSource.Task;
    }

    public bool IsPaused { get; set; }
    public float TimeScale { get; set; } = 1f;

    public override void _Process(double delta)
    {
        if (IsPaused) return;
        Time += (float) delta * TimeScale;
        for (int i = _waits.Count - 1; i >= 0; i--)
        {
            var wait = _waits[i];
            if (wait.Item1 - 0.001f < Time)
            {
                wait.Item2.SetResult();
                _waits.RemoveAt(i);
            }
        }
    }
}