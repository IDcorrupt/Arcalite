using Godot;
using System;

public partial class SaveItem : Panel
{
	Label saveId;
	Label saveName;
	Label savePlayTime;
	VBoxContainer parent;


	public int ID;
	
	public override void _Ready()
	{
		base._Ready();
		
		saveId = GetNode("HBox/Id") as Label;
		saveName = GetNode("HBox/Name") as Label;
		savePlayTime = GetNode("HBox/Time") as Label;
		parent = GetParent() as VBoxContainer;
		MouseFilter = MouseFilterEnum.Stop;

		StyleBoxFlat stylebox = new StyleBoxFlat();
		stylebox.BgColor = Color.FromHsv(0, 0, 0, 0);
		AddThemeStyleboxOverride("panel", stylebox);

		if (ID >= 0)
		{
			//database save
			saveId.Text = (ID + 1).ToString();
			saveName.Text = Globals.user.Characters[ID].Name;
			int minutes = Mathf.FloorToInt((float)Globals.user.Characters[ID].Playtime.TotalSeconds / 60);
			int seconds = Mathf.FloorToInt((float)Globals.user.Characters[ID].Playtime.TotalSeconds) - minutes * 60;
			savePlayTime.Text = $"Playtime: {minutes.ToString("D2")}:{seconds.ToString("D2")}";

		}
		else
		{
			//local save
			saveId.Text = "local";
			saveName.Text = Globals.currentSave[10];
			int minutes = Mathf.FloorToInt((float)Convert.ToDecimal(Globals.currentSave[11]) / 60);
			int seconds = Mathf.FloorToInt((float)Convert.ToDecimal(Globals.currentSave[11])) - minutes*60;
			savePlayTime.Text = $"Playtime: {minutes.ToString("D2")}:{seconds.ToString("D2")}";
		}
	}

	//hover visua
	public void OnMouseEntered()
	{
		RemoveThemeStyleboxOverride("panel");
		StyleBoxFlat stylebox = new StyleBoxFlat();
		stylebox.BgColor = Color.FromHsv(0, 0, 0, 0.2f);
		AddThemeStyleboxOverride("panel", stylebox);
	}
	public void OnMouseExited()
	{
		RemoveThemeStyleboxOverride("panel");
		StyleBoxFlat stylebox = new StyleBoxFlat();
		stylebox.BgColor = Color.FromHsv(0, 0, 0, 0);
		AddThemeStyleboxOverride("panel", stylebox);
	}

	public override void _GuiInput(InputEvent @event)
	{
		if(@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				if (ID >= 0)
				{
					//LOAD SPECIFIED CHARACTER TO SAVEFILE
					SaveLoadHandler.Load(Globals.user.Characters[ID].Save);
					GD.Print(Globals.user.Characters[ID].Save);
				}
				//enter game
				GetTree().ChangeSceneToPacked(PreloadRegistry.Game.Maps.gameScene);
				Globals.playerControl = true;
			}
		}
	}
}
