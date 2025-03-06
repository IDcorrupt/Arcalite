using Godot;
using System;

public partial class GameScene : Node2D
{
	Resource cursor = ResourceLoader.Load("res://Assets/Placeholder assets/Cursors/PNG/White/crosshair124.png");
	PackedScene pauseMenuScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/pause_menu.tscn");
	PackedScene UIscene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/ui.tscn");
	PackedScene RespawnScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/respawn_screen.tscn");

	//MAPS
	PackedScene Map100 = (PackedScene)ResourceLoader.Load("res://Nodes/Maps/map_debug.tscn");
	PackedScene Map0 = (PackedScene)ResourceLoader.Load("res://Nodes/Maps/map_0.tscn");

	Timer deathTimer;
	bool deadtrigger = false;	//shouldn't need this, but i do because i cant think of a better idea to not start the timer every frame
	CanvasLayer UILayer;
	Control UInode;
	Control pauseMenuNode;
	Control respawnScreen;
	Node2D mapNode;

	public override void _Ready()
	{
		//get special layer for UI
		UILayer = GetNode<CanvasLayer>("UILayer");
		UInode = (Control)UIscene.Instantiate();
		UILayer.AddChild(UInode);
		
		//add map
		//mapNode = (Node2D)debugMap.Instantiate();
		mapNode = (Node2D)Map0.Instantiate();
		//save original map name to globals (name changes next line for identification
		Globals.activeMap = mapNode.Name;
		mapNode.Name = "Map";
		AddChild(mapNode);
		//start game
		Globals.gameActive = true;
		//add custom cursor
		Input.SetCustomMouseCursor(cursor);

		//respawn stuff
		deathTimer = GetNode("DeathTimer") as Timer;
	}

    private void DeathTimer_Timeout()
    {
		GD.Print("timer expired");
		respawnScreen = RespawnScene.Instantiate() as Control;
		UILayer.AddChild(respawnScreen);
    }

    public override void _Process(double delta)
	{

		if (Globals.player.GetIsDead() && !deadtrigger)
		{
			deadtrigger = true;
			deathTimer.Start();
		}
		else if (!Globals.player.GetIsDead())
		{
			deadtrigger = false;
			if (Globals.gameActive && Input.IsActionJustPressed("ui_cancel"))
			{
				//pause ingame sequences
				Globals.gameActive = false;
				//initiate pause menu
				pauseMenuNode = (Control)pauseMenuScene.Instantiate();
				UILayer.AddChild(pauseMenuNode);
				//hide ui
				UInode.Visible = false;
				UInode.ProcessMode = ProcessModeEnum.Disabled;
				//pause other processes
				GetTree().Paused = true;

			}
			else
			{
				GetTree().Paused = false;
				UInode.ProcessMode = ProcessModeEnum.Inherit;
				UInode.Visible = true;
			}
		}
	}
}
