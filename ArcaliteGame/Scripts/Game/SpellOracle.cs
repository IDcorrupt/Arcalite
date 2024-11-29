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
    private HashSet<Node> previouslyDetectedObjects = new HashSet<Node>();      //need to remove slow from exiting targets
    private HashSet<Node> currentObjects = new HashSet<Node>();


    AnimatedSprite2D sprite;
    Area2D area;
    ShapeCast2D slowArea;
    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        area = GetNode<Area2D>("Area2D");
        slowArea = GetNode<ShapeCast2D>("ShapeCast2D");
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

    public void Slow()
    {
        if (slowArea.IsColliding())
        {
            for (int i = 0; i < slowArea.GetCollisionCount(); i++)
            {
                Node collider = slowArea.GetCollider(i) as Node;
                currentObjects.Add(collider);

            }
        }


        //handle exits
        foreach (Node obj in previouslyDetectedObjects)
        {
            if (!currentObjects.Contains(obj))
            {
                if (obj is LightMeele Lmeele)
                {
                    Lmeele.isSlowed = false;
                }
            }
        }
        //handle enters
        foreach (Node obj in currentObjects)
        {
            if (!previouslyDetectedObjects.Contains(obj))
            {
                if (obj is LightMeele Lmeele)   //ADD HANDLING FOR OTHER TYPES OF ENEMIES WHEN THEY EXIST
                {
                    Lmeele.isSlowed = true;
                    Lmeele.slowFactor = slowFactor;
                }
            }
        }
    }

    public void OnTreeExit()
    {
        foreach (Node obj in currentObjects)
        {
            if (obj is LightMeele Lmeele)   //ADD HANDLING FOR OTHER TYPES OF ENEMIES WHEN THEY EXIST
            {
                Lmeele.isSlowed = false;
            }   
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
                    slowFactor = 0.75f;
                    break;
                case 2:
                    slowFactor = 0.5f;
                    break;
                case 3:
                    slowFactor = 0.25f;
                    break;
                case 4:
                    slowFactor = 0.1f;
                    break;
                default:
                    break;
            }
            Slow();

        }
    }
}
