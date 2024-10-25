using Godot;
using Godot.Collections;
using System;
using static Godot.HttpRequest;

public partial class SubmenuSettings : Node
{
	static Node Parent;

	private int[] resXvalues = { 3840, 2560, 1920, 1366, 1280 };
	private int[] resYvalues = { 2160, 1440, 1080, 768, 720 };
	private PackedScene popupScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/popup.tscn");



    public bool isSaved = true;

	private Button Back;
	private Button Reset;
	private Button Save;

	private HotkeyRebindButton testKeybind;
	

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

    public override void _Process(double delta)
    {
        //exit if popup returned yes
        if (Globals.PopupResult)
        {
            Exit();
        }
    }

    //updates values for setting options
    public void UpdateSelectors()
	{
		GetNode<OptionButton>("Panel/Margin/SettingsContainer/SettingTabs/Game/MarginContainer/ScrollContainer/Vbox/difficulty/Selector").Selected = ConfigFileHandler.LoadSetting("game")["difficulty"].AsInt32() - 1;
		GetNode<OptionButton>("Panel/Margin/SettingsContainer/SettingTabs/Video/MarginContainer/ScrollContainer/Vbox/windowmode/Selector").Selected = ConfigFileHandler.LoadSetting("video")["windowmode"].AsInt32() - 1;
		GetNode<OptionButton>("Panel/Margin/SettingsContainer/SettingTabs/Video/MarginContainer/ScrollContainer/Vbox/resolution/Selector").Selected = ConfigFileHandler.LoadSetting("video")["resolutionX"].AsInt32() - 1;
		GetNode<BaseButton>("Panel/Margin/SettingsContainer/SettingTabs/Video/MarginContainer/ScrollContainer/Vbox/vsync/CheckButton").ButtonPressed = ConfigFileHandler.LoadSetting("video")["vsync"].AsBool();
		GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/master_volume/HSlider").Value = ConfigFileHandler.LoadSetting("audio")["master_volume"].AsInt32();
		GetNode<SpinBox>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/master_volume/SpinBox").Value = ConfigFileHandler.LoadSetting("audio")["master_volume"].AsInt32();
		GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/music_volume/HSlider").Value = ConfigFileHandler.LoadSetting("audio")["music_volume"].AsInt32();
		GetNode<SpinBox>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/music_volume/SpinBox").Value = ConfigFileHandler.LoadSetting("audio")["music_volume"].AsInt32();
		GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/sfx_volume/HSlider").Value = ConfigFileHandler.LoadSetting("audio")["sfx_volume"].AsInt32();
		GetNode<SpinBox>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/sfx_volume/SpinBox").Value = ConfigFileHandler.LoadSetting("audio")["sfx_volume"].AsInt32();
    }


    //setting change handlers
    //game
    public void DiffSelect(int index)
	{
		ConfigFileHandler.settingChanges["game"]["difficulty"] = index+1;
		isSaved = false;
	}

	//video
	public void WindowSelect(int index)
	{
		ConfigFileHandler.settingChanges["video"]["windowmode"] = index + 1;
		isSaved = false;

    }
    public void ResSelect(int index)
	{
		switch (index)
		{
			case 0:
				ConfigFileHandler.settingChanges["video"]["resolutionX"] = 1;
				ConfigFileHandler.settingChanges["video"]["resolutionY"] = 1;
				isSaved = false;

                break;
			case 1:
                ConfigFileHandler.settingChanges["video"]["resolutionX"] = 2;
                ConfigFileHandler.settingChanges["video"]["resolutionY"] = 2;
                isSaved = false;
                break;
			case 2:
                ConfigFileHandler.settingChanges["video"]["resolutionX"] = 3;
                ConfigFileHandler.settingChanges["video"]["resolutionY"] = 3;
                isSaved = false;
                break;
			case 3:
                ConfigFileHandler.settingChanges["video"]["resolutionX"] = 4;
                ConfigFileHandler.settingChanges["video"]["resolutionY"] = 4;
                isSaved = false;
                break;
			case 4:
                ConfigFileHandler.settingChanges["video"]["resolutionX"] = 5;
                ConfigFileHandler.settingChanges["video"]["resolutionY"] = 5;
                isSaved = false;
                break;

			default:
				break;
		}
	}
	public void VsyncToggle(bool toggled)
	{
		ConfigFileHandler.settingChanges["video"]["vsync"] = toggled;
		if (toggled != ConfigFileHandler.LoadSetting("video")["vsync"].AsBool())
		{
			isSaved = false;
		}
    }

