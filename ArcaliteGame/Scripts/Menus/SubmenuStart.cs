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

    private void ButtonControl(bool state)
    {
        if (state)
        {
            Continue.Disabled = false;
            Continue.Show();
            NewGame.Disabled = false;
            NewGame.Show();
            Back.Disabled = false;
            Back.Show();
        }
        else
        {
            Continue.Disabled = true;
            Continue.Hide();
            NewGame.Disabled = true;
            NewGame.Hide();
            Back.Disabled = true;
            Back.Hide();
        }
    }

    private void SubmenuContinueNode_MenuClosed()
    {
        ButtonControl(true);
    }
    public void ContinuePressed()
    {
        if (Globals.user.Id >= 0)
        {
            SaveLoadHandler.Load();
            SubmenuContinue submenuContinueNode = PreloadRegistry.ControlNodes.submenuContinueScene.Instantiate() as SubmenuContinue;
            submenuContinueNode.MenuClosed += SubmenuContinueNode_MenuClosed;
            ButtonControl(false);
            AddSibling(submenuContinueNode);
        }
        else
        {
            SaveLoadHandler.Load();
            Globals.playerControl = true;
            GetTree().ChangeSceneToFile("res://Nodes/Maps/gameScene.tscn");
        }
    }


    public void NewGamePressed()
    {
        NewGameLaunch newGameLaunchNode = PreloadRegistry.ControlNodes.newGameLaunchScene.Instantiate() as NewGameLaunch;
        ButtonControl(false);
        newGameLaunchNode.Cancel += NewGameLaunchNode_Cancel;
        AddChild(newGameLaunchNode);
    }

    private void NewGameLaunchNode_Cancel()
    {
        ButtonControl(true);
    }

    public void BackPressed()
    {
        if (Parent is MainMenu parentscript)
        {
            parentscript.submenuOpen = false;
            QueueFree();
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_cancel") && Back.Visible) //added back.visible as condition so submenu escape keys don't trigger this one too
            BackPressed();
    }

}
