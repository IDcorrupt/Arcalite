using Godot;
using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

public partial class LightMeele : CharacterBody2D
{
    public float maxHP;
    public float actualHP;
    public float damage;

    [Export] int namenum;

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
    public float chaseSpeed = 60f;
    public float speed = 0f;

    Timer dirTimer;
    Timer RoamCooldown;

    Player player;

    EnemyControl parent;

    public override void _Ready()
    {
        
        parent = (EnemyControl)GetParent();
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
            }else if (isChasing && !isDamaged)
            {
                //Direction = Position.DirectionTo()
                if (speed < chaseSpeed) speed += (float)delta * 70;
                Velocity = Direction * speed;
            }
        }
        else Velocity = new Vector2(0, Velocity.Y);
    }
    public void OnDirectionTimerTimeout()
    {
        double rand = new Random().Next(1, 8);
        dirTimer.WaitTime = rand/2;
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

        //apply slow
        Velocity *= slowFactor;
        MoveAndSlide();
    }
}
