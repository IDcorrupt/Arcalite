using Godot;
using System;

public partial class SignInPopup : Control
{
    MainMenu parent;
    Button signIn;
    Button cancel;
    LineEdit email;
    LineEdit password;

<<<<<<< Updated upstream
=======
    Button tempUser;

    [Signal] public delegate void LoginEventHandler();
    
>>>>>>> Stashed changes

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

        tempUser = GetNode("Panel/tempuser") as Button;
        tempUser.Pressed += TempUser_Pressed;

    }

    private void TempUser_Pressed()
    {
        UserData temp = new UserData();
        temp.Id = 0;
        temp.Username = "Lajos";
        CharacterData character = new CharacterData();
        character.Id = 0;
        character.Level = 0;
        character.Name = "anyad";
        character.Playtime = new TimeSpan(0, 0, 0);
        character.Save =
            "Map_0\n" +
            "Checkpoint0\n" +
            "False; False; False\n" +
            "100\n" +
            "100\n" +
            "100\n" +
            "100\n" +
            "5\n" +
            "0; 0\n" +
            $"{character.Id}\n" +
            $"{character.Name}\n" +
            $"{character.Playtime.TotalSeconds}";
        temp.Characters.Add(character);

        CharacterData character2 = new CharacterData();
        character2.Id = 1;
        character2.Level = 2;
        character2.Name = "char2";
        character2.Playtime = new TimeSpan(0, 4, 28);
        character2.Save = 
            "Map_0\n" +
            "Checkpoint2\n" +
            "True; True; True\n" +
            "150\n" +
            "130\n" +
            "135\n" +
            "130\n" +
            "11\n" +
            "1; 0\n" +
            $"{character2.Id}\n" +
            $"{character2.Name}\n" +
            $"{character2.Playtime.TotalSeconds}";
        temp.Characters.Add(character2);
        Globals.user = temp;

        parent.submenuOpen = false;
        EmitSignal(SignalName.Login);
        QueueFree();
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

        Console.WriteLine("bejelentkezve: {0}", userdata.Username);
        Console.WriteLine("karakterek:");
        foreach (CharacterData cd in userdata.Characters)
        {
            Console.WriteLine("\t{0};;;{1}", cd.Name, cd.Save);
        }

        //TODO: innent�l k�ne actually bet�lteni az adatokat

        parent.submenuOpen = false;
        QueueFree();
    }
}
