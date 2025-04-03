using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class SpellOracle : Node2D
{
    [Export] public Vector2 targetPosition;
    [Export] public int level;
    [Export] public float slowFactor;

    private bool slowing = false;
    private List<Node> affectedEntities = new List<Node>();

    Timer durationTimer;
    AnimatedSprite2D sprite;
    Area2D area;
    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        area = GetNode<Area2D>("Area2D");
        durationTimer = GetNode("Duration") as Timer;
        sprite.Play("cast");   
    }

    public void OnAnimationFinished()
    {
        if(sprite.Animation == "cast")
        {
            sprite.Play("loop");
            slowing = true;
            switch (level)
            {
                case 1:
                    durationTimer.WaitTime = 5;
                    break;
                case 2:
                    durationTimer.WaitTime = 10;
                    break;
                case 3:
                    durationTimer.WaitTime = 15;
                    break;
                case 4:
                    durationTimer.WaitTime = 20;
                    break;
                default:
                    durationTimer.WaitTime = 5;
                    break;
            }
            durationTimer.Start();
        }
        
        else if(sprite.Animation == "end")
        {
            TreeExit();
        }
    }

    public void OnDurationTimeout()
    {
        slowing = false;
        sprite.Play("end");
    }
    public void OnBodyEntered(Node2D body)
    {
        if (body is Enemy enemy)
            affectedEntities.Add(enemy);
        else if (body is CasterProjectile proj)
            affectedEntities.Add(proj);
        else if (body is BossMechArm arm)
            affectedEntities.Add(arm);
    }
    public void OnBodyExited(Node2D body)
    {
        if (body is Enemy enemy)
        {
            enemy.isSlowed = false;
            enemy.slowFactor = 1;
            affectedEntities.Remove(enemy);
        }
        else if (body is CasterProjectile proj)
        {
            proj.isSlowed = false;
            proj.slowFactor = 1;
            affectedEntities.Remove(proj);
        }
        else if (body is BossMechArm arm)
        {
            arm.isSlowed = false;
            arm.slowFactor = 1;
            affectedEntities.Remove(arm);   
        }
    }

    private void TreeExit()
    {
        foreach (Node obj in affectedEntities)
        {
            if (obj is Enemy enemy)
            {
                enemy.isSlowed = false;
                enemy.slowFactor = 1;
            }
            else if (obj is CasterProjectile castproj)
            {
                castproj.isSlowed = false;
                castproj.slowFactor = 1;
            }
            else if (obj is BossMechArm arm)
            {
                arm.isSlowed = false;
                arm.slowFactor = 1;
            }
        }
        QueueFree();
    }
    public override void _Process(double delta)
    {
        //cancel if player dies
        if(Globals.player.GetIsDead()) OnDurationTimeout();

        GlobalPosition = targetPosition;
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

            foreach (Node obj in affectedEntities)
            {
                if (obj is Enemy enemy)
                {
                    enemy.isSlowed = true;
                    enemy.slowFactor = slowFactor;
                }
                else if (obj is CasterProjectile castproj)
                {
                    castproj.isSlowed = true;
                    castproj.slowFactor = slowFactor;
                }
                else if (obj is BossMechArm arm)
                {
                    arm.isSlowed = true;
                    arm.slowFactor = slowFactor;
                }
            }
        }
        else
        {
            foreach (Node obj in affectedEntities)
            {
                if (obj is Enemy enemy)
                {
                    enemy.isSlowed = false;
                    enemy.slowFactor = 1;
                }
                else if (obj is CasterProjectile castproj)
                {
                    castproj.isSlowed = false;
                    castproj.slowFactor = 1;
                }
                else if (obj is BossMechArm arm)
                {
                    arm.isSlowed = false;
                    arm.slowFactor = 1;
                }
            }
        }
                
    }
}
