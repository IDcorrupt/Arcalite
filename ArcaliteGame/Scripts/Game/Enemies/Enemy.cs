using Godot;
using System;
using System.Transactions;

public partial class Enemy : CharacterBody2D
{
    //stats
    protected float maxHP;
    protected float currentHP;
    protected float damage;

    //external
    //[Export] protected int namenum;
    [Export] public bool isSlowed = false;
    [Export] public float slowFactor = 1;

    //values
    private bool jumped = false;

    protected bool isDead = false;
    protected bool playerInAtkRange = false;
    protected bool isAttacking = false;
    protected bool isHurt = false;
    protected bool lostVision = false;
    protected bool bufferRan = true;
    
    public bool isChasing;
    public bool isRoaming = true;
    public Vector2 Direction;
    protected float roamSpeed = 30f;
    protected float chaseSpeed = 120f;
    protected float speed = 0f;
    protected float jumpStrength;
    
    protected float prevDir = 0;
    protected Vector2 hitVector;

    //components
    protected Timer dirTimer;
    protected Timer RoamCooldown;
    protected Timer atkCooldown;
    protected Timer hurtTimer;
    protected Timer chaseBuffer;
    private Sprite2D indicator;
    private Area2D attackRange;
    private Area2D obstacleDetect;
    private CollisionPolygon2D obstacleDetectLeft;
    private CollisionPolygon2D obstacleDetectRight;
    private Area2D jumpTrigger;
    protected AnimatedSprite2D sprite;

    private RayCast2D lineOfSight;

    protected Player player;
    protected EnemyControl parent;


    public override void _Ready()
    {
        parent = (EnemyControl)GetParent();
        sprite = GetNode<AnimatedSprite2D>("Sprite");
        player = Globals.player;

        dirTimer = GetNode<Timer>("DirectionTimer");
        RoamCooldown = GetNode<Timer>("RoamCooldown");
        atkCooldown = GetNode<Timer>("AttackCooldown");
        hurtTimer = GetNode<Timer>("HurtTimer");
        chaseBuffer = GetNode<Timer>("ChaseBuffer");

        chaseBuffer.WaitTime = 3f;
        
        attackRange = GetNode<Area2D>("AttackRange");
        obstacleDetect = GetNode<Area2D>("ObstacleDetect");
        obstacleDetectLeft = GetNode<CollisionPolygon2D>("ObstacleDetect/left");
        obstacleDetectRight = GetNode<CollisionPolygon2D>("ObstacleDetect/right");
        jumpTrigger = GetNode<Area2D>("JumpTrigger");

        lineOfSight = GetNode("LineOfSight") as RayCast2D;

        //debug
        indicator = GetNode<Sprite2D>("indicator");
    }





