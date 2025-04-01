using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public partial class Map : Node2D
{

    PackedScene playerScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
    GameScene parent;
    Player player;
    GpuParticles2D rain;
    CameraController camera;

    Node2D checkpointContainer;

    bool debugtrigger = false;

    public override void _Ready()
    {
        //get general nodes
        parent = GetParent() as GameScene;
        camera = GetNode("CameraController") as CameraController;
        checkpointContainer = GetNode("CheckPoints") as Node2D;

        //set up checkpoints (and room statuses if loading from save)
        if (Globals.hasSavefile)
        {
            //delete(free) all checkpoints before the current one (so they can't be activated -> no save cheesing
            int marker = Convert.ToInt32(Globals.currentSave[1].Last()); //number at the end of checkpoint name
            int counter = 0;
            while (Convert.ToInt32(checkpointContainer.GetChildren()[counter].Name.ToString().Last()) < marker)
            {
                checkpointContainer.GetChildren()[counter].QueueFree();
                counter++;
            }
            //set spawnpoint
            Globals.spawnPoint = GetNode($"CheckPoints/{Globals.currentSave[1]}") as Checkpoint;
            camera.RespawnMove();
        }
        else
        {
            Globals.spawnPoint = GetNode("CheckPoints/Checkpoint0") as Checkpoint;
        }
        SetRoomStatus();
        Globals.spawnPoint.Empty();

        //fx
        rain = GetNode("FX/Rain") as GpuParticles2D;
        rain.Emitting = true;

        //player
        player = playerScene.Instantiate() as Player;
        AddChild(player);

    }
    private void SetRoomStatus()
    {
        int i = 0;
        if (Globals.hasSavefile)
        {
            string[] roomBools = Globals.currentSave[2].Split(';');
            foreach (EnemyControl controller in GetTree().GetNodesInGroup("Controllers"))
            {
                controller.SetRoomCleared(Boolean.Parse(roomBools[i]));
                i++;
            }
        }
        else
        {
            foreach (EnemyControl controller in GetTree().GetNodesInGroup("Controllers"))
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
        SetRoomStatus();
        player.SetStats();
    }
    public List<bool> roomStatus()
    {
        List<bool> roomStatus = new List<bool>();
        foreach (EnemyControl controller in GetTree().GetNodesInGroup("Controllers"))
        {
            roomStatus.Add(controller.isRoomCleared());
        }
        return roomStatus;
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
