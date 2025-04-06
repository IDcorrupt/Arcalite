using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Transactions;

public partial class Enemy : CharacterBody2D
{
    //stats
    protected float maxHP;
    protected float currentHP;
    protected float damage;
    //item drop chance
    protected int shardDropRate;

    //external
    [Export] public bool isSlowed = false;
    [Export] public float slowFactor = 1;

    //values
    private bool jumped = false;
    private bool attacked = false;

    protected bool isDead = false;
    protected bool playerInAtkRange = false;
    protected bool isAttacking = false;
    protected bool isHurt = false;
    protected bool lostVision = false;
    protected bool bufferRan = true;
    protected int attackFrame = 0;
    protected float atkCD;
    
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
    private CollisionShape2D hitBox;
    private Area2D attackRange;
    private Area2D obstacleDetect;
    private CollisionPolygon2D obstacleDetectLeft;
    private CollisionPolygon2D obstacleDetectRight;
    private Area2D jumpTrigger;
    protected AnimatedSprite2D sprite;

    private RayCast2D lineOfSight;

    protected Player player;
    protected EnemyControl parent;
    private Node2D itemContainer;

    private PackedScene itemScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/item.tscn");


    public override void _Ready()
    {
        parent = (EnemyControl)GetParent();
        itemContainer = parent.GetParent().GetParent().GetNode("Items") as Node2D;
        sprite = GetNode<AnimatedSprite2D>("Sprite");
        player = Globals.player;

        dirTimer = GetNode<Timer>("DirectionTimer");
        RoamCooldown = GetNode<Timer>("RoamCooldown");
        atkCooldown = GetNode<Timer>("AttackCooldown");
        hurtTimer = GetNode<Timer>("HurtTimer");
        chaseBuffer = GetNode<Timer>("ChaseBuffer");

        chaseBuffer.WaitTime = 3f;

        hitBox = GetNode<CollisionShape2D>("CollisionShape2D");
        attackRange = GetNode<Area2D>("AttackRange");
        obstacleDetect = GetNode<Area2D>("ObstacleDetect");
        obstacleDetectLeft = GetNode<CollisionPolygon2D>("ObstacleDetect/left");
        obstacleDetectRight = GetNode<CollisionPolygon2D>("ObstacleDetect/right");
        jumpTrigger = GetNode<Area2D>("JumpTrigger");

        lineOfSight = GetNode("LineOfSight") as RayCast2D;
    }

