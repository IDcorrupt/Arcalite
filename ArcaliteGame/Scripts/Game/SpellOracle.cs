using Godot;
using System;
using System.Collections.Generic;

public partial class SpellOracle : Node2D
{
    [Export] public Vector2 targetPosition;
    [Export] public int level;
    [Export] public float slowFactor;

    private bool slowing = false;
    private float slowDuration = 10;
    private List<Node> affectedEntities = new List<Node>();


    AnimatedSprite2D sprite;
    Area2D area;
    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        area = GetNode<Area2D>("Area2D");
        sprite.Play("cast");   
    }

    public void OnAnimationFinished()
    {
        if(sprite.Animation == "cast")
        {
            sprite.Position = new Vector2(2.245f, 1.275f);
            sprite.Scale = new Vector2(1.775f, 1.775f);
            sprite.Play("sustained_start");
            slowing = true;
            switch (level)
            {
                case 1:
                    slowDuration = 10;
                    break;
                case 2:
                    slowDuration = 20;
                    break;
                case 3:
                    slowDuration = 30;
                    break;
                case 4:
                    slowDuration = 40;
                    break;
                default:
                    break;
            }

        }
        else if(sprite.Animation == "sustained_start")
        {
            sprite.Play("sustained_middle");
        }
        else if(sprite.Animation == "sustained_end")
        {
            QueueFree();
        }
    }
    public void OnAnimationLooped()
    {
        if (slowing)
        {
            slowDuration -= 1;
        }
    }

    public void OnBodyEntered(Node2D body)
    {
        GD.PrintErr(body.Name+" entered");
        if (slowing)
        {
            if(body is Enemy enemy)
            {
                affectedEntities.Add(enemy);
                enemy.slowFactor = slowFactor;
                enemy.isSlowed = true;
            }
            else if(body is CasterProjectile proj)
            {
                affectedEntities.Add(proj);
                proj.slowFactor = slowFactor;
                proj.isSlowed = true;
            }
        }
    }
    public void OnBodyExited(Node2D body)
    {
        GD.PrintErr(body.Name+" exited");
        if (body is Enemy enemy)
        {
            GD.Print("enemy removed? " + affectedEntities.Remove((Enemy)enemy));
            enemy.slowFactor = 1;
            enemy.isSlowed = false;
        }
        else if (body is CasterProjectile proj)
        {
            GD.Print("projectile removed? " + affectedEntities.Remove((CasterProjectile)proj));
            proj.slowFactor = 1;
            proj.isSlowed = false;
        }
    }

    public void OnTreeExit()
    {
        foreach (Node obj in affectedEntities)
        {
            if (obj is Enemy enemy) 
                enemy.isSlowed = false;
            else if(obj is CasterProjectile castproj)
                castproj.isSlowed = false;
        }
    }
    public override void _Process(double delta)
    {
        GlobalPosition = targetPosition;

        if (slowDuration == 0)
        {
            slowing = false;
            sprite.Play("sustained_end");
        }
        if (slowing)
        {
            switch (level)
            {
                case 1:
                    slowFactor = 0.5f;
                    break;
                case 2:
                    slowFactor = 0.25f;
                    break;
                case 3:
                    slowFactor = 0.1f;
                    break;
                case 4:
                    slowFactor = 0.05f;
                    break;
                default:
                    break;
            }
        }
    }
}
