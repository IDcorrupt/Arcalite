using Godot;
using System;

public partial class DebugUi : Control
{
    private Player _player;

    public override void _Ready()
    {
        _player = GetNode<Player>("./Player");
    }

    public override void _Process(double delta)
    {
        if (_player != null)
        {
            bool isonfloor = _player.getIsonFloor();
            
            
        }
    }
}