    //movement
    public void Move(double delta)
    {
        //jump
        if(jumpTrigger.GetOverlappingBodies().Count == 0 && obstacleDetect.GetOverlappingBodies().Count >0 &&   //collisions
            !jumped &&          //can jump
            (Velocity.X > 0 || Velocity.X <0) &&     //is moving
            IsOnFloor())        //on ground
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y -(jumpStrength * slowFactor));
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
            if (Direction.X != 0)
            {
                if (speed < roamSpeed) speed += (float)delta * 70;
                else if (speed > roamSpeed+20) speed -= (float)delta * 70;
                Velocity = new Vector2(Direction.X * speed * slowFactor, Velocity.Y);
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

            Direction = GlobalPosition.DirectionTo(player.GlobalPosition);

            //regulate acceleration/deceleration
            if (speed < chaseSpeed) speed += (float)delta * 200;
            else if (speed > chaseSpeed) speed -= (float)delta * 200;

            //slowfactor added here
            Velocity = new Vector2(Direction.X*speed*slowFactor, Velocity.Y);
        }
    }
    private void Fall(double delta)
    {
        if (!IsOnFloor())
            Velocity = new Vector2(Velocity.X, Velocity.Y + (float)(Globals.GRAVITY * delta * slowFactor));
        else
            jumped = false;    //reset jump bool if on ground
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
            EngageAttack();
        else
            isAttacking = false;
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
    protected virtual void EngageAttack() 
    {
        Random atkRand = new Random();
        sprite.Play($"attack{atkRand.Next(1,3).ToString()}");
        isAttacking = true;
        attacked = false;
        Velocity = Vector2.Zero;
        speed = 0;
    }
    protected virtual void Attack()
    {
        //shell func, attack is different for each type
    }
    public virtual void Hit(float damage, Node2D attacker)
    {
        if (!isDead && !isHurt)
        {
            isHurt = true;
            currentHP -= damage;
            if (attacker != null && !isAttacking)
            {
                //knockback
                int dir = 0;
                if ((GlobalPosition - attacker.GlobalPosition).Normalized().X > 0)
                    dir = 1;
                else if ((GlobalPosition - attacker.GlobalPosition).Normalized().X < 0)
                    dir = -1;
                Velocity = new Vector2(dir * 200, 0);

                sprite.Play("hurt");
                hurtTimer.WaitTime = 0.5f;
                hurtTimer.Start();
            }
            else
                isHurt = false;
        }
    }
    private void OnHurtTimerTimeout() { isHurt = false; }
    public void OnChaseBufferTimeout() { isChasing = false; }

    //appearance
    protected virtual void Flip(bool dir)
    {
        if (dir)
        {
            //left
            sprite.FlipH = false;
            attackRange.RotationDegrees = 180;
            jumpTrigger.RotationDegrees = 180;
            obstacleDetectRight.Disabled = true;
            obstacleDetectLeft.Disabled = false;

        }
        else
        {
            //right
            sprite.FlipH = true;
            attackRange.RotationDegrees = 0;
            jumpTrigger.RotationDegrees = 0;
            obstacleDetectRight.Disabled = false;
            obstacleDetectLeft.Disabled = true;
        }
    }
    protected virtual void Animate()
    {
        if (!isAttacking && !(sprite.Animation == "hurt"))
        {
            if (atkCooldown.TimeLeft > 0)
                sprite.Play("idle");
            else if((IsOnFloor() && Velocity.X != 0) && !isDead)
                sprite.Play("walk");
            else if(Velocity.X == 0 && !isDead)
                sprite.Play("idle");
        }
    }
    public virtual void OnSpriteAnimationFinished()
    {
        if (sprite.Animation == "hurt")
            sprite.Play("idle");
        if (sprite.Animation == "attack1" || sprite.Animation == "attack2")
        {
            isAttacking = false;
            sprite.Play("idle");
            atkCooldown.Start(atkCD);
        }
        else if (sprite.Animation == "die")
        {
            sprite.Stop();
            Die();
        }
    }

    //die logic
    protected void DropItems(Enums.itemType itemtype = Enums.itemType.shard, int customDropRate = 0)
    {
        Item item = null;
        
        if (itemtype == Enums.itemType.shard)
        {
            int dropamount = 0;
            if (shardDropRate > 100)
            {
                dropamount = Mathf.FloorToInt(shardDropRate / 100);
                for (int i = 0; i < dropamount; i++)
                {
                    item = itemScene.Instantiate() as Item;
                    item.type = itemtype;
                    if (item is not null)
                    {
                        item.GlobalPosition = GlobalPosition;
                        itemContainer.AddChild(item);
                        item = null;
                    }
                }
            }
            //regular calc & drop
            if (Math.RNG(shardDropRate - dropamount * 100))
            {
                item = itemScene.Instantiate() as Item;
                item.type = itemtype;
            }
        }
        else if (Math.RNG(customDropRate))
        {
            item = itemScene.Instantiate() as Item;
            item.type = itemtype;
        }
        if (item is not null)
        {
            item.GlobalPosition = GlobalPosition;
            itemContainer.AddChild(item);
        }
    }
    protected virtual void Die()
    {
        parent.enemyAmount--;
        //empty item func call -> shard drop
        DropItems();
        QueueFree();

    }

    public virtual void Update(double delta)
    {
        Fall(delta);
        if (currentHP <=0)
            isDead = true;
        if (!isDead)
        {
            if (isChasing)
            {
                //update direction outside move(), so it flips while attacking
                Direction = GlobalPosition.DirectionTo(player.GlobalPosition);
            }
            if (isHurt)
                Velocity = new Vector2(Velocity.X > 0 ? Velocity.X - (float)delta * 700 : Velocity.X + (float)delta * 700, Velocity.Y);
            else if(isAttacking && !attacked && sprite.Frame >= attackFrame && playerInAtkRange)
            {
                //if engageAttack was called, but attack hasn't -> actual attacking if passed the specified frame
                Attack();
                attacked = true;
            }
            else if (isAttacking || atkCooldown.TimeLeft > 0)
            {
                //stop moving after attacking
                Velocity = new Vector2(0, Velocity.Y);
            }
            else if (!isAttacking)
            {
                //if not attacking & being attacked -> move, attack if in attack range
                Move(delta);
                if (playerInAtkRange && isChasing)
                    EngageAttack();
            }
            if (Direction.X < 0)
                Flip(true);
            else if(Direction.X > 0)
                Flip(false);
        }
        else
        {
            //remove enemyself layer from itself -> essentially disabling collisions without falling through the map (those work on collisions too)
            SetCollisionLayerValue(2, false);
            
            //fall and only die on floor
            if (IsOnFloor())
            {
                if(sprite.Animation != "die")
                {
                    Velocity = Vector2.Zero;
                    sprite.Play("die");
                }
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
