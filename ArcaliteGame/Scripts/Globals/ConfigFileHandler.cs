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
            
        }
        ApplySettings();
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

        //1 is for primary keys, 1 is for secondaries, 3 is for controller (idk how to do that one yet)
        config.SetValue("controls", "move_left", "A");
        config.SetValue("controls", "move_left-alt", "Left");
        config.SetValue("controls", "move_right", "D");
        config.SetValue("controls", "move_right-alt", "Right");
        config.SetValue("controls", "move_jump", "W");
        config.SetValue("controls", "move_jump-alt", "Up");
        config.SetValue("controls", "move_crouch", "S");
        config.SetValue("controls", "move_crouch-alt", "Down");
        config.SetValue("controls", "move_dash", "LShift");
        config.SetValue("controls", "move_dash-alt", "");
        config.SetValue("controls", "spell_oracle", "Space");
        config.SetValue("controls", "spell_oracle-alt", "");
        config.SetValue("controls", "spell_slot1", "Q");
        config.SetValue("controls", "spell_slot1-alt", "");
        config.SetValue("controls", "spell_slot2", "E");
        config.SetValue("controls", "spell_slot2-alt", "");
        config.SetValue("controls", "attack_normal", "mouse_1");
        config.SetValue("controls", "attack_normal-alt", "");
        config.SetValue("controls", "attack_charge", "mouse_2");
        config.SetValue("controls", "attack_charge-alt", "");

        config.SetValue("accessibility", "lang", "en");

        config.Save(SETTINGS_FILE_PATH);

        
    }


    public static void SaveSetting(string section)
    {
        foreach (KeyValuePair<string, Variant> item in settingChanges[section])
        {
            string key = item.Key;
            Variant value = item.Value;
            //GD.Print($"saving setting --> section: {section}, key: {key}, value: {value}");
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
        
        ResetSTChanges();
    }

    public static void ResetSTChanges()
    {
        foreach (var item in new string[5] { "game", "video", "audio", "controls", "accessibility" })
        {
            //why does a print break everything???

            /*foreach (KeyValuePair<string, Variant> setting in settingChanges[item])
            {
                GD.Print($"Setting: {item} | {setting.Key} | {setting.Value}\n" +
                    $" reverted to: {item} | {setting.Key} | {LoadSetting(item)[setting.Key]}");
            }*/
            settingChanges[item] = LoadSetting(item);
        }
    }
}
