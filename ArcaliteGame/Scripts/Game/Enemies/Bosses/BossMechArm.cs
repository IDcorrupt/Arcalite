using Godot;
using System;
using System.Linq.Expressions;

public partial class BossMechArm : CharacterBody2D
{
    private AnimatedSprite2D sprite;
    private CollisionShape2D hitBox;
    private Timer launchSequenceTimer;

    //true if right, false if left
    [Export] private bool SIDE;
    private int launchSequenceStep = 0;
    private Vector2 origin;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode("Sprite") as AnimatedSprite2D;
        hitBox = GetNode("CollisionShape2D") as CollisionShape2D;
        launchSequenceTimer = GetNode("LaunchSequenceTimer") as Timer;
        launchSequenceTimer.Timeout += LaunchSequenceTimer_Timeout;

        origin = Position;
    }


    public void LaunchSequence()
    {
        GD.Print("arm launcsequence: "+launchSequenceStep);
        if (SIDE)
        {
            switch (launchSequenceStep)
            {
                case 0:
                    launchSequenceTimer.Start(2f);
                    break;
                case 1:
                    //CollisionMask |= 3;
                    //CollisionMask |= 8;
                    launchSequenceTimer.Start();
                    break;

                default: 
                    break;
            }
        }
       /* else
        {
            switch (launchSequenceStep)
            {
                case 0:
                    launchSequenceTimer.Start();
                    break;
                case 1:
                    CollisionMask |= 3;
                    CollisionMask |= 8;
                    Velocity = new Vector2(0, 300);
                    launchSequenceTimer.Start();
                    break;
                default:
                    break;
            }
        }*/
    }
    private void LaunchSequenceTimer_Timeout() { launchSequenceStep++; GD.Print("arm sequence timer timeout"); LaunchSequence(); }

    private void Animate()
    {

    }
    
    private void IdleMove()
    {

    }

    private void Update(double delta)
    {


        Animate();
    }




    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Update(delta);
        MoveAndSlide();
    }
}
