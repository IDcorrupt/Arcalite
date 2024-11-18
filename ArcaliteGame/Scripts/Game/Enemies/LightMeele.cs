using Godot;
using System;
using System.Runtime.CompilerServices;

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
    private Vector2 prevDir =new Vector2(0,0);
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
                    GD.Print("accel");
                    if (speed < roamSpeed) speed += (float)delta * 40;
                    else if (speed > roamSpeed) speed -= (float)delta * 40;
                    Velocity = new Vector2((float)(Direction.X * speed), Velocity.Y);
                    Velocity = Direction * speed;
                    prevDir = Direction;
                }else if (Direction.X == 0)
                {
                    if (speed > 0)
                    {
                        GD.Print("decel");
                        speed -= (float)delta * 1000;
                    }else
                    {
                        GD.Print("idle");
                        speed = 0;
                        prevDir = new Vector2(0,prevDir.Y);
                    }
                    Velocity = prevDir * speed;


                }
                isRoaming = true;
            }
        }
        else Velocity = new Vector2(0, Velocity.Y);
    }
    public void OnDirectionTimerTimeout()
    {
        dirTimer.WaitTime = (new Random().Next(1,8))/2;
        GD.Print($"dirtimer timeout, next timer: {dirTimer.WaitTime}");
        if (!isChasing)
        {
            Direction = new Vector2(0, Direction.Y);
            RoamCooldown.Start();
        }
    }
    public void OnRoamCoolDownTimeout()
    {
        GD.Print("roamcooldown timeout");
        Direction = new Vector2(dirChoose(), Direction.Y);
        Velocity = new Vector2(0, Velocity.Y);
        dirTimer.Start();
    }
    private float dirChoose()
    {
        int[] array = { 1, -1 };
        int rand = new Random().Next(0,2);
        return array[rand];
    }



    public override void _Process(double delta)
    {
        //modifier for oracle functionality
        if (!isSlowed)
        {
            slowFactor = 1;
        }

        if (!IsOnFloor())
        {
            Velocity = new Vector2(0, Velocity.Y + 100 * Globals.GRAVITY * (float)delta);
        }
        Move(delta);
        MoveAndSlide();
    }
}
