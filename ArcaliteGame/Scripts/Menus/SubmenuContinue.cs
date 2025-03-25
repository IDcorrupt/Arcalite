using Godot;
using System;

public partial class SubmenuContinue : Control
{
    PackedScene saveItemScene = (PackedScene)ResourceLoader.Load("res://Nodes/Menus/save_item.tscn");
    Label nosave;
    VBoxContainer list;


    [Signal] public delegate void MenuClosedEventHandler();

    public override void _Ready()
    {
        base._Ready();
        nosave = GetNode("Panel/Nosave") as Label;
        list = GetNode("Panel/SaveList") as VBoxContainer;
        nosave.Hide();

        if (Globals.hasSavefile)
        {
            //add local save with id = -1;
            if (Globals.currentSave[9] == "-1")
            {
                SaveItem saveItemNode = saveItemScene.Instantiate() as SaveItem;
                saveItemNode.ID = -1;
                list.AddChild(saveItemNode);

            }
        }
        GD.Print("save count: " + Globals.user.Characters.Count);
        Globals.user.Characters = DBConnector.GetCharacters(Globals.user.Id);
        for (int i = 0; i < Globals.user.Characters.Count; i++)
        {
            //add saveitems from profile if there are
            SaveItem saveItemNode = saveItemScene.Instantiate() as SaveItem;
            saveItemNode.ID = i;

            list.AddChild(saveItemNode);
        }

        

        if (list.GetChildCount() <= 1)
            //show nosave label if list only includes the filler
            nosave.Show();
        

    }

    public void OnBackPressed()
    {
        EmitSignal(SignalName.MenuClosed);
        QueueFree();
    }

    public override void _Process(double delta)
    {
        //escape functionality
        base._Process(delta);
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            OnBackPressed();
        }
    }

}
