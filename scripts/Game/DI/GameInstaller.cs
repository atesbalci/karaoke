using System;
using Godot;
using Karaoke.Game.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.DI;

public partial class GameInstaller : Node
{
    private IServiceProvider _services;
    
    public override void _Ready()
    {
        InstallBindings(); 
        this.IterateThroughAllChildrenRecursive(Inject);
        ChildEnteredTree += OnChildEnteredTree;
    }

    private void InstallBindings()
    {
        var serviceBindings = new ServiceCollection();
       
        _services = serviceBindings.BuildServiceProvider();
    }

    private void OnChildEnteredTree(Node node)
    {
        Inject(node);
    }

    private void Inject(Node node)
    {
        if (node is IInjectable injectable)
        {
            injectable.InjectDependencies(_services);
            GD.Print($"Injected {injectable.GetType().Name}!");
        }
    }
}