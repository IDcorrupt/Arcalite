using Godot;
using System;

public partial class MainMenu : Control
{
    private Button Start;
    private Button Settings;
    private Button Website;
    private Button Quit;


    public override void _Ready()
    {
        Start = GetNode<Button>("/Start");
        Settings = GetNode<Button>("/Settings");
        Website = GetNode<Button>("/Website");
        Quit = GetNode<Button>("/Quit");
        
    }


    public void StartPressed()
    {
        
        GD.Print("kurwa");
    }
    public override void _Process(double delta)
    {
        

    }
}
