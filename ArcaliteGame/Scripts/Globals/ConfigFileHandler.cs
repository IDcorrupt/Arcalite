using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class ConfigFileHandler : Node
{
    static ConfigFile config = new ConfigFile();
    const string SETTINGS_FILE_PATH = "res://settings.ini";
    public static Dictionary<string, Dictionary<string, Variant>> settingChanges = new Dictionary<string, Dictionary<string, Variant>>();

    static int[] resXvalues = { 3840, 2560, 1920, 1280, 640 };
    static int[] resYvalues = { 2160, 1440, 1080, 720, 360 };
    static Window window;

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
        ResetSTChanges();
        ApplySettings();
        window = GetWindow();
    }

    public static void DefaultSettings()
    {
        config.SetValue("game", "difficulty", 2); //easy = 1 | normal = 2 | hard = 3

        config.SetValue("video", "windowmode", 1); //windowed = 1 | borderless = 2 | exclusive = 3
        config.SetValue("video", "resolutionX", 5); //4k = 1 | 2k = 2 | 1080p = 3 | 720p = 4 | 360p(source res) = 5
        config.SetValue("video", "resolutionY", 5);
        config.SetValue("video", "vsync", true);


        config.SetValue("audio", "master_volume", 50);
        config.SetValue("audio", "music_volume", 100);
        config.SetValue("audio", "sfx_volume", 100);

        //normal is primary keybind, -alt is secondary, and idk how to do controller yet
        config.SetValue("controls", "move_left", "A");
        config.SetValue("controls", "move_left-alt", "Left");
        config.SetValue("controls", "move_right", "D");
        config.SetValue("controls", "move_right-alt", "Right");
        config.SetValue("controls", "move_jump", "W");
        config.SetValue("controls", "move_jump-alt", "Up");
        config.SetValue("controls", "move_crouch", "S");
        config.SetValue("controls", "move_crouch-alt", "Down");
        config.SetValue("controls", "move_dash", "Shift");
        config.SetValue("controls", "move_dash-alt", "");
        config.SetValue("controls", "spell_oracle", "Space");
        config.SetValue("controls", "spell_oracle-alt", "");
        config.SetValue("controls", "spell_slot1", "Q");
        config.SetValue("controls", "spell_slot1-alt", "");
        config.SetValue("controls", "spell_slot2", "E");
        config.SetValue("controls", "spell_slot2-alt", "");
        config.SetValue("controls", "attack_normal", "MouseLeft");
        config.SetValue("controls", "attack_normal-alt", "");
        config.SetValue("controls", "attack_charge", "MouseRight");
        config.SetValue("controls", "attack_charge-alt", "");

        config.SetValue("accessibility", "lang", "en");

        config.Save(SETTINGS_FILE_PATH);

        Globals.invalidSettings = false;
    }


    public static void SaveSettings()
    {
        foreach (var section in new string[5] { "game", "video", "audio", "controls", "accessibility" })
        { 
            foreach (KeyValuePair<string, Variant> item in settingChanges[section])
            {
                string key = item.Key;
                Variant value = item.Value;
                config.SetValue(section, key, value);
            }
        }
        config.Save(SETTINGS_FILE_PATH);
        ApplySettings();
        ResetSTChanges();
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
        try
        {
            //game
            Globals.Difficulty = (int)settingChanges["game"]["difficulty"];

            //video
            int winmode = (int)settingChanges["video"]["windowmode"];
            switch (winmode)
            {
                case 1:

                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                    break;
                case 2:
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);

                    break;
                case 3:
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                    break;

            }
            int resSize = (int)settingChanges["video"]["resolutionX"];
            
            DisplayServer.WindowSetSize(new Vector2I(resXvalues[resSize - 1], resYvalues[resSize - 1]));
            if ((bool)settingChanges["video"]["vsync"])
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
            else
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
            //reposition screen: -- doesn't work
            //var centerScreen = DisplayServer.ScreenGetPosition() + DisplayServer.ScreenGetSize()/2;
            //var windowSize = window.GetSizeWithDecorations();
            //window.Position = centerScreen-windowSize/2;
            
            //audio
            //i'Ll do this when we have audio :3

            //controls
            foreach (KeyValuePair<string, Variant> setting in settingChanges["controls"])
            {
                string keyString = LoadSetting("controls")[setting.Key].ToString();
                if (keyString.Length > 0)
                {
                    if (keyString.Contains("Mouse"))
                    {
                        string MouseString = keyString.Replace("Mouse", "");
                        InputEventMouseButton inputeventButton = new InputEventMouseButton()
                        {
                            ButtonIndex = (MouseButton)Enum.Parse(typeof(MouseButton), MouseString, true),
                            ShiftPressed = false,
                            CtrlPressed = false,
                            AltPressed = false,
                            MetaPressed = false,
                            Pressed = true,
                        };
                    }
                    else
                    {
                        InputEventKey inputEventKey = new InputEventKey()
                        {
                            AltPressed = false,
                            ShiftPressed = false,
                            CtrlPressed = false,
                            MetaPressed = false,
                            Pressed = true,
                        };
                        InputMap.ActionGetEvents(setting.Key);
                        if (keyString.Length == 1)
                        {
                            //if keycode is single length (normal letters)
                            Key key = (Key)(char)keyString[0];
                            inputEventKey.Keycode = key;
                        }
                        else
                        {
                            //if keycode is longer than 1 (space, shift, enter)
                            Key key = (Key)Enum.Parse(typeof(Key), keyString, true);
                            inputEventKey.Keycode = key;
                        }
                    }

                

                }

            }

            //accessibility
            //say what now?
        }
        catch (Exception ex)
        {
            GD.PrintErr(ex);
            Globals.invalidSettings = true;
        }
    }

    public static void ResetSTChanges()
    {
        foreach (var item in new string[5] { "game", "video", "audio", "controls", "accessibility" })
        {
            settingChanges[item] = LoadSetting(item);
        }
    }
}
