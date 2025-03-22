using Godot;
using System;
using System.Collections.Generic;

public partial class SaveLoadHandler : Node
{
    static string savepath = "user://savefile.txt";


    public static void Save(List<bool> roomsCleared, float MaxHP, float MaxMP, float currentHP, float currentMP, float attackDamage, List<int> equippedItems)
    {
        var file = FileAccess.Open(savepath, FileAccess.ModeFlags.Write);
        //map
        file.StoreLine(Globals.activeMap);
        //checkpoint
        file.StoreLine(Globals.spawnPoint.Name.ToString());
        //rooms cleared
        file.StoreLine(String.Join("; ", roomsCleared));
        //player max hp
        file.StoreLine(MaxHP.ToString());
        //player max mp
        file.StoreLine(MaxMP.ToString());
        //player current hp
        file.StoreLine(currentHP.ToString());
        //player current mp
        file.StoreLine(currentMP.ToString());
        //player attack damage
        file.StoreLine(attackDamage.ToString());
        //items
        file.StoreLine(String.Join("; ", equippedItems));

        //save id (i assume the database will need it)
        file.StoreLine("-1");
        //save name
        file.StoreLine(Globals.runName);
        //playtime
        file.StoreString(Globals.playTime.ToString());
        file.Close();

        //TODO: Ide valahol majd el kell tarolni a jelenlegi playerid-t, es az elso argumentumnak megadni.
        //DBConnector.UploadSave(, savepath);
    }

    public static bool CheckSave()
    {
        //check if savefile exists
        if (FileAccess.FileExists(savepath))
            return true;
        else return false;
    }
    public static void Load(string save = "")
    {
        //delete save in memory
        Globals.currentSave = new List<string>();
        if (save.Length > 0)
        {
            string[] cloudsave = save.Split('\n');
            foreach (string s in cloudsave)
                Globals.currentSave.Add(s);
        }
        //open file
        var file = FileAccess.Open(savepath, FileAccess.ModeFlags.Read);
        string[] data = file.GetAsText().Split('\n');
        //load to memory
        foreach (string s in data)
            Globals.currentSave.Add(s);
    }
}
