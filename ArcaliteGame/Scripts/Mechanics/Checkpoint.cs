using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Quic;

public partial class Checkpoint : Node2D
{
    private Area2D triggerArea;
    private AnimatedSprite2D sprite;

    public override void _Ready()
    {
        base._Ready();
        triggerArea = GetNode("TriggerArea") as Area2D;
        sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
        sprite.Play("idle");
    }


    public void TriggerAreaBodyEntered(Node2D body)
    {
        TriggerCheckpoint();
    }
    public void OnAnimationFinished()
    {
        if (sprite.Animation == "triggered")
        QueueFree();
    }

    private void TriggerCheckpoint()
    {
        List<bool> asd = new List<bool>();
        asd.Add(false);
        asd.Add(false);
        asd.Add(true);
        asd.Add(false);
        SaveLoadHandler.Save(asd, 100, 100, 50, 50, 10);
        sprite.Play("triggered");
    }

}