    //movement
    public void Move(double delta)
    {
        //jump
        if(jumpTrigger.GetOverlappingBodies().Count == 0 && obstacleDetect.GetOverlappingBodies().Count >0 &&   //collisions
            !jumped &&          //can jump
            (Velocity.X > 0 || Velocity.X <0))     //is moving
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y -jumpStrength);
            jumped = true;
        }
        //moving
        if (!isChasing)
        {
            //idle
            if (dirTimer.Paused)
            {
                dirTimer.Paused = false;
                OnDirectionTimerTimeout();
                dirTimer.Start();
                Velocity = Vector2.Zero; //adding this here so it only runs once on disengage
            }
            if (RoamCooldown.Paused)
            {
                RoamCooldown.Paused = false;
            }
            indicator.Modulate = Color.Color8(30,30,30,30);
            if (Direction.X != 0)
            {
                if (speed < roamSpeed) speed += (float)delta * 70;
                else if (speed > roamSpeed+20) speed -= (float)delta * 70;
                Velocity = new Vector2(Direction.X * speed * slowFactor, Velocity.Y * slowFactor);
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
                Velocity = new Vector2(prevDir * speed, Velocity.Y);


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
            indicator.Modulate = Color.Color8(0,0, 255);
            Direction = GlobalPosition.DirectionTo(player.GlobalPosition);

            if (speed < chaseSpeed) speed += (float)delta * 200;
            else if (speed > chaseSpeed) speed -= (float)delta * 200;
            //slowfactor added here
            Velocity = new Vector2(Direction.X*speed*slowFactor, Velocity.Y*slowFactor);
        }
    }
    private void Fall(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + (float)(Globals.GRAVITY * delta));
            jumped = true;
        }
        else
        {
            jumped = false;    //reset jump bool if on ground
        }
    }
    //idle logic
    public void OnDirectionTimerTimeout()
    {
        double rand = new Random().Next(1, 8);
        dirTimer.WaitTime = rand / 2;
        Direction = new Vector2(0, Velocity.Y);
        RoamCooldown.Start();
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
        if (playerInAtkRange && !player.GetIsDead())
        {
            Attack();
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
        //shell func, attack is different for each type
        isAttacking = true;
        sprite.Play("attack");
        Velocity = Vector2.Zero;
        speed = 0;

    }
    public virtual void Hit(float damage, Node2D attacker)
    {
        if (!isDead && !isHurt)
        {
            isHurt = true;
            currentHP -= damage;
            if(attacker != null)
            {
                //knockback
                int dir = 0;
                if ((GlobalPosition - attacker.GlobalPosition).Normalized().X > 0)
                    dir = 1;
                else if ((GlobalPosition - attacker.GlobalPosition).Normalized().X < 0)
                    dir = -1;
                Velocity = new Vector2(dir * 200, 0);


                hurtTimer.WaitTime = 1;
            }
            else
            {
                hurtTimer.WaitTime = 0.2f;
            }
            hurtTimer.Start();
        }
        GD.Print(this.Name + " hp left: " + currentHP);
    }
    private void OnHurtTimerTimeout() { isHurt = false; }
    public void OnChaseBufferTimeout() { isChasing = false; }
    //appearance
    private void Flip(bool dir)
    {
        if (dir)
        {
            //right
            sprite.FlipH = true;
            indicator.Position = new Vector2(-6, -16);
            attackRange.RotationDegrees = 180;
            jumpTrigger.RotationDegrees = 180;
            obstacleDetectRight.Disabled = true;
            obstacleDetectLeft.Disabled = false;

        }
        else
        {
            sprite.FlipH = false;
            indicator.Position = new Vector2(6, -16);
            attackRange.RotationDegrees = 0;
            jumpTrigger.RotationDegrees = 0;
            obstacleDetectRight.Disabled = false;
            obstacleDetectLeft.Disabled = true;
        }
    }
    protected virtual void Animate()
    {
        if (!isAttacking)
        {
            if (atkCooldown.TimeLeft > 0)
                sprite.Play("idle");
            else if((IsOnFloor() && Velocity.X > 1) && !isDead)
                sprite.Play("walk");
            else if(Velocity.X == 0 && !isDead)
                sprite.Play("idle");
        }
    }
    public void OnSpriteAnimationFinished()
    {
        if (sprite.Animation == "attack")
        {
            isAttacking = false;
            sprite.Play("idle");
            atkCooldown.Start();
        }
        else if (sprite.Animation == "die")
            Die();
    }


    private void Die()
    {
        parent.enemyAmount--;
        QueueFree();    
    }

    public virtual void Update(double delta)
    {
        Fall(delta);
        if (currentHP <=0)
            isDead = true;
        if (!isDead)
        {
            if (isHurt)
                Velocity = new Vector2(Velocity.X > 0 ? Velocity.X - (float)delta * 700 : Velocity.X + (float)delta * 700, Velocity.Y);
            if (isAttacking || atkCooldown.TimeLeft > 0)
            {
                //for debug indicator and movement stop
                if (isAttacking)
                    indicator.Modulate = Color.Color8(255, 0, 255, 255);
                else
                    indicator.Modulate = Color.Color8(255, 255, 0, 255);
                //stop moving after attacking
                Velocity = new Vector2(0, Velocity.Y);
            }
            else if (!isAttacking)
            {
                //if not attacking & being attacked -> move, attack if in attack range
                Move(delta);
                if (playerInAtkRange && isChasing)
                    Attack();
            }
            if (Direction.X < 0)
                Flip(true);
            else if(Direction.X > 0)
                Flip(false);
        }
        else
        {
            //fall and only die on floor
            if (IsOnFloor())
            {
                Velocity = Vector2.Zero;
                sprite.Play("die");
            }else
            {
                Velocity = new Vector2(Velocity.X - 200 * (float)delta, Velocity.Y);
            }
        }

        //strict disengage if player is dead
        if(player.GetIsDead())
            isChasing = false;
        else
        {
            //update line of sight
            lineOfSight.TargetPosition = player.GlobalPosition - GlobalPosition;
            if (lineOfSight.IsColliding() && lineOfSight.GetCollider() is Player)
            {
                bufferRan = false;
                isChasing = true;
            }
            else
                lostVision = true;
            if (lostVision && !bufferRan)
            {
                bufferRan = true;
                chaseBuffer.Start();
            }
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
        MoveAndSlide();
        Animate();
    }
}
