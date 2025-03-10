using Godot;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;

public partial class Item : CharacterBody2D
{

    private AnimatedSprite2D sprite;
    private Area2D area;
    private RayCast2D floatTrigger;
    private Enums.itemType type;
    public Item(Enums.itemType type)
    {
        this.type = type;
    }
    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode("Sprite") as AnimatedSprite2D;
        area = GetNode("PickupArea") as Area2D;
        switch (type)
        {
            case Enums.itemType.necklace:
                sprite.Play("necklace");
                break;
            case Enums.itemType.shield:
                sprite.Play("shield");
                break;
            case Enums.itemType.shard:
                sprite.Play("shard");
                break;
            default:
                break;
        }
    }

    private Vector2 Float(double delta)
    {
        Vector2 vel = Velocity;
        if (floatTrigger.IsColliding())
        {
            vel.Y -= Globals.GRAVITY * (float)delta;
        }
        else
        {
            vel.Y += Globals.GRAVITY * (float)delta;
        }
        if (vel.X < 0) 
            vel.X += 100 * (float)delta;
        else if (vel.X > 0)
            vel.X -= 100 * (float)delta; 
        return vel;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = Float(delta);
    }
}
