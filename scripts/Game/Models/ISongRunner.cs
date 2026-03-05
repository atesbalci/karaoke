using System.Threading;
using System.Threading.Tasks;

namespace Karaoke.Game.Models;

public interface ISongRunner
{
    float Time { get; }
    void RunSong(Lyrics lyrics, string  songPath);
    bool IsPaused { get; set; }
    float TimeScale { get; set; }
    float Pitch { get; set; }
    Task ScaledDelay(float delay, CancellationToken token);
}