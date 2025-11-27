using Godot;
using Karaoke.Game.Models.Parsing;

namespace Karaoke.Game.Models.Providers;

public class LocalLyricsProvider(ILyricsParser lyricsParser) : ILyricsProvider
{
    public Lyrics GetLyrics(string songId)
    {
        return lyricsParser.ParseLyrics(FileAccess.GetFileAsString(LocalConstants.SongsPath + songId + ".txt"));
    }
}