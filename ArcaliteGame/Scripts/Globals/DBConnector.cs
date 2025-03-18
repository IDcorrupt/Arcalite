using Godot;
using System;

public struct CharacterData 
{
    public int Id, Level, HP;
    public string Name, AvatarUrl;

    public CharacterData(int id, string name, int hp, int level, string avatarUrl)
    {
        Id = id; 
        Name = name;
        HP = hp;
        Level = level; 
        AvatarUrl = avatarUrl;
    }
}

public struct UserData
{
    public int Id;
    public string Username;
    public List<CharacterData> Characters;

    public UserData(int id, string username, List<CharacterData> characters = null)
    {
        Id = id;
        Username = username;
        Characters = characters;
    }
};

public static class DBConnector
{
    private static string host = "localhost", user = "root", password = "", database = "arcalite";
    private static string connstr = "host=localhost;user=root;password=;database=arcalite";
    private static MySqlConnection conn = new MySqlConnection(connstr);

    private static void setConnstr()
    {
        connstr = $"host={host};user={user};password={password};database={database}";
        conn = new MySqlConnection(connstr) ;
    }

    /* PROPERTIES */

    public static string Host { 
        get => host; 
        set {
            host = value;
            setConnstr();
        }
    }

    public static string User
    {
        get => user;
        set
        {
            user = value;
            setConnstr();
        }
    }

    public static string Password
    {
        get => password;
        set
        {
            password = value;
            setConnstr();
        }
    }

    public static string Database
    {
        get => database;
        set
        {
            database = value;
            setConnstr();
        }
    }

    /* METHODS */

    public static UserData GetUserData(string email, string password)
    {
        //user->id, name, characters
        //character->id, name, hp, level, avatar

        //fetching user data
        string query = @$"
            SELECT id, username 
            FROM profile 
            WHERE deletedAt IS NULL AND email = '{email}' AND password = PASSWORD('{password}')";

        using ()
        {

        }

        int profileid = 1;  //ezt majd a query-bõl
        //itt majd visszatérés, ha nincs ilyen fiók, n stuff

        //fetching character data
        query = $@"
            SELECT player.id, player.name, player.hp, player.levelid, avatar.image
            FROM player INNER JOIN avatar ON avatar.id = player.avatarid
            WHERE player.profileid = {profileid}";

        //összerakni struct-listába, majd a user structot visszaküldeni
    }
}