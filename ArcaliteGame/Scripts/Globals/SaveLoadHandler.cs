using Godot;
using System;
using System.Collections.Generic;

public partial class SaveLoadHandler : Node
{
    static string savepath = "user://saves";


    public static void Save(List<bool> roomsCleared, float MaxHP, float MaxMP, float currentHP, float currentMP, float attackDamage)
    {
        //[TBD] ADD ITEMS WHEN THEY START EXISTING
        var file = FileAccess.Open(savepath, FileAccess.ModeFlags.Write);
        GD.Print("storing active map: " + Globals.activeMap);
        file.StoreString(Globals.activeMap);
        file.StoreVar(Globals.spawnPoint.Name.ToString());
        foreach(var room in roomsCleared)
            file.StoreString(room.ToString());
        /*file.StoreVar(MaxHP);
        file.StoreVar(MaxMP);
        file.StoreVar(currentHP);
        file.StoreVar(currentMP);
        file.StoreVar(attackDamage);*/
    }

    public bool CheckSave()
    {
        if (FileAccess.FileExists(savepath))
            return true;
        else return false;
    }
    public static void Load()
    {
        var file = FileAccess.Open(savepath, FileAccess.ModeFlags.Read);
        GD.Print("activemap: " + file.GetLine());
        file.GetAsText();
        //ADD DATA HERE
    }



}
