using Godot;
using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

public partial class SubmenuStart : Control
{
    Node Parent;
    private Button Continue;
    private Button NewGame;
    private Button Back;
    public override void _Ready()
    {
        Continue = GetNode<Button>("Continue");
        NewGame= GetNode<Button>("NewGame");
        Back = GetNode<Button>("Back");
        Continue.Pressed += ContinuePressed;
        NewGame.Pressed += NewGamePressed;
        Back.Pressed += BackPressed;
        Parent = GetParent();

        Globals.hasSavefile = SaveLoadHandler.CheckSave();
        if(Globals.hasSavefile) 
            Continue.Disabled = false;
        else Continue.Disabled = true;
    }

    public void ContinuePressed()
    {
        SaveLoadHandler.Load();
        Globals.playerControl = true;
        GetTree().ChangeSceneToFile("res://Nodes/Maps/gameScene.tscn");
    }
    public void NewGamePressed()
    {
        Globals.playerControl = true;
        Globals.hasSavefile = false;
        GetTree().ChangeSceneToFile("res://Nodes/Maps/gameScene.tscn");
    }
    public void BackPressed()
    {
        if (Parent is MainMenu parentscript)
        {
            parentscript.submenuOpen = false;
            QueueFree();
        }
    }

}
