using System;
using Godot;
using Karaoke.Game.Models;

namespace Karaoke.Game.Views;

public partial class LyricsSegmentView : Label
{
    [Export] private ShaderMaterial _regularMaterial;
    [Export] private ShaderMaterial _wiggleMaterial;
    [Export] private ShaderMaterial _colorWaveMaterial;
    
    private ShaderMaterial _material;

    public void Initialize(LyricsSegment segment)
    {
        Text = segment.Text;

        ShaderMaterial mat;
        switch (segment.Style)
        {
            case LyricsStyle.Wiggle:
                mat = _wiggleMaterial;
                break;
            case LyricsStyle.ColorWave:
                mat = _colorWaveMaterial;
                break;
            default:
                mat = _regularMaterial;
                break;
        }
        
        _material = (ShaderMaterial) mat.Duplicate();
        Material = _material;
    }

    public void UpdateProgress(float progressNormalized)
    {
        _material.SetShaderParameter("Progress", progressNormalized * GetSize().X);
    }
}