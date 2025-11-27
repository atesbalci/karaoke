using System;
using Godot;
using Karaoke.Game.Models;
using Karaoke.Utils.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Karaoke.Game.Views;

public partial class LyricsCursorView : Path2D, IInjectable
{
    private ISongRunner _songRunner;
    private PathFollow2D _follower;
    private Control _cursor;

    public void InjectDependencies(IServiceProvider serviceProvider)
    {
        _songRunner = serviceProvider.GetRequiredService<ISongRunner>();
        _follower = GetChild<PathFollow2D>(0);
        _cursor = _follower.GetChild<Control>(0);
        _cursor.Visible = false;
    }

    public void SetVisibility(bool b)
    {
        _cursor.Visible = b;
    }

    public void Update(Control segmentView, bool isBottomLine, float progress)
    {
        _cursor.Visible = true;
        var size = segmentView.Size;
        var viewPos = segmentView.GlobalPosition;
        Curve.SetPointPosition(0, viewPos + new Vector2(0f, isBottomLine ? size.Y : 0f));
        Curve.SetPointPosition(1, viewPos + new Vector2(size.X, isBottomLine ? size.Y : 0f));
        Curve.SetPointOut(0, new Vector2(size.X * 0.5f, 200f * (isBottomLine ? 1f : -1f)));
        _follower.ProgressRatio = progress;
    }
}