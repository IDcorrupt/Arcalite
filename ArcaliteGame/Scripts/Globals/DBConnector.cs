using Godot;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Collections;

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
        
        UserData userdata = new UserData();

        //checking if profile exists
        string query = @$"
            SELECT id 
            FROM profile 
            WHERE deletedAt IS NULL AND email = '{email}'";

        using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
        {
            if (!reader.HasRows) throw new Exception("Nincs aktív fiók ehhez az e-mail címhez regisztrálva!");
        }

        //fetching user data
        query = @$"
            SELECT id, username 
            FROM profile 
            WHERE deletedAt IS NULL AND email = '{email}' AND password = PASSWORD('{password}')";

        using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
        {
            if (!reader.HasRows) { throw new Exception("Hibás jelszó!"); }

            reader.Read();
            userdata.Id = reader.GetInt32("id");
            userdata.Username = reader.GetString("name");
        }

        //NEED TO IMPLEMENT FETCHING THE SAVES

        //fetching character data
        query = $@"
            SELECT player.id AS id, player.name AS name, player.hp AS hp, player.mp AS mp, player.levelid AS level, avatar.image AS image
            FROM player INNER JOIN avatar ON avatar.id = player.avatarid
            WHERE player.profileid = {userdata.Id}";

        using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
        {

        }

        //összerakni struct-listába, majd a user structot visszaküldeni
    }
}