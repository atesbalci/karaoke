using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

namespace Karaoke.Game.Models.Providers;

public class LocalSongListProvider : ISongListProvider
{
    public IEnumerable<string> GetSongIds()
    {
        return DirAccess.GetFilesAt(LocalConstants.SongsPath).Select(Path.GetFileNameWithoutExtension);
    }
}