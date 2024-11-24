using Godot;
using System;

public partial class GameScene : Node2D
{
    PackedScene debugMapScene = (PackedScene)ResourceLoader.Load("res://Nodes/Maps/Map.tscn");
    private PackedScene pauseMenu = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/pause_menu.tscn");


    Node2D debugMap;

    public override void _Ready()
    {
        debugMap = (Node2D)debugMapScene.Instantiate();
        AddChild(debugMap);
    }

    public override void _Process(double delta)
    {
        if (Globals.gameActive && Input.IsActionJustPressed("ui_cancel"))
        {
            Globals.gameActive = false;
            Control pauseMenuNode = (Control)pauseMenu.Instantiate();
            AddChild(pauseMenuNode);

            debugMap.Paused
            
        }
    }
}
