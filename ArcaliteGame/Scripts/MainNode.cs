using Godot;
using System;

public partial class MainNode : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Globals.invalidSettings)
		{
            Control popup = (Control)Globals.popupScene.Instantiate();
            ((Popup)popup).SetMessageType("invalidSettings");
            AddChild(popup);
			ConfigFileHandler.DefaultSettings();
        }
	}
}
