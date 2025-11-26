using System;
using Godot;

namespace Karaoke.Utils.Engine;

public static class EngineUtils
{
    public static void IterateThroughAllChildrenRecursive(this Node root, Action<Node> action)
    {
        foreach (var child in root.GetChildren())
        {
            action(child);
            IterateThroughAllChildrenRecursive(child, action);
        }
    }
}