using Godot;
using System;

public partial class Map : Node2D
{
    PackedScene Player = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
    GameScene parent;

    bool debugtrigger = false;

    public override void _Ready()
    {
        parent = GetParent() as GameScene;
        Globals.spawnPoint = GetNode<Node2D>("SpawnPoint");
        Node2D player = (Node2D)Player.Instantiate();
        AddChild(player);

    }

    public override void _Process(double delta)
    {
       
    }

    
}
