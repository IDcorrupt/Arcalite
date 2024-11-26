using Godot;
using System;

public partial class MainNode : Node2D
{
    private PackedScene mainMenu = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/mainmenu.tscn");


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		MainMenu mainMenuNode = mainMenu.Instantiate() as MainMenu;
		mainMenuNode.Position = new Vector2(320, 180);
		AddChild(mainMenuNode);
		Input.SetCustomMouseCursor(null);
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
