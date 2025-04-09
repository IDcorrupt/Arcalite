using Godot;
using System;

public partial class GameScene : Node2D
{
	Timer deathTimer;
	bool deadtrigger = false;	//shouldn't need this, but i do because i cant think of a better idea to not restart the timer every frame
	CanvasLayer UILayer;
	Control UInode;
	Control pauseMenuNode;
	Control respawnScreen;
	Map mapNode;




	public override void _Ready()
	{
		//get special layer for UI
		UILayer = GetNode<CanvasLayer>("UILayer");
		UInode = (Control)PreloadRegistry.ControlNodes.UIscene.Instantiate();
		UILayer.AddChild(UInode);

		//add map
		if (Globals.hasSavefile)
		{
			//load runid, runname and map from save
			string mapname = Globals.currentSave[0];
			Globals.runName = Globals.currentSave[10];
			GD.Print("savefile runid: " + Globals.currentSave[9]);
			Globals.runID = Convert.ToInt32(Globals.currentSave[9]);
			Globals.playTime = (float)Convert.ToDecimal(Globals.currentSave[11]);
			//due to changes because of build incompatibility, this method is a stopgap, undoing the futureproofing that was done for multiple maps
			mapNode = PreloadRegistry.Game.Maps.Map0Scene.Instantiate() as Map;
        }
        else 
		{ 
			mapNode = PreloadRegistry.Game.Maps.Map0Scene.Instantiate() as Map;
			Globals.playTime = 0;
		}

		//save original map name to globals (name changes next line for identification)
		Globals.activeMap = mapNode.Name;
		mapNode.Name = "Map";
		AddChild(mapNode);


		//add custom cursor
		Input.SetCustomMouseCursor(PreloadRegistry.Assets.cursor);
		//respawn stuff
		deathTimer = GetNode("DeathTimer") as Timer;


		//start game
		Globals.gameActive = true;
	}

    private void DeathTimer_Timeout()
    {
		respawnScreen = PreloadRegistry.ControlNodes.RespawnScene.Instantiate() as Control;
		UILayer.AddChild(respawnScreen);
    }

    public override void _Process(double delta)
	{
		if(Globals.player != null)
		{
			if (Globals.gameActive && !Globals.player.GetIsDead() && !Globals.gameBeaten)
				Globals.playTime += (float)delta;

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
					pauseMenuNode = (Control)PreloadRegistry.ControlNodes.pauseMenuScene.Instantiate();
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
}
