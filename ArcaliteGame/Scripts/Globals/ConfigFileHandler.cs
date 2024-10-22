using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;

public partial class ConfigFileHandler : Node
{
    static ConfigFile config = new ConfigFile();
    const string SETTINGS_FILE_PATH = "res://settings.ini";
    public static Dictionary<string, Dictionary<string, Variant>> settingChanges = new Dictionary<string, Dictionary<string, Variant>>();


    public override void _Ready()
    {
        if (!FileAccess.FileExists(SETTINGS_FILE_PATH))
        {
            DefaultSettings();
        }
        else
        {
            config.Load(SETTINGS_FILE_PATH);
            foreach (var item in new string[5] {"game", "video", "audio", "controls", "accessibility"})
            {
                settingChanges[item] = LoadSetting(item);
            }
        }

    }

    public static void DefaultSettings()
    {
        config.SetValue("game", "difficulty", 2); //easy = 1 | normal = 2 | hard = 3

        config.SetValue("video", "windowmode", 1); //windowed = 1 | borderless = 2 | exclusive = 3
        config.SetValue("video", "resolutionX", 4); //4k = 1 | 2k = 2 | 1080p = 3 | 720p = 4 | 360p(source res) = 5
        config.SetValue("video", "resolutionY", 4);
        config.SetValue("video", "vsync", true);


        config.SetValue("audio", "master_volume", 50);
        config.SetValue("audio", "music_volume", 100);
        config.SetValue("audio", "sfx_volume", 100);

        config.SetValue("controls", "game_left", "A|Left");
        config.SetValue("controls", "game_right", "D|Right");
        config.SetValue("controls", "game_jump", "W|Up");
        config.SetValue("controls", "game_crouch", "S|Down");
        config.SetValue("controls", "game_dash", "LShift|");
        config.SetValue("controls", "game_oracle", "Space|");
        config.SetValue("controls", "game_spellslot1", "Q|");
        config.SetValue("controls", "game_spellslot2", "E|");
        config.SetValue("controls", "primary_attack", "mouse_1|");
        config.SetValue("controls", "charge_attack", "mouse_2|");

        config.SetValue("accessibility", "lang", "en");

        config.Save(SETTINGS_FILE_PATH);

        foreach (var item in new string[5] { "game", "video", "audio", "controls", "accessibility" })
        {
            settingChanges[item] = LoadSetting(item);
        }
    }


    public static void SaveSetting(string section)
    {
        foreach (KeyValuePair<string, Variant> item in settingChanges[section])
        {
            string key = item.Key;
            Variant value = item.Value;
            GD.Print($"saving setting --> section: {section}, key: {key}, value: {value}");
            config.SetValue(section, key, value);
            
        }
        config.Save(SETTINGS_FILE_PATH);
        ApplySettings();
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

    public static bool checkSettings()
    {
        bool check = true;
        foreach (var item in new string[5] { "game", "video", "audio", "controls", "accessibility" })
        {
            foreach (KeyValuePair<string,Variant> setting in settingChanges[item])
            {
                if(setting.Value.ToString() != LoadSetting(item)[setting.Key].ToString())
                {
                    check = false;
                }
            }
        }


        return check;
    }


    private static void ApplySettings()
    {

    }
}
