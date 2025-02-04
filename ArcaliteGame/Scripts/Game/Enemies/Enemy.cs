using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    //stats
    protected float maxHP;
    protected float currentHP;
    protected float damage;

    //external
    [Export] protected int namenum;
    [Export] public bool isSlowed = false;
    [Export] public float slowFactor = 1;

    //values
    protected bool isDead = false;
    protected bool playerInAtkRange = false;
    protected bool isAttacking = false;
    protected bool isDamaged = false;

    public bool isChasing;
    public bool isRoaming = true;
    public Vector2 Direction;
    public float roamSpeed = 30f;
    public float chaseSpeed = 120f;
    public float speed = 0f;
    
    protected float prevDir = 0;

    //components
    protected Timer dirTimer;
    protected Timer RoamCooldown;
    protected Timer atkCooldown;
    private Sprite2D indicator;
    private Area2D attackRange;
    private AnimatedSprite2D sprite;

    protected Player player;
    protected EnemyControl parent;

    public override void _Ready()
    {
        parent = (EnemyControl)GetParent();
        sprite = GetNode<AnimatedSprite2D>("Sprite");
        player = Globals.Player;

        dirTimer = GetNode<Timer>("DirectionTimer");
        RoamCooldown = GetNode<Timer>("RoamCooldown");
        atkCooldown = GetNode<Timer>("AttackCooldown");
        
        attackRange = GetNode<Area2D>("AttackRange");
        
        //debug
        indicator = GetNode<Sprite2D>("indicator");
    }


    //movement

    public void Move(double delta)
    {
        if (!isDead)
        {
            if (!isChasing)
            {
                if (dirTimer.Paused)
                    dirTimer.Paused = false;
                if (RoamCooldown.Paused)
                    RoamCooldown.Paused = false;
                indicator.Modulate = Color.Color8(255, 0, 0, 255);
                if (Direction.X != 0)
                {
                    if (speed < roamSpeed) speed += (float)delta * 70;
                    Velocity = Direction * speed;
                    prevDir = Direction.X;
                }
                else if (Direction.X == 0)
                {
                    if (speed > 0)
                    {
                        speed -= (float)delta * 100;
                    }
                    else
                    {
                        speed = 0;
                        prevDir = 0;
                    }
                    Velocity = new Vector2(prevDir, Velocity.Y);


                }
                isRoaming = true;
            }
            else if (isChasing && !isDamaged)
            {
                dirTimer.Paused = true;
                RoamCooldown.Paused = true;
                if (isAttacking)
                {
                    indicator.Modulate = Color.Color8(255, 0, 255, 255);
                    Velocity = new Vector2(0, Velocity.Y);
                    //attack
                    //[TBD] WILL BE BASED ON ANIM END, CURRENTLY JUST THE COOLDOWN TIMER EXTENDED
                }
                else
                {
                    indicator.Modulate = Color.Color8(0, 0, 255, 255);
                    Direction = GlobalPosition.DirectionTo(player.GlobalPosition);
                    if (Direction.X < 0)
                        Flip(true);
                    else Flip(false);
                    if (speed < chaseSpeed) speed += (float)delta * 200;
                    Velocity = Direction * speed;
                    //nullify vertical velocity for chase vel
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }
        }
        else Velocity = new Vector2(0, Velocity.Y);
    }

    //idle logic
    public void OnDirectionTimerTimeout()
    {
        double rand = new Random().Next(1, 8);
        dirTimer.WaitTime = rand / 2;
        if (!isChasing)
        {
            Direction = new Vector2(0, Velocity.Y);
            RoamCooldown.Start();
        }
    }
    public void OnRoamCoolDownTimeout()
    {
        Direction = new Vector2(dirChoose(), Velocity.Y);
        if (Direction.X < 0)
            Flip(true);
        else Flip(false);
        Velocity = new Vector2(0, Velocity.Y);
        dirTimer.Start();
    }
    private float dirChoose()
    {
        int[] array = { 1, -1 };
        int rand = new Random().Next(0, 2);
        return array[rand];
    }


    //attack logic
    private void AtkCooldownTimeout()
    {
        if (playerInAtkRange)
        {
            atkCooldown.Start();
        }
        else
        {
            isAttacking = false;
        }

    }
    public void AttackRangeBodyEntered(Node2D body)
    {
        if (body is Player)
        {
            playerInAtkRange = true;
            if (!isDamaged && !isAttacking)
            {
                atkCooldown.Start();
                isAttacking = true;
            }
        }
    }
    public void AttackRangeBodyExited(Node2D body)
    {
        if (body is Player)
        {
            playerInAtkRange = false;
        }
    }

    //appearance
    private void Flip(bool dir)
    {
        if (dir)
        {
            //right
            sprite.FlipH = true;
            indicator.Position = new Vector2(-6, -16);
            attackRange.RotationDegrees = 180;
        }
        else
        {
            sprite.FlipH = false;
            indicator.Position = new Vector2(6, -16);
            attackRange.RotationDegrees = 0;
        }
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

        //debug
        //debug


        //apply slow
        Velocity *= slowFactor;
        MoveAndSlide();
    }



}
