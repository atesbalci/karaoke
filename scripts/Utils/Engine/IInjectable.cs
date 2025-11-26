using System;

namespace Karaoke.Utils.Engine;

public interface IInjectable
{
    void InjectDependencies(IServiceProvider serviceProvider);
}