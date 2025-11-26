using System;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Views;

public partial class SongTimeView : Label, IInjectable
{
    private ISongRunner _songRunner;
    
    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        _songRunner = serviceProvider.GetRequiredService<ISongRunner>();
    }

    public override void _Process(double _)
    {
        Text = _songRunner.Time.ToString("F1");
    }
}