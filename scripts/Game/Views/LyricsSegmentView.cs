using Godot;

namespace Karaoke.Game.Views;

public partial class LyricsSegmentView : Label
{
    private ShaderMaterial _material;
    
    public override void _Ready()
    {
        _material = (ShaderMaterial) Material.Duplicate();
        Material = _material;
    }

    public void Initialize(string text)
    {
        Text = text;
    }

    public void UpdateProgress(float progressNormalized)
    {
        _material.SetShaderParameter("Progress", progressNormalized * GetSize().X);
    }
}