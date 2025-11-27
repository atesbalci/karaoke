using System.Collections.Generic;

namespace Karaoke.Game.Models.Providers;

public interface ISongListProvider
{
    IEnumerable<string> GetSongIds();
}