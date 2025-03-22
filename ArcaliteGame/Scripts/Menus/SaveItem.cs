using Godot;
using System;

public partial class SaveItem : HBoxContainer
{
    Label saveId;
    Label saveName;
    Label savePlayTime;
    HBoxContainer parent;

    public int ID;
    
    public override void _Ready()
    {
        base._Ready();
        saveId = GetNode("Id") as Label;
        saveName = GetNode("Name") as Label;
        savePlayTime = GetNode("Time") as Label;
        parent = GetParent() as HBoxContainer;
        MouseFilter = MouseFilterEnum.Stop;

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
                GetTree().ChangeSceneToFile("res://Nodes/Maps/gameScene.tscn");
                Globals.playerControl = true;
            }
        }
    }
}
