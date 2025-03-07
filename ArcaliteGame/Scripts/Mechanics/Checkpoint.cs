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
        sprite.Play("idle");
        map = GetParent().GetParent() as Map;
    }


    public void TriggerAreaBodyEntered(Node2D body)
    {
        if(!triggered)
            TriggerCheckpoint();
    }
    public void OnAnimationFinished()
    {
        if (sprite.Animation == "triggered")
            sprite.Play("empty");
    }

    private void TriggerCheckpoint()
    {
        Globals.spawnPoint.QueueFree();
        sprite.Play("triggered");
        Globals.spawnPoint = this;
        SaveLoadHandler.Save(map.roomStatus(), Globals.player.GetMaxHP(), Globals.player.GetMaxMP(), Globals.player.GetCurrentHP(), Globals.player.GetCurrentMP(), Globals.player.GetAttackDamage(), Globals.player.GetCooldowns());
    }

    public void Empty()
    {
        triggered = true;
        sprite.Play("empty");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
    }
}
