using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

public partial class LightMeele : CharacterBody2D
{
    public float maxHP;
    public float actualHP;
    public float damage;

    [Export] public bool isSlowed = false;
    [Export] public float slowFactor = 1;
    public bool isChasing;
    public bool isRoaming = true;
    public bool isDead = false;
    public bool isDamaging = false;
    public bool isDamaged = false;

    public Vector2 Direction;
    private float prevDir = 0;
    public float roamSpeed = 30f;
    public float speed = 0f;

    Timer dirTimer;
    Timer RoamCooldown;



    public override void _Ready()
    {
        dirTimer = GetNode<Timer>("DirectionTimer");
        RoamCooldown = GetNode<Timer>("RoamCooldown");
    }


    //movement

    public void Move(double delta)
    {
        if (!isDead)
        {
            if (!isChasing)
            {
                if (Direction.X != 0)
                {
                    if (speed < roamSpeed) speed += (float)delta * 70;
                    Velocity = new Vector2((float)(Direction.X * speed), Velocity.Y);
                    Velocity = Direction * speed;
                    prevDir = Direction.X;
                }else if (Direction.X == 0)
                {
                    if (speed > 0)
                    {
                        speed -= (float)delta * 100;
                    }else
                    {
                        speed = 0;
                        prevDir = 0;
                    }
                    Velocity = new Vector2(prevDir, Velocity.Y);


                }
                isRoaming = true;
            }
        }
        else Velocity = new Vector2(0, Velocity.Y);
    }
    public void OnDirectionTimerTimeout()
    {
        dirTimer.WaitTime = (new Random().Next(1,8))/2;
        if (!isChasing)
        {
            Direction = new Vector2(0, Velocity.Y);
            RoamCooldown.Start();
        }
    }
    public void OnRoamCoolDownTimeout()
    {
        Direction = new Vector2(dirChoose(), Velocity.Y);
        Velocity = new Vector2(0, Velocity.Y);
        dirTimer.Start();
    }
    private float dirChoose()
    {
        int[] array = { 1, -1 };
        int rand = new Random().Next(0,2);
        return array[rand];
    }



    public override void _PhysicsProcess(double delta)
    {
        //modifier for oracle functionality
        if (!isSlowed)
        {
            slowFactor = 1;
        }

        if (!IsOnFloor())
        {
            Velocity = new Vector2(0, Velocity.Y + (float)(Globals.GRAVITY * delta));
        }
        else if (IsOnFloor())
        {
            Velocity = new Vector2(Velocity.X, 0);
            Move(delta);
        }
        MoveAndSlide();
    }
}
