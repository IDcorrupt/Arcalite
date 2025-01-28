using Godot;
using System;

public partial class Ui : Control
{
    RichTextLabel HPnum;
    RichTextLabel MPnum;

    //for debug
    Button debugButton;

    public override void _Ready()
    {
        HPnum = GetNode<RichTextLabel>("HPNumDisplay");
        MPnum = GetNode<RichTextLabel>("MPNumDisplay");
        debugButton = GetNode<Button>("debugbutton");
        
    }

    public void debugToggled(bool toggled)
    {
        
    }

    public override void _Process(double delta)
    {
        HPnum.Text = "HP: " + Mathf.Round(Globals.Player.ActualHP); 
        MPnum.Text = "MP: " + Mathf.Round(Globals.Player.ActualMP); 
    }
}
