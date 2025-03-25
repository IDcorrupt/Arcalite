using Godot;
using System;

public partial class NewGameLaunch : Control
{
    LineEdit saveName;
    Button Start;

    [Signal] public delegate void CancelEventHandler();
    public override void _Ready()
    {
        base._Ready();
        saveName = GetNode("Panel/Name") as LineEdit;
        Start = GetNode("Panel/Start") as Button;
        Start.Pressed += Start_Pressed;
    }

    private void Start_Pressed()
    {
        Globals.runName = saveName.Text.Length > 0 ? saveName.Text : "New save";
        Globals.playerControl = true;
        Globals.hasSavefile = false;
        GetTree().ChangeSceneToFile("res://Nodes/Maps/gameScene.tscn");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            EmitSignal(SignalName.Cancel);
            QueueFree();
        }
    }
}
