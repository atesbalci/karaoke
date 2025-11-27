using System;
using System.Collections.Generic;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Game.Models.Providers;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Views;

public partial class SongListView : Node, IInjectable
{
    [Export] private Node _songsParentNode;
    [Export] private PackedScene _songListingEntry;
    [Export] private PackedScene _songScene;

    private SongRunnerSettings _songRunnerSettings;

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        _songRunnerSettings = serviceProvider.GetRequiredService<SongRunnerSettings>();
        PopulateList(serviceProvider.GetRequiredService<ISongListProvider>().GetSongIds());
    }

    private void PopulateList(IEnumerable<string> songs)
    {
        foreach (var song in songs)
        {
            var songEntry = _songListingEntry.Instantiate<Button>();
            _songsParentNode.AddChild(songEntry);
            songEntry.Text = song;
            songEntry.Pressed += () => OnSongClicked(song);
        }
    }

    private void OnSongClicked(string id)
    {
        _songRunnerSettings.SongId = id;
        QueueFree();
        GetParent().AddChild(_songScene.Instantiate());
    }
}