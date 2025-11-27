using System;
using Godot;
using Karaoke.Game.Controllers;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Parsing;
using Karaoke.Game.Models.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.DI;

public partial class SongInstaller : DIInstaller
{
    [Export] private SongRunnerController _songRunnerController;

    protected override IServiceProvider InstallBindings(IServiceProvider provider)
    {
        // Use the default song if the scene is directly loaded
        var settings = provider == null
            ? new SongRunnerSettings { SongId = "song" }
            : provider.GetRequiredService<SongRunnerSettings>();
        
        return new ServiceCollection()
            .AddSingleton(settings)
            .AddSingleton<ILyricsParser, CustomLyricsParser>()
            .AddSingleton<ILyricsProvider, LocalLyricsProvider>()
            .AddSingleton<ISongRunner>(_songRunnerController)
            .BuildServiceProvider();
    }
}