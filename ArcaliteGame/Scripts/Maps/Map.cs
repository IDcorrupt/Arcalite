using Godot;
using System;

public partial class Map : Node2D
{
    PackedScene Player = (PackedScene)ResourceLoader.Load("res://Nodes/Game/player.tscn");
	private PackedScene pauseMenu = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/pause_menu.tscn");
    public override void _Ready()
    {
        Globals.spawnPoint = GetNode<Node2D>("SpawnPoint");
        Node2D player = (Node2D)Player.Instantiate();
        AddChild(player);

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
