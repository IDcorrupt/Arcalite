using Godot;
using System;

public partial class GameScene : Node2D
{
    Resource cursor = ResourceLoader.Load("res://Assets/Placeholder assets/Cursors/PNG/White/crosshair124.png");
    PackedScene debugMap = (PackedScene)ResourceLoader.Load("res://Nodes/Maps/Map.tscn");
    PackedScene pauseMenu = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/pause_menu.tscn");
    PackedScene UIscene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/ui.tscn");

    Node2D debugMapNode;
    Control UInode;

    public override void _Ready()
    {
        Input.SetCustomMouseCursor(cursor);
        Globals.gameActive = true;
        debugMapNode = (Node2D)debugMap.Instantiate();
        UInode = (Control)UIscene.Instantiate();
        Globals.activeMap = debugMapNode;
        AddChild(debugMapNode);
        AddChild(UInode);
    }

    public override void _Process(double delta)
    {
        if (Globals.gameActive && Input.IsActionJustPressed("ui_cancel"))
        {
            Globals.gameActive = false;
            Control pauseMenuNode = (Control)pauseMenu.Instantiate();
            AddChild(pauseMenuNode);
            GetTree().Paused = true;
            
        }else GetTree().Paused = false;
    }
}