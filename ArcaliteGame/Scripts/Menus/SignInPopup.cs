using Godot;
using System;

public partial class SignInPopup : Control
{
    MainMenu parent;
    Button signIn;
    Button cancel;
    LineEdit email;
    LineEdit password;
    Label errorCodes;

    Button tempUser;
    [Signal] public delegate void LoginEventHandler();
    

    public override void _Ready()
    {
        base._Ready();
        parent = GetParent() as MainMenu;
        signIn = GetNode("Panel/SignIn") as Button;
        cancel = GetNode("Panel/Cancel") as Button;
        email = GetNode("Panel/Email") as LineEdit;
        password = GetNode("Panel/Password") as LineEdit;
        errorCodes = GetNode("Panel/errors") as Label;

        signIn.Pressed += SignIn_Pressed;
        cancel.Pressed += Cancel_Pressed;
    }

    private void Cancel_Pressed()
    {
        parent.submenuOpen = false;
        EmitSignal(SignalName.Login);
        QueueFree();
    }

    private void SignIn_Pressed()
    {
        string emailtext = email.Text;
        string passwordtext = password.Text;

        try
        {
            Globals.user = DBConnector.GetUserData(emailtext, passwordtext);
            parent.submenuOpen = false;
            EmitSignal(SignalName.Login);
            QueueFree();
        }
        catch (Exception ex)
        {
            errorCodes.Text = ex.Message;
            throw;
        }
    }
}
