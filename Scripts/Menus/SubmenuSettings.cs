using Godot;
using System;

public partial class SubmenuSettings : Node
{
    Node Parent;


    private Button Back;
    private Button Reset;
    private Button Save;

    public override void _Ready()
    {
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



    public void DiffSelect(int index)
    {
        ConfigFileHandler.SaveSetting("game", "difficulty", (index+1));
    }


    public void UpdateSelectors()
    {
        GetNode<OptionButton>("Panel/Margin/SettingsContainer/SettingTabs/Game/MarginContainer/ScrollContainer/Vbox/Difficulty/Selector").Selected = ConfigFileHandler.LoadSetting("game")["difficulty"].AsInt32() - 1;

    }

    //button controls   
    public void BackPressed()
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
    }
    public void ResetPressed()
    {
        ConfigFileHandler.DefaultSettings();
        UpdateSelectors();
    }
    public void SavePressed()
    {

    }
}
