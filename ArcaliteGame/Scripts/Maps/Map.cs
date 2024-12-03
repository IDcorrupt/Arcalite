using Godot;
using System;

public partial class Map : Node2D
{
    PackedScene Player = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
	PackedScene pauseMenu = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/pause_menu.tscn");
    GameScene parent;
    Ui UI;

    public Camera2D camera;
    public override void _Ready()
    {
        parent = GetParent() as GameScene;
        UI = GetNode<Control>("../UI") as Ui;   
        Globals.spawnPoint = GetNode<Node2D>("SpawnPoint");
        camera = GetNode<Camera2D>("Camera2D");
        Node2D player = (Node2D)Player.Instantiate();
        AddChild(player);

    }

    public void CameraController(string direction)
    {
        GD.Print("moving: " + direction);
        switch (direction)
        {
            case "up":
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 180);
                UI.Position = new Vector2(UI.Position.X, UI.Position.Y - 180);
                break;
            case "right":
                camera.Position = new Vector2(camera.Position.X + 640, camera.Position.Y);
                UI.Position = new Vector2(UI.Position.X + 640, UI.Position.Y);
                break;
            case "down":
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 180);
                UI.Position = new Vector2(UI.Position.X, UI.Position.Y + 180);
                break;
            case "left":
                camera.Position = new Vector2(camera.Position.X - 640, camera.Position.Y);
                UI.Position = new Vector2(UI.Position.X - 640, UI.Position.Y);
                break;
            default:
                break;
        }
    }


    public override void _Process(double delta)
    {
        if (Globals.gameActive && Input.IsActionJustPressed("ui_cancel"))
        {
            Globals.gameActive = false;
            Control pauseMenuNode = (Control)pauseMenu.Instantiate();
            AddChild(pauseMenuNode);
        }
    }

    
}
