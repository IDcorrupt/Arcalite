using Godot;
using System;
using System.Collections.Generic;

public partial class ConfigFileHandler : Node
{
    ConfigFile config = new ConfigFile();
    const string SETTINGS_FILE_PATH = "res://settings.ini";


    public override void _Ready()
    {
        if (!FileAccess.FileExists(SETTINGS_FILE_PATH))
        {
            config.SetValue("controls", "game_left", "A");
            config.SetValue("controls", "game_left", "Arrow_left");
            config.SetValue("controls", "game_right", "D");
            config.SetValue("controls", "game_right", "Arrow_right");
            config.SetValue("controls", "game_jump", "W");
            config.SetValue("controls", "game_jump", "Arrow_up");
            config.SetValue("controls", "game_crouch", "S");
            config.SetValue("controls", "game_crouch", "Arrow_down");

            config.SetValue("video", "fullscreen", true);
            config.SetValue("video", "resolution", 720);

            config.SetValue("audio", "master_volume", 1.0);
            config.SetValue("audio", "sfx_volume", 1.0);

            config.Save(SETTINGS_FILE_PATH);

        }
        else
        {
            config.Load(SETTINGS_FILE_PATH);
        }

    }
    public void SaveSetting(string section, string key, Variant value)
    {
        config.SetValue(section, key, value);
        config.Save(SETTINGS_FILE_PATH);
    }
    public Dictionary<string,Variant> LoadSetting(string section)
    {
        Dictionary<string, Variant> settings = new Dictionary<string, Variant>();
        foreach (var key in config.GetSectionKeys(section))
        {
            settings[key] = config.GetValue(section, key);
        }
        return settings;
    }


}
