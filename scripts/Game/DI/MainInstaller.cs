using System;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.DI;

public partial class MainInstaller : DIInstaller
{
    protected override IServiceProvider InstallBindings(IServiceProvider parentContext)
    {
        return new ServiceCollection()
            .AddSingleton<ISongListProvider, LocalSongListProvider>()
            .AddSingleton<SongRunnerSettings>()
            .BuildServiceProvider();
    }
}