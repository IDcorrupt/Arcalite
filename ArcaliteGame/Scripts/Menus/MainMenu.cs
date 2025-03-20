using Godot;
using System;

public partial class MainMenu : Control
{
    private Button Start;
    private Button Settings;
    private Button Website;
    private Button Quit;

    private Button signIn;
    private Button Register;

    public bool submenuOpen = false;

    private PackedScene submenuStart = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/submenuStart.tscn");
    private PackedScene submenuSettings = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/submenuSettings.tscn");
    private PackedScene submenuWebsite = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/submenuWebsite.tscn");
    private PackedScene signInPopupScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/sign_in_popup.tscn");

    private TileMapLayer background;
    public override void _Ready()
    {
        Start = GetNode<Button>("Start");
        Settings = GetNode<Button>("Settings");
        Website = GetNode<Button>("Website");
        Quit = GetNode<Button>("Quit");
        signIn = GetNode("Account/Sign In") as Button;
        Register = GetNode("Account/Register") as Button;
        Start.Pressed += StartPressed;
        Settings.Pressed += SettingsPressed;
        Website.Pressed += WebsitePressed;
        Quit.Pressed += QuitPressed;
        signIn.Pressed += SignIn_Pressed;
        Register.Pressed += Register_Pressed;
        
        background = GetNode("bg") as TileMapLayer;
        background.TileSet = TilesetLoader.LoadedTileset;
    }

    private void Register_Pressed()
    {
        //LINK DOESN'T WORK BECAUSE REGISTER PAGE DOESN'T HAVE UNIQUE LINK (it goes to sign in page)
        OS.ShellOpen("http://localhost/school/Website/login.html#");    //REDO LINK FOR FINAL (file structure will probably change)
    }

    private void SignIn_Pressed()
    {
        Control signInPopup = signInPopupScene.Instantiate() as Control;
        AddChild(signInPopup);
        submenuOpen = true;
    }

    public void StartPressed()
    {
        Node startNode = submenuStart.Instantiate();
        AddChild(startNode);
        submenuOpen = true;
    }
    public void SettingsPressed()
    {
        Node settingNode = submenuSettings.Instantiate();
        AddChild(settingNode);
        submenuOpen = true;
    }
    public void WebsitePressed()
    {
        OS.ShellOpen("http://localhost/school/Website/");       //REDO LINK FOR FINAL (file structure will probably change)
    }
    public void QuitPressed()
    {
        GetTree().Quit();
    }
    public override void _Process(double delta)
    {
        if (TilesetLoader.LoadedTileset != null)
            background.TileSet = TilesetLoader.LoadedTileset;
        if (submenuOpen)
        {
            Start.Visible = false;
            Settings.Visible = false;
            Website.Visible = false;
            Quit.Visible = false;
        }
        else
        {
            Start.Visible = true;
            Settings.Visible = true;
            Website.Visible = true;
            Quit.Visible = true;
        }
        }
}
