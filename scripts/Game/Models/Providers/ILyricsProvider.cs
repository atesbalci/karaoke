namespace Karaoke.Game.Models.Providers;

public interface ILyricsProvider
{
    public Lyrics GetLyrics(string songId);
}