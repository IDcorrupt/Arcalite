using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

public struct CharacterData 
{
    public int Id, Level, HP, MP;
    public string Name, AvatarUrl, Save;
}

public struct UserData
{
    public int Id;
    public string Username;
    public List<CharacterData> Characters;

    public UserData()
    {
        Characters = new List<CharacterData>();
    }
};

public static class DBConnector
{
    private static string host = "localhost", user = "root", password = "", database = "arcalite";
    private static int port = 3306;
    private static string connstr = "host=localhost;user=root;password=;database=arcalite";
    private static MySqlConnection conn = new MySqlConnection(connstr);

    private static void setConnstr()
    {
        connstr = $"host={host};port={port};user={user};password={password};database={database}";
        conn = new MySqlConnection(connstr) ;
    }

    #region PROPERTIES

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

    public static int Port
    {
        get => port;
        set
        {
            port = value;
            setConnstr();
        }
    }

    #endregion

    #region METHODS

    public static UserData GetUserData(string email, string password)
    {
        UserData userdata = new UserData();
        conn.Open();

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
        query = $@"
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

        //fetching character data
        query = $@"
            WITH lastSave AS (
                SELECT *
                FROM saves
                WHERE time = (SELECT MAX(time) FROM saves AS s WHERE s.playerid = saves.playerid)
            )
            SELECT player.id AS id, player.name AS name, player.hp AS hp, player.mp AS mp, player.levelid AS level, avatar.image AS image, lastSave.save AS save
            FROM player 
                INNER JOIN avatar ON avatar.id = player.avatarid
                LEFT JOIN lastSave ON player.id = lastSave.playerid
            WHERE player.profileid = {userdata.Id}";

        using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
        {
            while (reader.Read()) 
            { 
                CharacterData character = new CharacterData();
                character.Id = reader.GetInt32("id");
                character.Name = reader.GetString("name");
                character.HP = reader.GetInt32("hp");
                character.MP = reader.GetInt32("mp");
                character.Level = reader.GetInt32("level");
                character.AvatarUrl = reader.GetString("image");

                long length = reader.GetBytes(reader.GetOrdinal("save"), 0, null, 0, int.MaxValue);
                byte[] data = new byte[length];
                reader.GetBytes(reader.GetOrdinal("save"), 0, data, 0, (int)length);

                character.Save = System.Text.Encoding.UTF8.GetString(data);

                userdata.Characters.Add(character);
            }
        }

        conn.Close();
        return userdata;
    }

    public static bool UploadSave(int playerId, string path)
    {
        string query = "INSERT INTO saves (playerid, save) VALUES (@playerid, @save)";

        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@playerid", playerId);
            cmd.Parameters.AddWithValue("@save", File.ReadAllBytes("path"));

            return cmd.ExecuteNonQuery() != 0;
        }
    }

    #endregion
}