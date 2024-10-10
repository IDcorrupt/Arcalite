using Godot;
using System;

public partial class MainMenu : Control
{
    private Button Start;
    private Button Settings;
    private Button Website;
    private Button Quit;

    public bool submenuOpen = false;

    private PackedScene submenuStart = (PackedScene)ResourceLoader.Load("res://Nodes/submenuStart.tscn");
    //private PackedScene submenuSettings = (PackedScene)ResourceLoader.Load("");
    //private PackedScene submenuWebsite = (PackedScene)ResourceLoader.Load("");
    //private PackedScene submenuUser = (PackedScene)ResourceLoader.Load("");

    public override void _Ready()
    {
        Start = GetNode<Button>("Start");
        Settings = GetNode<Button>("Settings");
        Website = GetNode<Button>("Website");
        Quit = GetNode<Button>("Quit");
        Start.Pressed += StartPressed;
        Settings.Pressed += SettingsPressed;
        Website.Pressed += WebsitePressed;
        Quit.Pressed += QuitPressed;
    }


    public void StartPressed()
    {
        Node startNode = submenuStart.Instantiate();
        AddChild(startNode);
        submenuOpen = true;
    }
    public void SettingsPressed()
    {
        
    }
    public void WebsitePressed()
    {
        
    }
    public void QuitPressed()
    {
        GetTree().Quit();
    }
    public override void _Process(double delta)
    {
        if (submenuOpen)
        {
            Start.Visible = false;
            Settings.Visible = false;
            Website.Visible = false;
            Quit.Visible = false;
        }
        else
        {
            Start.Visible = true;
            Settings.Visible = true;
            Website.Visible = true;
            Quit.Visible = true;
        }
        }
}
