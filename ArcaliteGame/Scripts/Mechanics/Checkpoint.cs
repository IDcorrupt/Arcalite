using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Quic;

public partial class Checkpoint : Node2D
{
    private Area2D triggerArea;
    private AnimatedSprite2D sprite;
    private Map map;
    public bool triggered;

    public override void _Ready()
    {
        base._Ready();
        triggerArea = GetNode("TriggerArea") as Area2D;
        sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
        map = GetParent().GetParent() as Map;
        sprite.Play("idle");
        TreeExiting += Checkpoint_TreeExiting;
    }

    private void Checkpoint_TreeExiting()
    {
        GD.Print($"{this.Name} exiting tree");
    }

    public void TriggerAreaBodyEntered(Node2D body)
    {
        if(!triggered && sprite.Animation == "idle")
            TriggerCheckpoint();
    }
    public void TriggerAreaBodyExited(Node2D body)
    {
        //scuffed solution for new game / map switch saving -> spawning on a checkpoint sets it to empty -> doesn't save
        if(Name == "Checkpoint0")
            SaveLoadHandler.Save(map.roomStatus(), Globals.player.GetMaxHP(), Globals.player.GetMaxMP(), Globals.player.GetCurrentHP(), Globals.player.GetCurrentMP(), Globals.player.GetAttackDamage(), Globals.player.GetCooldowns());
    }
    public void OnAnimationFinished()
    {
        if (sprite.Animation == "triggered")
            sprite.Play("empty");
    }

    private void TriggerCheckpoint()
    {
        Globals.spawnPoint.QueueFree();
        Globals.spawnPoint = this;
        sprite.Play("triggered");
        SaveLoadHandler.Save(map.roomStatus(), Globals.player.GetMaxHP(), Globals.player.GetMaxMP(), Globals.player.GetCurrentHP(), Globals.player.GetCurrentMP(), Globals.player.GetAttackDamage(), Globals.player.GetCooldowns());
    }

    public void Empty()
    {
        triggered = true;
        sprite.Play("empty");

    }
}
