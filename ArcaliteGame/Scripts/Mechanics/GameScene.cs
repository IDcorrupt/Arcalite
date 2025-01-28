using Godot;
using System;

public partial class GameScene : Node2D
{
    Resource cursor = ResourceLoader.Load("res://Assets/Placeholder assets/Cursors/PNG/White/crosshair124.png");
    PackedScene debugMap = (PackedScene)ResourceLoader.Load("res://Nodes/Maps/map_debug.tscn");
    PackedScene Map1 = (PackedScene)ResourceLoader.Load("res://Nodes/Maps/map_0.tscn");
    PackedScene pauseMenu = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/pause_menu.tscn");
    PackedScene UIscene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/ui.tscn");

    CanvasLayer UILayer;
    Control UInode;
    Control pauseMenuNode;
    Node2D mapNode;

    public override void _Ready()
    {
        //get special layer for UI
        UILayer = GetNode<CanvasLayer>("UILayer");
        UInode = (Control)UIscene.Instantiate();
        UILayer.AddChild(UInode);
        //add map
        mapNode = (Node2D)debugMap.Instantiate();
        Globals.activeMap = mapNode;
        AddChild(mapNode);
        //start game
        Globals.gameActive = true;
        //add custom cursor
        Input.SetCustomMouseCursor(cursor);
    }

    public override void _Process(double delta)
    {
        if (Globals.gameActive && Input.IsActionJustPressed("ui_cancel"))
        {
            //pause ingame sequences
            Globals.gameActive = false;
            //initiate pause menu
            pauseMenuNode = (Control)pauseMenu.Instantiate();
            UILayer.AddChild(pauseMenuNode);
            //hide ui
            UInode.Visible = false;
            UInode.ProcessMode = ProcessModeEnum.Disabled;
            //pause other processes
            GetTree().Paused = true;

        }
        else
        {
            GetTree().Paused = false;
            UInode.ProcessMode= ProcessModeEnum.Inherit;
            UInode.Visible = true;
        }
    }
}
