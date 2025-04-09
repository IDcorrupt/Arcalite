using Godot;
using System;

public partial class MainNode : Node2D
{
    Control loadingScreen;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		loadingScreen = PreloadRegistry.ControlNodes.loadingScreenScene.Instantiate() as Control;
		AddChild(loadingScreen);
		Input.SetCustomMouseCursor(null);
	}
	public void LoadingFinished()
	{
        MainMenu mainMenuNode = PreloadRegistry.ControlNodes.mainMenu.Instantiate() as MainMenu;
        loadingScreen.QueueFree();
        AddChild(mainMenuNode);

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Globals.invalidSettings)
		{
            Control popup = (Control)PreloadRegistry.ControlNodes.popupScene.Instantiate();
            ((Popup)popup).SetMessageType("invalidSettings");
            AddChild(popup);
			ConfigFileHandler.DefaultSettings();
        }
	}
}
