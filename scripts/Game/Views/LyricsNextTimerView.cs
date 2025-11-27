using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Views;

public partial class LyricsNextTimerView : Label, IInjectable
{
    private ISongRunner _songRunner;
    private Tween _tween;
    
    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        _songRunner = serviceProvider.GetRequiredService<ISongRunner>();
    }

    public async Task Countdown(float seconds, CancellationToken cancellationToken)
    {
        var secondsDecimal = Mathf.FloorToInt(seconds + 0.001f);
        
        // Trim the subseconds
        await _songRunner.ScaledDelay(seconds - secondsDecimal, cancellationToken);

        for (int i = secondsDecimal; i > 0; i--)
        {
            ShowNumber(i);
            await _songRunner.ScaledDelay(1f, cancellationToken);
        }
    }

    private void ShowNumber(int val)
    {
        _tween?.Kill();
        Scale = Vector2.One;
        Set("theme_override_colors/font_color", new Color(1f, 1f, 1f));
        Text = val.ToString();
        _tween = CreateTween().BindNode(this);
        _tween.TweenProperty(this, "theme_override_colors/font_color", new Color(1f, 1f, 1f, 0f), 0.5f);
        _tween.Parallel().TweenProperty(this, "scale", new Vector2(2f, 2f), 0.5f);
        _tween.TweenCallback(Callable.From(() => Text = string.Empty));
    }
}