using Godot;
using System;

public partial class PauseMenu : Control
{
    private Button Continue;
    private Button Settings;
    private Button Exit;
    private Button Quit;
    private Node parent;

    private bool settingsOpen;
    public override void _Ready()
    {
        Continue = GetNode<Button>("Continue");
        Settings = GetNode<Button>("Settings");
        Exit = GetNode<Button>("Exit");
        Quit = GetNode<Button>("Quit");
        parent = GetParent();
        ProcessMode = ProcessModeEnum.Always;
        Continue.Pressed += ContinuePressed;
        Settings.Pressed += SettingsPressed;
        Exit.Pressed += ExitPressed;
        Quit.Pressed += QuitPressed;

    }

    public override void _Process(double delta)
    {
        if (!settingsOpen && Input.IsActionJustPressed("ui_cancel"))
        {
            ContinuePressed();
        }
    }

    private void ContinuePressed()
    {
        Globals.gameActive = true;
        QueueFree();
        parent.GetTree().Paused = false;
    }

    private void SettingsPressed()
    {
        Node SettingsNode = PreloadRegistry.ControlNodes.submenuSettings.Instantiate();
        SettingsNode.TreeExited += settingsClosed;
        AddChild(SettingsNode);
        settingsOpen = true;
        Continue.Visible = false;
        Settings.Visible = false;
        Exit.Visible = false;
        Quit.Visible = false;
    }
    private void ExitPressed()
    {
        Globals.playerControl = false;
        GetTree().Paused = false;
        GetTree().ChangeSceneToPacked(PreloadRegistry.ControlNodes.mainScene);
    }
    private void QuitPressed()
    {
        GetTree().Quit();
    }

    private void settingsClosed()
    {
        settingsOpen = false;
        Continue.Visible = true;
        Settings.Visible = true;
        Exit.Visible = true;
        Quit.Visible = true;
    }
}
