using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

public partial class ConfigFileHandler : Node
{
    static ConfigFile config = new ConfigFile();
    const string SETTINGS_FILE_PATH = "user://settings.ini";
    public static Dictionary<string, Dictionary<string, Variant>> settingChanges = new Dictionary<string, Dictionary<string, Variant>>();

    static int[] resXvalues = { 3840, 2560, 1920, 1280, 640 };
    static int[] resYvalues = { 2160, 1440, 1080, 720, 360 };
    static int screenId;
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
        window = GetWindow();
        ResetSTChanges();
        ApplySettings();            
    }

    public static void DefaultSettings()
    {
        config.SetValue("game", "difficulty", 1);   //easy = 0 | normal = 1 | hard = 2
        config.SetValue("game", "dashmode", 0);     //8dir = 0 | mouse = 1

        config.SetValue("video", "windowmode", 0); //windowed = 0 | borderless = 1 | exclusive = 2
        config.SetValue("video", "resolutionX", 4); //4k = 0 | 2k = 1 | 1080p = 2 | 720p = 3 | 360p(native) = 4
        config.SetValue("video", "resolutionY", 4);
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
        config.SetValue("controls", "spell_slot1", "E");
        config.SetValue("controls", "spell_slot1-alt", "");
        config.SetValue("controls", "spell_slot2", "Q");
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
            //difficulty
            Globals.Difficulty = (int)settingChanges["game"]["difficulty"];
            //dash mode
            Globals.DashMode = (int)settingChanges["game"]["dashmode"];

        //video
            //window mode
            int winmode = (int)settingChanges["video"]["windowmode"];
            switch (winmode)
            {
                case 0:

                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, false);
                    break;
                case 1:
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
                    DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);

                    break;
            }
            //resolution
            int resSize = (int)settingChanges["video"]["resolutionX"];
            //check if res setting exceeds user's screen -> if so, reset to highest value
            bool resMod = false;
            while (resXvalues[resSize] > DisplayServer.ScreenGetSize(screenId).X)
            {
                resMod = true;
                resSize++;
            }
            if (resMod)
            {
                config.SetValue("video", "resolutionX", resSize);
                config.SetValue("video", "resolutionY", resSize);
                config.Save(SETTINGS_FILE_PATH);
            }
            DisplayServer.WindowSetSize(new Vector2I(resXvalues[resSize], resYvalues[resSize]));
            
            //vsync
            if ((bool)settingChanges["video"]["vsync"])
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
            else
                DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);

            //reposition screen
            CenterWindowOnScreen(GetScreenUnderMouse());

        //audio
            //[NOT YET IMPLEMENTED]

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
            //[NOT YET IMPLEMENTED]
        }
        catch (Exception ex)
        {
            GD.PrintErr(ex);
            Globals.invalidSettings = true;
        }
    }

    static int GetScreenUnderMouse()
    {
        Vector2I mousePosition = DisplayServer.MouseGetPosition();
        int screenCount = DisplayServer.GetScreenCount();

        for (int i = 0; i < screenCount; i++)
        {
            Vector2I screenPosition = DisplayServer.ScreenGetPosition(i);
            Vector2I screenSize = DisplayServer.ScreenGetSize(i);
            Rect2I screenRect = new Rect2I(screenPosition, screenSize);

            if (screenRect.HasPoint(mousePosition))
            {
                return i;
            }
        }

        // Fallback to primary screen if mouse position doesn't match any screen
        return DisplayServer.GetPrimaryScreen();
    }

    static void CenterWindowOnScreen(int screenIndex)
    {
        Vector2I screenPosition = DisplayServer.ScreenGetPosition(screenIndex);
        Vector2I screenSize = DisplayServer.ScreenGetSize(screenIndex);
        Vector2I windowSize = window.GetSizeWithDecorations();

        Vector2I newPosition = screenPosition + (screenSize - windowSize) / 2;
        window.Position = newPosition;
    }




    public static void ResetSTChanges()
    {
        foreach (var item in new string[5] { "game", "video", "audio", "controls", "accessibility" })
        {
            settingChanges[item] = LoadSetting(item);
        }
    }

}
