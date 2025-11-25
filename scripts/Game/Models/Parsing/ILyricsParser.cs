namespace Karaoke.Game.Models.Parsing;

public interface ILyricsParser
{
    Lyrics ParseLyrics(string lyricsContentRaw);
}