using Godot;
using System;

public partial class Map : Node2D
{

    PackedScene playerScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
    GameScene parent;
    Player player;
    GpuParticles2D rain;

    CameraController camera;


    bool debugtrigger = false;

    public override void _Ready()
    {
        parent = GetParent() as GameScene;
        camera = GetNode("CameraController") as CameraController;
        if (Globals.hasSavefile)
        {
            Globals.spawnPoint = GetNode<Node2D>("");       //LOAD CHECKPOINT FROM SAVEFILE HERE

        }
        else
            Globals.spawnPoint = GetNode<Node2D>("Checkpoint0");

        player = playerScene.Instantiate() as Player;
        rain = GetNode("FX/Rain") as GpuParticles2D;
        rain.Emitting = true;
        AddChild(player);
    }
    private void SetRoomStatus(bool savefile)
    {
        if (savefile)
        {
            foreach (EnemyControl controller in GetTree().GetNodesInGroup("Controllers"))
                controller.SetRoomCleared(true);    //ADD CLEAR STATUS FROM SAVEFILE HERE
        }else
        {
            foreach(EnemyControl controller in GetTree().GetNodesInGroup("Controllers"))
                controller.SetRoomCleared(false);
        }
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

    public void OnCameraMoved(string direction)
    {
        switch (direction)
        {
            case "left":
                rain.GlobalPosition = new Vector2(rain.GlobalPosition.X - 640, rain.GlobalPosition.Y);
                break;
            case "right":
                rain.GlobalPosition = new Vector2(rain.GlobalPosition.X + 640, rain.GlobalPosition.Y);
                break;
            default:
                break;
        }
    }
}
