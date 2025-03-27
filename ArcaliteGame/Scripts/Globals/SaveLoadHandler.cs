using Godot;
using System;
using System.Collections.Generic;

public partial class SaveLoadHandler : Node
{
    static string savepath = "user://savefile.txt";


    public static void Save(List<bool> roomsCleared, float MaxHP, float MaxMP, float currentHP, float currentMP, float attackDamage, List<int> equippedItems)
    {
        string save = "";
        //0: map
        save += Globals.activeMap+"\n";
        //1: checkpoint
        save += Globals.spawnPoint.Name.ToString()+"\n";
        //2: rooms cleared
        save += $"{String.Join("; ", roomsCleared)}\n";
        //3: player max hp
        save += MaxHP.ToString()+"\n";
        //4: player max mp
        save += MaxMP.ToString()+"\n";
        //5: player current hp
        save += currentHP.ToString()+"\n";
        //6: player current mp
        save += currentMP.ToString()+"\n";
        //7: player attack damage
        save += attackDamage.ToString()+"\n";
        //8: items
        save += $"{String.Join("; ", equippedItems)}\n";
        //9: save id (i assume the database will need it)
        save += Globals.runID.ToString()+"\n";
        //10: save name
        save += Globals.runName + "\n";
        //11: playtime
        save += Globals.playTime.ToString();

        bool newsave = false;
        //cloud stuff
        if (Globals.user.Id >= 0)
        {
            GD.Print("ini cloud save check");
            switch (DBConnector.PrepareSave(Globals.runID, save))
            {
                case Enums.SaveState.None:
                    GD.PrintErr("preparesave() returned none");
                    break;
                case Enums.SaveState.Created:
                    DBConnector.UploadSave(DBConnector.GetLastPlayerEntry(), save);
                    Globals.runID = DBConnector.GetLastPlayerEntry();
                    newsave = true;
                    break;
                case Enums.SaveState.Existing:
                    DBConnector.UploadSave(Globals.runID, save);
                    break;
                default:
                    break;
            }
        }
        if (newsave)
        {
            //if new save was created -> assign new run id
            string[] data = save.Split('\n');
            data[9] = Globals.runID.ToString();
            GD.Print("new save entry: " + data[9]);
            save = String.Join("\n", data);
        }



        var file = FileAccess.Open(savepath, FileAccess.ModeFlags.Write);
        foreach (string line in save.Split('\n'))
            file.StoreLine(line);
        file.Close();

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
            //if called from cloudsave
            string[] cloudsave = save.Split('\n');
            foreach (string s in cloudsave)
                Globals.currentSave.Add(s);
        }
        else
        {
            //if called from local save
            var file = FileAccess.Open(savepath, FileAccess.ModeFlags.Read);
            string[] data = file.GetAsText().Split('\n');
            foreach (string s in data)
                Globals.currentSave.Add(s);
        }
    }
}
