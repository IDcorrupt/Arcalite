using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Quic;

public partial class Checkpoint : Node2D
{
    [Export] private Area2D triggerArea;
    [Export] public bool finalCheckPoint = false;
    private Map map;
    private AnimatedSprite2D sprite;
    GpuParticles2D particles;
    public bool playerEntered = false;
    private bool playerExited = false;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
        particles = GetNode("GPUParticles2D") as GpuParticles2D;
        map = GetParent().GetParent() as Map;

        if(triggerArea == null)
        {
            //if no custom TA set, set the default one
            triggerArea = GetNode("TriggerArea") as Area2D;
        }
        else
        {
            //delete default trigger if there is custom
            Area2D defaultTrigger = GetNode("TriggerArea") as Area2D;
            defaultTrigger.QueueFree();

            //connect custom TA signals
            triggerArea.CollisionLayer = 12;    //not expecting it to be set in inspector
            triggerArea.CollisionMask = 1;
            triggerArea.BodyEntered += TriggerAreaBodyEntered;
            triggerArea.BodyExited += TriggerAreaBodyExited;
        }


            sprite.Play("idle");
    }


    public void TriggerAreaBodyEntered(Node2D body)
    {
        Player_EnteredRestArea();
        if (!playerEntered && sprite.Animation == "idle")
            TriggerCheckpoint();
    }
    public void TriggerAreaBodyExited(Node2D body)
    {
        //scuffed solution for new game / map switch saving -> spawning on a checkpoint sets it to empty -> doesn't save
        if (!playerExited)
        {
            if(Name == "Checkpoint0")
                SaveLoadHandler.Save(
                    map.roomStatus(), 
                    Globals.player.GetMaxHP(), 
                    Globals.player.GetMaxMP(), 
                    Globals.player.GetPotentialHP(), 
                    Globals.player.GetCurrentMP(), 
                    Globals.player.GetAttackDamage(), 
                    Globals.player.GetEquips()
                    );
            playerExited = true;
        }
        Player_ExitedRestArea();
    }
    public void OnAnimationFinished()
    {
        if (sprite.Animation == "triggered")
            Empty();
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
            Globals.player.GetPotentialHP(), 
            Globals.player.GetCurrentMP(), 
            Globals.player.GetAttackDamage(), 
            Globals.player.GetEquips()
            );
        playerEntered = true;
        //set this to true too so it doesn't save again on hitbox exit
        playerExited = true;
    }

    public void Empty()
    {
        playerEntered = true;
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

        if (!playerEntered)
            Globals.player.Rest(true, 20);
        else Globals.player.Rest(true);
    }
}
