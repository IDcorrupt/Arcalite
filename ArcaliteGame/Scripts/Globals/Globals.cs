using Godot;
using System;
using System.Collections.Generic;

public partial class Globals : Node
{
    //mechanics
    //sets whether user can control the player character
    public static bool playerControl = false;
    //bool for checking if user has an active savefile
    public static bool hasSavefile = false;
    public static List<string> currentSave = new List<string>();
    //name of currently active game run
    public static string runName = "";
    public static int runID = -1;
    //needed for pause menu
    public static bool gameActive = false;
    public const float GRAVITY = 1500f;

    //active stuff
    public static Checkpoint spawnPoint = null;
    public static string activeMap = null;
    //Godot has no built-in stopwatch, so i have to use this
    public static float playTime = 0;
    //used for stopping time after final boss was beaten
    public static bool gameBeaten = false;

    //player cuz i need it everywhere
    public static Player player = null;


    //settings
    public static int Difficulty = 2;
    public static int DashMode = 0;
    public static float[] diffMultipliers = { 1, 1.5f, 2f };
    public static bool PopupResult = false;
    public static bool PopupOpen = false;
    public static bool invalidSettings = false;
    public static PackedScene popupScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/popup.tscn");

    //for controls - decides which button is being rebound
    public static int buttontoggle = 0;

    //website connection stuff
    public static UserData user = new UserData();

}
