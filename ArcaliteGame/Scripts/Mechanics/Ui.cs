using Godot;
using System;

public partial class Ui : Control
{
    RichTextLabel HPnum;
    RichTextLabel MPnum;

    public override void _Ready()
    {
        HPnum = GetNode<RichTextLabel>("HPNumDisplay");
        MPnum = GetNode<RichTextLabel>("MPNumDisplay");
    }

    public override void _Process(double delta)
    {
        HPnum.Text = "HP: " + Mathf.Round(Globals.Player.ActualHP); 
        MPnum.Text = "MP: " + Mathf.Round(Globals.Player.ActualMP); 
    }
}
