using System;
using Godot;
using Karaoke.Game.Engine;
using Karaoke.Game.Models.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.DI;

public partial class SongInstaller : Node
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
        _services = new ServiceCollection()
            .AddSingleton<ILyricsParser, CustomLyricsParser>()
            .BuildServiceProvider();
    }

    private void OnChildEnteredTree(Node node)
    {
        node.IterateThroughAllChildrenRecursive(Inject);
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