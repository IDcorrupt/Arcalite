using System;
using System.Collections.Generic;
using System.IO;
<<<<<<< Updated upstream
using MySql.Data.MySqlClient;
=======
using System.Security.Cryptography;
using Godot;
using MySqlConnector;
>>>>>>> Stashed changes

public struct CharacterData
{
    public int Id, Level;
    public string Name, Save;
    public TimeSpan Playtime;
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
        conn = new MySqlConnection(connstr);
    }

    #region PROPERTIES

    public static string Host
    {
        get => host;
        set
        {
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

<<<<<<< Updated upstream
=======
    public static void ClearUserData()
    {
        if (Globals.user.Id >= 0)
        {
            Globals.user = new UserData();
        }
    }

>>>>>>> Stashed changes
    public static UserData GetUserData(string email, string password)
    {
        UserData userdata = new UserData();
        try
        {
            conn.Open();
        }
        catch (Exception)
        {
            throw new Exception("server not found");
        }

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
<<<<<<< Updated upstream
            if (!reader.HasRows) { throw new Exception("Hibás jelszó!"); }
=======
            if (!reader.HasRows)
            {
                conn.Close();
                throw new Exception("Incorrect password");
            }
>>>>>>> Stashed changes

            reader.Read();
            userdata.Id = reader.GetInt32("id");
            userdata.Username = reader.GetString("name");
        }

        //fetching character data
        query = $@"
            WITH lastSaves AS (
                SELECT *
                FROM saves
                WHERE time = (SELECT MAX(time) FROM saves AS s WHERE s.playerid = saves.playerid)
            )
<<<<<<< Updated upstream
            SELECT player.id AS id, player.name AS name, player.hp AS hp, player.mp AS mp, player.levelid AS level, avatar.image AS image, lastSaves.save AS save
=======
            SELECT player.id AS id, player.name AS name, player.levelid AS level, lastSaves.save AS save, player.playtime as time
>>>>>>> Stashed changes
            FROM player 
                INNER JOIN avatar ON avatar.id = player.avatarid
                LEFT JOIN lastSaves ON player.id = lastSaves.playerid
            WHERE player.profileid = {userdata.Id};";

        using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
        {
<<<<<<< Updated upstream
            while (reader.Read()) 
=======
            while (reader.Read())
>>>>>>> Stashed changes
            {
                CharacterData character = new CharacterData();
                character.Id = reader.GetInt32("id");
                character.Name = reader.GetString("name");
                character.Playtime = reader.GetTimeSpan("time");
                character.Level = reader.GetInt32("level");

                if (reader["save"] != DBNull.Value)
                {
                    long length = reader.GetBytes(reader.GetOrdinal("save"), 0, null, 0, int.MaxValue);
                    byte[] data = new byte[length];
                    reader.GetBytes(reader.GetOrdinal("save"), 0, data, 0, (int)length);
                    character.Save = System.Text.Encoding.UTF8.GetString(data);
<<<<<<< Updated upstream
                } 
=======
                }
>>>>>>> Stashed changes
                else
                {
                    character.Save = null;
                }

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
            cmd.Parameters.AddWithValue("@save", File.ReadAllBytes(path));

            return cmd.ExecuteNonQuery() != 0;
        }
    }

    #endregion
} 