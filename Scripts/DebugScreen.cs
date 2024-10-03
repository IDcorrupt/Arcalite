using Godot;
using System;

public partial class DebugScreen : RichTextLabel
{
    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("/root/Node2D/Player");
    }

    public override void _Process(double delta)
    {
        if (_player != null)
        {
            bool isonfloor = _player.getIsonFloor();
            Text = $"is on floor: {isonfloor}";
        }

    }
}
