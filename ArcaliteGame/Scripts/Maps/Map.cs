using Godot;
using System;

public partial class Map : Node2D
{
    Resource cursor = ResourceLoader.Load("res://Assets/Placeholder assets/Cursors/PNG/White/crosshair124.png");
    PackedScene Player = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
    public override void _Ready()
    {
        Globals.spawnPoint = GetNode<Node2D>("SpawnPoint");
        Input.SetCustomMouseCursor(cursor);
        Node2D player = (Node2D)Player.Instantiate();
        AddChild(player);
        player.Position = Globals.spawnPoint.Position;
    }

    public override void _Process(double delta)
    {
        
    }
}
