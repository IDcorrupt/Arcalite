using Godot;
using System;

public partial class Globals : Node
{
    //mechanics
    public static bool playerControl = false;
    public static bool hasSavefile = false;
    public const float GRAVITY = 1500f;      //gravity, duh

    //active spawnpoint
    public static Node2D spawnPoint = null;

    //settings
    public static int Difficulty = 2;
    public static bool PopupResult = false;
    public static bool invalidSettings = false;
    public static PackedScene popupScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/popup.tscn");

    //for controls - decides which button is being rebound
    public static int buttontoggle = 0;
}
