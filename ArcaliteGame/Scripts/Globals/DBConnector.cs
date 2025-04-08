using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using Godot;
using MySqlConnector;

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
        Id = -1;
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

    public static void ClearUserData()
    {
        if (Globals.user.Id >= 0)
        {
            Globals.user = new UserData();
        }
    }

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
            if (!reader.HasRows)
            {
                conn.Close();
                throw new Exception("Account not found");
            }
        }

        //fetching user data
        query = $@"
            SELECT id, username 
            FROM profile 
            WHERE deletedAt IS NULL AND email = '{email}' AND password = PASSWORD('{password}')";

        using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
        {
            if (!reader.HasRows)
            {
                conn.Close();
                throw new Exception("Incorrect password");
            }

            reader.Read();
            userdata.Id = reader.GetInt32("id");
            userdata.Username = reader.GetString("username");
        }

        //fetching character data

        userdata.Characters = GetCharacters(userdata.Id, true);
        
        conn.Close();
        return userdata;
    }
    public static List<CharacterData> GetCharacters(int userId, bool connState = false)
    {
        if (!connState)
            conn.Open();
        List<CharacterData> characters = new List<CharacterData>();
        string query = $@"
            WITH lastSaves AS (
                SELECT *
                FROM saves
                WHERE time = (SELECT MAX(time) FROM saves AS s WHERE s.playerid = saves.playerid)
            )
            SELECT player.id AS id, player.name AS name, player.levelid AS level, lastSaves.save AS save, player.playtime as time
            FROM player 
                INNER JOIN avatar ON avatar.id = player.avatarid
                LEFT JOIN lastSaves ON player.id = lastSaves.playerid
            WHERE player.profileid = {userId};";

        using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
        {
            while (reader.Read())
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
                }
                else
                {
                    character.Save = null;
                }

                characters.Add(character);
            }
        }
        if(!connState)
            conn.Close();
        return characters;
    }

    public static int GetLastPlayerEntry(bool connstate = false)
    {
        conn.Open();
        string query = "SELECT MAX(id) FROM player;";
        int newRunID = Convert.ToInt32(new MySqlCommand(query, conn).ExecuteScalar());
        conn.Close();
        return newRunID;
    }

    public static Enums.SaveState PrepareSave(int playerId, string save)
    {
        try
        {
            bool existing = false;
            conn.Open();
            string query = $"SELECT id FROM player WHERE id = {playerId};";
            using (MySqlDataReader reader = new MySqlCommand(query, conn).ExecuteReader())
            {
                //if player exists
                if (reader.HasRows)
                    existing = true;
            }
            if (existing)
            {
                query = $"UPDATE player SET playtime={Globals.playTime.ToString()} WHERE id={playerId};";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    cmd.ExecuteNonQuery();
                conn.Close();
                return Enums.SaveState.Existing;
            }
            //if player entry doesn't exist
            string[] savedata = save.Split('\n');
            query = "INSERT INTO player (name, hp, mp, profileid, avatarid, levelid, playtime) " +
                       "VALUES (@name, @hp, @mp, @profileid, @avatarid, @levelid, @playtime);";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", savedata[10]);
                cmd.Parameters.AddWithValue("@hp", savedata[3]);
                cmd.Parameters.AddWithValue("@mp", savedata[4]);
                cmd.Parameters.AddWithValue("@profileid", Globals.user.Id);
                cmd.Parameters.AddWithValue("@avatarid", '1');
                cmd.Parameters.AddWithValue("@levelid", '1');
                cmd.Parameters.AddWithValue("@playtime", savedata[11]);

                bool result = cmd.ExecuteNonQuery() != 0;
                conn.Close();
                if (result)
                {
                    return Enums.SaveState.Created;
                }
                else
                    return Enums.SaveState.None;
            }
        }
        catch (Exception)
        {
            conn.Close();
            return Enums.SaveState.None;
            throw;
        }
    }

    public static bool UploadSave(int playerId, string save)
    {
        try
        {
            //these 3 lines insert the runID if it was newly created
            string[] data = save.Split('\n');
            data[9] = playerId.ToString();
            save = String.Join("\n", data);

            conn.Open();
            string query = "INSERT INTO saves (playerid, save) VALUES (@playerid, @save)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@playerid", playerId);
                cmd.Parameters.AddWithValue("@save", save);

                bool result = cmd.ExecuteNonQuery() != 0;
                conn.Close();
                return result;
            }
        }
        catch (Exception)
        {
            conn.Clone();
            throw;
        }
    }

    #endregion
} 