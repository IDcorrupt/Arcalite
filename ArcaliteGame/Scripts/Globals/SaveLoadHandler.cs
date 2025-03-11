using Godot;
using System;
using System.Collections.Generic;

public partial class SaveLoadHandler : Node
{
    static string savepath = "user://savefile.txt";


    public static void Save(List<bool> roomsCleared, float MaxHP, float MaxMP, float currentHP, float currentMP, float attackDamage, List<float> cooldowns, List<int> equippedItems)
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
        //player cooldowns
        file.StoreLine(String.Join("; ", cooldowns));
        //items
        file.StoreString(String.Join(", ", equippedItems));
        file.Close();
    }

    public static bool CheckSave()
    {
        //check if savefile exists
        if (FileAccess.FileExists(savepath))
            return true;
        else return false;
    }
    public static void Load()
    {
        //delete save in memory
        Globals.currentSave = new List<string>();
        //opne file
        var file = FileAccess.Open(savepath, FileAccess.ModeFlags.Read);
        string[] data = file.GetAsText().Split('\n');
        //load to memory
        foreach (string s in data)
            Globals.currentSave.Add(s);
    }
}
