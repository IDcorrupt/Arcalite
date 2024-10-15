using Godot;
using System;
using System.Collections.Generic;

public partial class ConfigFileHandler : Node
{
    static ConfigFile config = new ConfigFile();
    const string SETTINGS_FILE_PATH = "res://settings.ini";
    private int[] resXvalues = { 3840, 2560, 1920, 1366, 1280 };
    private int[] resYvalues = { 2160, 1440, 1080, 768, 720 };


    public override void _Ready()
    {
        if (!FileAccess.FileExists(SETTINGS_FILE_PATH))
        {
            DefaultSettings();
        }
        else
        {
            config.Load(SETTINGS_FILE_PATH);
        }

    }

    public static void DefaultSettings()
    {
        config.SetValue("game", "difficulty", "2"); //easy = 1 | normal = 2 | hard = 3

        config.SetValue("controls", "game_left", "A");
        config.SetValue("controls", "game_left", "Arrow_left");
        config.SetValue("controls", "game_right", "D");
        config.SetValue("controls", "game_right", "Arrow_right");
        config.SetValue("controls", "game_jump", "W");
        config.SetValue("controls", "game_jump", "Arrow_up");
        config.SetValue("controls", "game_crouch", "S");
        config.SetValue("controls", "game_crouch", "Arrow_down");
        config.SetValue("controls", "game_dash", "LShift");
        config.SetValue("controls", "game_oracle", "Space");
        config.SetValue("controls", "game_spellslot1", "Q");
        config.SetValue("controls", "game_spellslot2", "E");
        config.SetValue("controls", "Primary_attack", "mouse_1");
        config.SetValue("controls", "Charge_attack", "mouse_2");


        config.SetValue("video", "windowmode", "windowed"); //windowed | borderless | exclusive
        config.SetValue("video", "resolutionX", 1280);
        config.SetValue("video", "resolutionY", 720);
        config.SetValue("video", "Vsync", true);


        config.SetValue("audio", "master_volume", 50);
        config.SetValue("audio", "music_volume", 100);
        config.SetValue("audio", "sfx_volume", 100);

        config.Save(SETTINGS_FILE_PATH);
    }


    public static void SaveSetting(string section, string key, Variant value)
    {
        GD.Print($"saving setting --> section: {section}, key: {key}, value: {value}");
        config.SetValue(section, key, value);
        config.Save(SETTINGS_FILE_PATH);
    }
    public static Dictionary<string,Variant> LoadSetting(string section)
    {
        Dictionary<string, Variant> settings = new Dictionary<string, Variant>();
        foreach (var key in config.GetSectionKeys(section))
        {
            settings[key] = config.GetValue(section, key);
        }
        return settings;
    }


}
