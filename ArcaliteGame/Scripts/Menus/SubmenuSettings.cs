using Godot;
using Godot.Collections;
using System;
using static Godot.HttpRequest;

public partial class SubmenuSettings : Node
{
	Node Parent;

	private int[] resXvalues = { 3840, 2560, 1920, 1366, 1280 };
	private int[] resYvalues = { 2160, 1440, 1080, 768, 720 };
	private PackedScene popupScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/popup.tscn");



    private bool isSaved = true;

	private Button Back;
	private Button Reset;
	private Button Save;


	

	public override void _Ready()
	{
		isSaved = true;
		//get buttons
		Back = GetNode<Button>("Panel/Margin/SettingsContainer/Actions/Hbox/Back");
		Reset = GetNode<Button>("Panel/Margin/SettingsContainer/Actions/Hbox/Reset");
		Save = GetNode<Button>("Panel/Margin/SettingsContainer/Actions/Hbox/Save");

		//set option selectors
		UpdateSelectors();

		//set button actions
		Back.Pressed += BackPressed;
		Reset.Pressed += ResetPressed;
		Save.Pressed += SavePressed;

		//get parent
		Parent = GetParent();


	}
	public void UpdateSelectors()
	{
		GetNode<OptionButton>("Panel/Margin/SettingsContainer/SettingTabs/Game/MarginContainer/ScrollContainer/Vbox/Difficulty/Selector").Selected = ConfigFileHandler.LoadSetting("game")["difficulty"].AsInt32() - 1;

	}



	public void DiffSelect(int index)
	{
		ConfigFileHandler.settingChanges["game"]["difficulty"] = index+1;
		isSaved = false;
	}



	//button controls   
	public void BackPressed()
	{
		if (!isSaved) {
			Control popup = (Control)popupScene.Instantiate();
			((Popup)popup).SetMessageType("nosave");
			AddChild(popup);

            if (Globals.PopupResult)
            {
                if (Parent is MainMenu parentscript)
                {
                    parentscript.submenuOpen = false;
                    QueueFree();
                }
                else
                {
                    //ERROR HANDLING TBD HERE
                    GD.Print("node path changed?");
                    GetTree().Quit();
                }
				Globals.PopupResult = false;
            }




        }
		else
		{
            if (Parent is MainMenu parentscript)
            {
                parentscript.submenuOpen = false;
                QueueFree();
            }
        }

	}



	public void ResetPressed()
	{
		ConfigFileHandler.DefaultSettings();
		UpdateSelectors();
	}
	public void SavePressed()
	{
		foreach (var item in new string[5] { "game", "video", "audio", "controls", "accessibility" })
		{
			ConfigFileHandler.SaveSetting(item);
		}
		isSaved = true;
	}
}
