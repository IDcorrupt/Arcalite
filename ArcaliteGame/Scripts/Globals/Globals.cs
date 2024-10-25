using Godot;
using System;

public partial class Globals : Node
{
    //mechanics
    public static bool playerControl = false;
    public static bool hasSavefile = false;
    public static bool PopupResult = false;


    //settings
    public static int Difficulty = 2;
    //for controls - decides which button is being rebound
    public static int buttontoggle = 0;
}
