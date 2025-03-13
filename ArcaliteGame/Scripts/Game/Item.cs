using Godot;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;

public partial class Item : CharacterBody2D
{

    private AnimatedSprite2D sprite;
    private Area2D area;
    private RayCast2D floatTrigger;
    private bool groundHit;

    //item properties
    [Export] public Enums.itemType type;
    [Export] private float itemCooldown = 5;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode("Sprite") as AnimatedSprite2D;
        area = GetNode("PickupArea") as Area2D;
        floatTrigger = GetNode("FloatTrigger") as RayCast2D;

        //determine item type
        switch (type)
        {
            case Enums.itemType.necklace:
                sprite.Play("necklace");
                itemCooldown = 10f;
                break;
            case Enums.itemType.shield:
                sprite.Play("shield");
                itemCooldown = 20f;
                break;
            case Enums.itemType.shard:
                sprite.Play("shard");
                break;
            default:
                break;
        }

        //set throw velocity and angle
        Random dirRandom = new Random();
        Vector2 throwDir = new Vector2(-150, 0);
        throwDir = throwDir.Rotated(0.5f + (float)dirRandom.NextDouble() * (Mathf.Pi - 1.0f));
        Velocity = throwDir;
    }

    private Vector2 Float(double delta)
    {
        Vector2 vel = Velocity;
        if (!floatTrigger.IsColliding() & !groundHit)
        {
            //initial fall
            vel.Y += Globals.GRAVITY / 3 * (float)delta;
        }else if (!floatTrigger.IsColliding())
        {
            //dropped item state float downwards
            vel.Y += Globals.GRAVITY / 15 * (float)delta;
            //cap Y
            if (vel.Y > 20)
                vel.Y = 20;
        }
        else
        {
            //dropped item state float upwards
            vel.Y -= Globals.GRAVITY / 30 * (float)delta;
            //cap Y
            if (vel.Y < -20)
                vel.Y = -20;
        }
        //gradually slow horizontal movement
        if (vel.X < -10)
            vel.X += 120 * (float)delta;
        else if (vel.X > 10)
            vel.X -= 120 * (float)delta;
        else
            vel.X = 0;


        return vel;
    }

    public void OnTriggerAreaEntered(Node2D body)
    {
        if(body is Player player)
        {
            //send pickup notif to player here (func)
            Globals.player.PickupItem(type, itemCooldown);
            QueueFree();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        GD.Print("itemtype: " + type);
        if (floatTrigger.IsColliding() && !groundHit)
        {
            groundHit = true;
            Velocity = new Vector2(0, Velocity.Y);
        }
        Velocity = Float(delta);
        MoveAndSlide();
    }
}
