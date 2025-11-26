using System;
using Godot;
using Karaoke.Game.Controllers;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Parsing;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.DI;

public partial class SongInstaller : Node
{
    [Export] private SongRunnerController _songRunnerController;
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
            .AddSingleton<ISongRunner>(_songRunnerController)
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