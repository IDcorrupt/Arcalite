using Godot;
using System;
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
        if (Globals.hasSavefile)
        {
            //regular button appearance
        }
        else
        {
            //special appearance for disabled button
        }
    }

    public void ContinuePressed()
    {
        if (Globals.hasSavefile)
        {
            //save loading here
        }
        else
        {
            //nothing because no savefile
            //will delete this else case later
        }
    }
    public void NewGamePressed()
    {
        Globals.playerControl = true;
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
