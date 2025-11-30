using System;
using Godot;

namespace Karaoke.Game.Views;

public partial class LyricsCursorView : Path2D
{
    private PathFollow2D _follower;
    private Control _cursor;
    private Label _currentSegment;

    public override void _Ready()
    {
        _follower = GetChild<PathFollow2D>(0);
        _cursor = _follower.GetChild<Control>(0);
        _cursor.Visible = false;
    }

    public void SetVisibility(bool b)
    {
        _cursor.Visible = b;
    }

    public void Update(Label segmentView, bool isBottomLine, float progress)
    {
        if (_currentSegment != segmentView)
        {
            _currentSegment = segmentView;
            RefreshPath(isBottomLine);
        }

        _follower.ProgressRatio = progress;
    }

    private void RefreshPath(bool isBottomLine)
    {
        var size = _currentSegment.Size;
        var viewPos = _currentSegment.GlobalPosition;
        var text = _currentSegment.Text;
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Curve.PointCount = words.Length + 1;
        float totalLength = text.Length;
        float curX = 0f;
        Curve.SetPointPosition(0, PositionOnLabel(0f, isBottomLine));
        for (int i = 0; i < words.Length; i++)
        {
            var wordSizeX = ((words[i].Length + 1) / totalLength) * size.X;
            curX += wordSizeX;
            Curve.SetPointPosition(i + 1, PositionOnLabel(curX, isBottomLine));
            Curve.SetPointOut(i, new Vector2(wordSizeX * 0.5f, 100f * (isBottomLine ? 1f : -1f)));
        }
    }

    private Vector2 PositionOnLabel(float xOffset, bool isBottomLine)
    {
        return _currentSegment.GlobalPosition +
               new Vector2(xOffset, isBottomLine ? _currentSegment.Size.Y : 0f);
    }
}