using Godot;
using System;

public partial class Map : Node2D
{

    PackedScene playerScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
    GameScene parent;
    Player player;

    CameraController camera;

    bool debugtrigger = false;

    public override void _Ready()
    {
        parent = GetParent() as GameScene;
        camera = GetNode("CameraController") as CameraController;
        Globals.spawnPoint = GetNode<Node2D>("SpawnPoint");
        player = playerScene.Instantiate() as Player;
        AddChild(player);

    }

    public void Respawn()
    {
        camera.RespawnMove();
        foreach(EnemyControl controller in GetTree().GetNodesInGroup("Controllers"))
        {
            controller.DespawnEnemies();    
        }
        player.SetStats();
    }

}
