using System;
using Godot;
using Karaoke.Utils.Engine;

namespace Karaoke.Game.DI;

public abstract partial class DIInstaller : Node
{
    public IServiceProvider Services { get; private set; }
    
    public override void _Ready()
    {
        // TODO: Find a more elegant solution for this
        Services = InstallBindings((GetTree().Root.GetChild(0) as DIInstaller)?.Services);
        this.IterateThroughAllChildrenRecursive(Inject);
    }

    protected abstract IServiceProvider InstallBindings(IServiceProvider parentContext);

    private void Inject(Node node)
    {
        if (node is IInjectable injectable)
        {
            injectable.InjectDependencies(Services);
        }
    }
}