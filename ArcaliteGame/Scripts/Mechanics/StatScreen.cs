using Godot;
using System;

public partial class StatScreen : Control
{
    Label Time;
    Label Shards;

    bool loaded = false;

    public override void _Ready()
    {
        base._Ready();
        Globals.statScreen = this;
        Time = GetNode("Time") as Label;
        Shards = GetNode("Shards") as Label;
    }

    public void UpdateStats()
    {
        //time
        TimeSpan time = TimeSpan.FromSeconds(Mathf.Round((double)Globals.playTime));
        Time.Text = time.ToString(@"mm\:ss");
        //shards
        int hpShards = Mathf.FloorToInt(Globals.player.GetMaxHP() - 100) / 10;
        int mpShards = Mathf.FloorToInt(Globals.player.GetMaxMP() - 100) / 10;
        int dmgShards = Mathf.FloorToInt(Globals.player.GetAttackDamage() - 5) / 2;
        Shards.Text = (hpShards + mpShards + dmgShards).ToString();
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Globals.gameBeaten && !loaded)
        {
            UpdateStats();
            loaded = true;
        }
    }

}
