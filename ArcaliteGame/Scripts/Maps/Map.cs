using Godot;
using System;

public partial class Map : Node2D
{
    PackedScene Player = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
    GameScene parent;

    public Camera2D camera;
    public override void _Ready()
    {
        parent = GetParent() as GameScene;
        Globals.spawnPoint = GetNode<Node2D>("SpawnPoint");
        //camera = GetNode<Camera2D>("Camera2D");
        Node2D player = (Node2D)Player.Instantiate();
        AddChild(player);

    }

    public void CameraController(string direction)
    {
        /*GD.Print("moving: " + direction);
        switch (direction)
        {
            case "top":
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 360);
                break;
            case "right":
                camera.Position = new Vector2(camera.Position.X + 640, camera.Position.Y);
                break;
            case "bottom":
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 360);
                break;
            case "left":
                camera.Position = new Vector2(camera.Position.X - 640, camera.Position.Y);
                break;
            default:
                break;
        }*/
    }


    public override void _Process(double delta)
    {

    }

    
}
