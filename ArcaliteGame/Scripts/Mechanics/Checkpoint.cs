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
    GpuParticles2D particles;
    public bool triggered;

    public override void _Ready()
    {
        base._Ready();
        triggerArea = GetNode("TriggerArea") as Area2D;
        sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
        particles = GetNode("GPUParticles2D") as GpuParticles2D;
        map = GetParent().GetParent() as Map;
        sprite.Play("idle");
    }


    public void TriggerAreaBodyEntered(Node2D body)
    {
        Player_EnteredRestArea();
        if (!triggered && sprite.Animation == "idle")
            TriggerCheckpoint();
    }
    public void TriggerAreaBodyExited(Node2D body)
    {
        //scuffed solution for new game / map switch saving -> spawning on a checkpoint sets it to empty -> doesn't save
        if(Name == "Checkpoint0")
            SaveLoadHandler.Save(
                map.roomStatus(), 
                Globals.player.GetMaxHP(), 
                Globals.player.GetMaxMP(), 
                Globals.player.GetCurrentHP(), 
                Globals.player.GetCurrentMP(), 
                Globals.player.GetAttackDamage(), 
                Globals.player.GetEquips()
                );
        Player_ExitedRestArea();
    }
    public void OnAnimationFinished()
    {
        if (sprite.Animation == "triggered")
        {
            GD.Print("finised");
            Empty();
        }
    }

    private void TriggerCheckpoint()
    {
        Globals.spawnPoint.QueueFree();
        Globals.spawnPoint = this;
        sprite.Play("triggered");
        SaveLoadHandler.Save(
            map.roomStatus(), 
            Globals.player.GetMaxHP(), 
            Globals.player.GetMaxMP(), 
            Globals.player.GetCurrentHP(), 
            Globals.player.GetCurrentMP(), 
            Globals.player.GetAttackDamage(), 
            Globals.player.GetEquips()
            );
        triggered = true;
    }

    public void Empty()
    {

        triggered = true;
        sprite.Play("empty");
        particles.Emitting = true;

    }
    public override void _Process(double delta)
    {
        base._Process(delta);
        //connect rest state recharge 
        if(Globals.player is not null)
        {
            Globals.player.EnteredRestArea += Player_EnteredRestArea;
            Globals.player.ExitedRestArea += Player_ExitedRestArea;
        }
    }

    private void Player_ExitedRestArea()
    {
        Globals.player.Rest(false);
    }

    private void Player_EnteredRestArea()
    {

        if (!triggered)
            Globals.player.Rest(true, 20);
        else Globals.player.Rest(true);
    }
}
