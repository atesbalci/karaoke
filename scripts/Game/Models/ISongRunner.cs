namespace Karaoke.Game.Models;

public interface ISongRunner
{
    float Time { get; }
    void RunSong(Lyrics lyrics);
    bool IsPaused { get; set; }
    float TimeScale { get; set; }
}