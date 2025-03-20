using Godot;
using System;

public partial class SignInPopup : Control
{
    MainMenu parent;
    Button signIn;
    Button cancel;
    LineEdit email;
    LineEdit password;


    public override void _Ready()
    {
        base._Ready();
        parent = GetParent() as MainMenu;
        signIn = GetNode("Panel/SignIn") as Button;
        cancel = GetNode("Panel/Cancel") as Button;
        email = GetNode("Panel/Email") as LineEdit;
        password = GetNode("Panel/Password") as LineEdit;

        signIn.Pressed += SignIn_Pressed;
        cancel.Pressed += Cancel_Pressed;
    }

    private void Cancel_Pressed()
    {
        parent.submenuOpen = false;
        QueueFree();
    }

    private void SignIn_Pressed()
    {
        string emailtext = email.Text;
        string passwordtext = password.Text;

        UserData userdata = DBConnector.GetUserData(emailtext, passwordtext);

        //TODO: innentõl kéne actually betölteni az adatokat

        parent.submenuOpen = false;
        QueueFree();
    }
}
