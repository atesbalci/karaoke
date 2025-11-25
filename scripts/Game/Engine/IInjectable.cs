using System;

namespace Karaoke.Game.Engine;

public interface IInjectable
{
    void InjectDependencies(IServiceProvider serviceProvider);
}