using Godot;
using System;

public partial class Spike : Node2D
{
    private AnimatedSprite2D sprite;
    private Area2D hitBox;
    private Timer stillTimer;
    private float damage;
    private bool facing;
    private bool playerInArea = false;
    private float lastSpeedScale = 1;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode("Sprite") as AnimatedSprite2D;
        hitBox = GetNode("HitBox") as Area2D;
        stillTimer = GetNode("StillTimer") as Timer;
    }
    public void SetStats(float damage, bool facing)
    {
        this.damage = damage;
        this.facing = facing;
        Random rand = new Random();
        RotationDegrees = rand.Next(-20, 21);
        float scaleMod = (float)Mathf.Clamp((0.4 + rand.NextDouble()) * 2, 0.8, 1.4);
        Scale = new Vector2(scaleMod, scaleMod);    
        if (facing)
            sprite.FlipH = true;
        else
            sprite.FlipH = false;   //just in case
        sprite.Play("default");
        lastSpeedScale = 1;
    }

    public void OnHitBoxAreaEntered(Node2D body)
    {
        if(body is Player)
            playerInArea = true;
    }
    public void OnHitBoxAreaExited(Node2D body)
    {
        if (body is Player)
            playerInArea = false;
    }
    public void OnStillTimerTimeout()
    {
        //play anim backwards
        sprite.Play("default", -1, true);
        lastSpeedScale = -1;
    }
    public void OnSpriteAnimationFinised()
    {
        //forward
        if (lastSpeedScale > 0)
            stillTimer.Start();
        //reverse
        else if(lastSpeedScale < 0)
            QueueFree();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        //this is allowed to be in the update loop because player has protection against attackspam (won't obliterate it)
        if(sprite.Frame >= 6 && playerInArea)
        {
            //apply full damage if anim is still progress (timeleft 0), or if it just finished (timeleft > 0.8)
            if (stillTimer.TimeLeft > 0.8 || stillTimer.TimeLeft == 0)
            { 
                Vector2 hitVector = new Vector2()
                {
                    X = facing ? -700 : 700,
                    Y = -400
                };

                Globals.player.Hit(damage, hitVector);
            }
            //apply reduced damage otherwise
            else
            {
                Vector2 hitVector = new Vector2()
                {
                    X = facing ? 100 : -100,
                    Y = -50
                };
                Globals.player.Hit(5, hitVector);
            }
        }
    }
}
