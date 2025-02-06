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
    protected bool hasAttacked = false;
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

    public void Update(double delta)
    {
        if (!isDead)
        {
            if (isAttacking || atkCooldown.TimeLeft > 0)
            {
                //for debug indicator and movement stop
                if (isAttacking)
                    indicator.Modulate = Color.Color8(255, 0, 255, 255);
                else
                    indicator.Modulate = Color.Color8(255, 255, 0, 255);
                Velocity = new Vector2(0, Velocity.Y);
            }
            else if(!isAttacking || !isDamaged)
            {
                Move(delta);
                if (playerInAtkRange)
                {
                    GD.Print("sdijfhdosgjoi");
                    Attack();
                }
            }
            if (Direction.X < 0)
                Flip(true);
            else Flip(false);
        }
    }


    public void Move(double delta)
    {
        if (!IsOnFloor())
        {
            //if falling
            Velocity = new Vector2(0, Velocity.Y + (float)(Globals.GRAVITY * delta));
            return;
        }
        else
        {
            //moving
            if (!isChasing)
            {
                //idle
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
                        speed -= (float)delta * 100;
                    else
                    {
                        speed = 0;
                        prevDir = 0;
                    }
                    Velocity = new Vector2(prevDir, Velocity.Y);


                }
                isRoaming = true;
            }
            else if (isChasing)
            {
                //chasing
                //pause idle timers
                dirTimer.Paused = true;
                RoamCooldown.Paused = true;

                //debug indicator and direction calculation
                indicator.Modulate = Color.Color8(0, 0, 255, 255);
                Direction = GlobalPosition.DirectionTo(player.GlobalPosition);

                if (speed < chaseSpeed) speed += (float)delta * 200;
                Velocity = Direction * speed;
                //nullify vertical velocity for chase vel
                Velocity = new Vector2(Velocity.X, 0);
            }
        }

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
            sprite.Frame = 0;
            sprite.Play("attack");
            hasAttacked = false;
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
        }
    }
    public void AttackRangeBodyExited(Node2D body)
    {
        if (body is Player)
        {
            playerInAtkRange = false;
        }
    }
    protected virtual void Attack() 
    {
        isAttacking = true;
        GD.Print("attack");
        //shell func, attack is different for each type

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

    private void Animate()
    {
        if (isAttacking)
        {
            sprite.Play("attack");
        }
        else if(IsOnFloor() || Velocity.X > 0)
        {
            sprite.Play("walk");
        }
    }
    
    public void OnSpriteAnimationFinished()
    {
        if(sprite.Animation == "attack")
        {
            isAttacking = false;
            atkCooldown.Start();
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        //modifier for oracle functionality
        if (!isSlowed)
        {
            slowFactor = 1;
        }

        Update(delta);

        //apply slow
        Velocity *= slowFactor;

        Animate();
        MoveAndSlide();
    }



}
