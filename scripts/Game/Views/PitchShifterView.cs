using System;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;
using Range = Godot.Range;

namespace Karaoke.Game.Views;

public partial class PitchShifterView : Range, IInjectable
{
    [Export] private AudioBusLayout _audioBusLayout;
    private ISongRunner _songRunner;

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        _songRunner = serviceProvider.GetRequiredService<ISongRunner>();
    }
    
    public override void _ValueChanged(double newValue)
    {
        _songRunner.Pitch = (float)newValue;
    }
}