using Godot;
using System;

public partial class MainMenu : Control
{
    private Button Start;
    private Button Settings;
    private Button Website;
    private Button Quit;
    private Panel AccountPanel;

    private Button signIn;
    private Button Register;
    private Label AccountName;

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
        AccountPanel = GetNode("Account") as Panel;
        signIn = GetNode("Account/Sign In") as Button;
        Register = GetNode("Account/Register") as Button;
        AccountName = GetNode("Account/Name") as Label;
        Start.Pressed += StartPressed;
        Settings.Pressed += SettingsPressed;
        Website.Pressed += WebsitePressed;
        Quit.Pressed += QuitPressed;
        signIn.Pressed += SignIn_Pressed;
        Register.Pressed += Register_Pressed;
        
        background = GetNode("bg") as TileMapLayer;
        background.TileSet = TilesetLoader.LoadedTileset;
        if(Globals.user.Id >= 0)
        {
            AccountName.Text = Globals.user.Username;
            signIn.Text = "Sign out";
            signIn.Position = new Vector2(43, signIn.Position.Y);
            Register.Hide();
            Register.Disabled = true;
        }
    }

    private void Register_Pressed()
    {
        OS.ShellOpen("http://localhost/Arcalite/login.html#");
    }

    private void SignIn_Pressed()
    {
        if(Globals.user.Id >= 0)
        {
            DBConnector.ClearUserData();
            AccountName.Text = "Guest";
            signIn.Text = "Sign in";
            signIn.Position = new Vector2(10, signIn.Position.Y);
            Register.Show();
            Register.Disabled = false;
            //CLEAR SAVE
        }
        else
        {
            SignInPopup signInPopup = signInPopupScene.Instantiate() as SignInPopup;
            AddChild(signInPopup);
            signInPopup.Login += SignInPopup_Login;
            submenuOpen = true;

        }
    }

    private void SignInPopup_Login()
    {
        if (Globals.user.Id >= 0)
        {
            AccountName.Text = Globals.user.Username;
            signIn.Text = "Sign out";
            signIn.Position = new Vector2(43, signIn.Position.Y);
            Register.Hide();
            Register.Disabled = true;
        }
        else
        {
            AccountName.Text = "Guest";
            signIn.Text = "Sign in";
            signIn.Position = new Vector2(10, signIn.Position.Y);
            Register.Show();
            Register.Disabled = false;
        }
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
        OS.ShellOpen("http://localhost/Arcalite/index.html");
    }
    public void QuitPressed()
    {
        GetTree().Quit();
    }

    private void ButtonControls(bool state)
    {
        if (state)
        {
            Start.Visible = true;
            Settings.Visible = true;
            Website.Visible = true;
            Quit.Visible = true;
            AccountPanel.Visible = true;
        }
        else
        {
            Start.Visible = false;
            Settings.Visible = false;
            Website.Visible = false;
            Quit.Visible = false;
            AccountPanel.Visible = false;
        }
    }

    public override void _Process(double delta)
    {
        if (TilesetLoader.LoadedTileset != null)
            background.TileSet = TilesetLoader.LoadedTileset;
        if (submenuOpen)
            ButtonControls(false);
        else
            ButtonControls(true);

    }
}
