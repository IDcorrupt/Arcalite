using Godot;
using System;

public partial class PauseMenu : Control
{
    private Button Continue;
    private Button Settings;
    private Button Exit;
    private Button Quit;

    private bool settingsOpen;

    private PackedScene submenuSettings = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/submenuSettings.tscn");

    public override void _Ready()
    {
        Continue = GetNode<Button>("Continue");
        Settings = GetNode<Button>("Settings");
        Exit = GetNode<Button>("Exit");
        Quit = GetNode<Button>("Quit");

        Continue.Pressed += ContinuePressed;
        Settings.Pressed += SettingsPressed;
        Exit.Pressed += ExitPressed;
        Quit.Pressed += QuitPressed;

    }


    private void ContinuePressed()
    {
        Globals.gameActive = true;
        QueueFree();
    }

    private void SettingsPressed()
    {
        Node SettingsNode = submenuSettings.Instantiate();
        SettingsNode.TreeExited += settingsClosed;
        Continue.Visible = false;
        Settings.Visible = false;
        Exit.Visible = false;
        Quit.Visible = false;
    }
    private void ExitPressed()
    {
        Globals.playerControl = false;
        GetTree().ChangeSceneToFile("res://Nodes/main.tscn");
    }
    private void QuitPressed()
    {
        GetTree().Quit();
    }

    private void settingsClosed()
    {
        Continue.Visible = true;
        Settings.Visible = true;
        Exit.Visible = true;
        Quit.Visible = true;
    }
}
