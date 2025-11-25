namespace Karaoke.Game.Models;

public interface ISongRunner
{
    float Time { get; }
    void RunSong(Lyrics lyrics);
}