	//audio
	public void MasterVolumeSlideEnded(bool valueChanged)
	{
		if (valueChanged)
		{
			ConfigFileHandler.settingChanges["audio"]["master_volume"] = GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/master_volume/HSlider").Value;
			isSaved = false;
        }
	}
	public void MasterVolumeChanged(float value)
	{
		ConfigFileHandler.settingChanges["audio"]["master_volume"] = value;

		//sync slider to spinbox
		GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/master_volume/HSlider").Value = value;
		if(value != ConfigFileHandler.LoadSetting("audio")["master_volume"].AsInt32())
		{
			isSaved = false;
		}
    }
    public void MusicVolumeSlideEnded(bool valueChanged)
    {
        if (valueChanged)
        {
            ConfigFileHandler.settingChanges["audio"]["music_volume"] = GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/music_volume/HSlider").Value;
            isSaved = false;
        }
    }
    public void MusicVolumeChanged(float value)
    {
        ConfigFileHandler.settingChanges["audio"]["music_volume"] = value;

        //sync slider to spinbox
        GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/music_volume/HSlider").Value = value;
		if(value != ConfigFileHandler.LoadSetting("audio")["music_volume"].AsInt32())
		{
			isSaved = false;
		}
    }
    public void SFXVolumeSlideEnded(bool valueChanged)
    {
        if (valueChanged)
        {
            ConfigFileHandler.settingChanges["audio"]["sfx_volume"] = GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/sfx_volume/HSlider").Value;
            isSaved = false;
        }
    }
    public void SFXVolumeChanged(float value)
    {
        ConfigFileHandler.settingChanges["audio"]["sfx_volume"] = value;

        //sync slider to spinbox
        GetNode<Slider>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/sfx_volume/HSlider").Value = value;
		if(value != ConfigFileHandler.LoadSetting("audio")["sfx_volume"].AsInt32())
		{
			isSaved = false;
		}
    }

    //sync spinbox with slider
    public void MasterVolumeSliding(float value)
	{
		GetNode<SpinBox>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/master_volume/SpinBox").Value = value;
	}
    public void MusicVolumeSliding(float value)
    {
        GetNode<SpinBox>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/music_volume/SpinBox").Value = value;
    }
    public void SFXVolumeSliding(float value)
    {
        GetNode<SpinBox>("Panel/Margin/SettingsContainer/SettingTabs/Audio/MarginContainer/ScrollContainer/Vbox/sfx_volume/SpinBox").Value = value;
    }



    //button controls   
    public void BackPressed()
	{
		//check if settings were actually changed
		if(ConfigFileHandler.checkSettings()) isSaved = true;
		if (!isSaved) {

			//if changes are not saved
			Control popup = (Control)popupScene.Instantiate();
			((Popup)popup).SetMessageType("nosave");
			AddChild(popup);
			//popup will initiate exit() func if promted
        }
		else
		{
			//if changes are saved
			Exit();
        }

	}
	public void ResetPressed()
	{
		ConfigFileHandler.DefaultSettings();
		UpdateSelectors();
	}
	public void SavePressed()
	{
		ConfigFileHandler.SaveSettings();
		isSaved = true;
	}
	
	//exit func for nosave exit
	public void Exit()
	{
        ConfigFileHandler.ResetSTChanges();
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
