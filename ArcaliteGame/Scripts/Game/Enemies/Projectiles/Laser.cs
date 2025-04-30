using Godot;
using System;
using System.ComponentModel.Design;

public partial class Laser : Node2D
{
    private Node2D ray;
    private AnimatedSprite2D startAnim;
    private AnimatedSprite2D impactAnim;
    private float speedScale = 1;



    public override void _Ready()
    {
        base._Ready();
        ray = GetNode("Ray") as Node2D;
        startAnim = GetNode("Ray/StartAnim") as AnimatedSprite2D;
        impactAnim = GetNode("EndAnim") as AnimatedSprite2D;

        speedScale = 1;
        startAnim.Play("start_cast");
        impactAnim.Play("impact_cast");
        foreach (AnimatedSprite2D sprite in GetTree().GetNodesInGroup("laserAnims"))
        {
            sprite.Play("middle_cast");
        }
    }
    public void RotateBeam(float raidans)
    {
        ray.Rotation = raidans;
    }
    public void SetImpactPoint(Vector2 point)
    {
        impactAnim.GlobalPosition = point;
    }

    public void OnAnimationFinished()
    {
        if(speedScale > 0) {
            startAnim.Play("start_sustained");
            impactAnim.Play("impact_sustained");
            foreach (AnimatedSprite2D sprite in GetTree().GetNodesInGroup("laserAnims"))
            {
                sprite.Play("middle_sustained");
            }
        }
        else
            QueueFree();
        
    }
    public void TurnOff()
    {
        speedScale = -1;
        startAnim.PlayBackwards("start_cast");
        impactAnim.PlayBackwards("impact_cast");
        foreach (AnimatedSprite2D sprite in GetTree().GetNodesInGroup("laserAnims"))
        {
            sprite.PlayBackwards("middle_cast");
        }
    }
}
