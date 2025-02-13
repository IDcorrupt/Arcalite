using Godot;
using System;

public partial class RespawnScreen : Control
{
    private Button respawn;
    private Button mainMenu;
    private Button quit;
    private Map map;
    public override void _Ready()
    {
        base._Ready();

        map = GetParent().GetParent().GetNode("Map") as Map;
        
        respawn = GetNode("Respawn") as Button;
        mainMenu = GetNode("MainMenu") as Button;
        quit = GetNode("Quit") as Button;
        respawn.Pressed += Respawn_Pressed;
        mainMenu.Pressed += MainMenu_Pressed;
        quit.Pressed += Quit_Pressed;
    }

    private void Quit_Pressed()
    {
        GetTree().Quit();
    }

    private void MainMenu_Pressed()
    {
        Globals.playerControl = false;
        GetTree().ChangeSceneToFile("res://Nodes/main.tscn");
    }

    private void Respawn_Pressed()
    {
        map.Respawn();
        QueueFree();
    }
}
