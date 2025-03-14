using Godot;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

public partial class Player : CharacterBody2D
{
//values
    //crouch
    private int defHB_X = 16;       //hitbox for crouch
    private int defHB_Y = 29;       //hitbox for crouch
    private bool isCrouching = false;    //crouch bool
    private bool crouchDown = false;    // up & down required for animations
    private bool crouchUp = false;

    //movement
    private int maxSpeed = 300;         //maximum X vector value
    private double vel = 0;             //X velocity
    private int jumpStrength = 450;     //jump height/strength
    private const float GRAVITY = Globals.GRAVITY;
    private float prevDir = 0;          //last movement direction for deceleration

    //dash
    private bool dashed = false;            //dash initiated
    private float dashSpeed = 2000f;        //initial dash speed
    private float dashDecayRate = 15000f;   //dash speed decay rate
    private float currentDashSpeed = 0f;    //current dash speed
    private bool isDashing = false;         //is dash currently active
    private Vector2 dashVector;             //fixed vector for dash endpoint -> dash follows mouse otherwise :3

    //attacks
    private bool CAisCharging = false;  //is charge attack currently charging
    private float CACharge = 0f;        //used to track charge progress
    private int chargeLevel = 0;

    //other
    private bool isHurt = false;
    private bool isDead = false;

//stats
    private float MaxHP = 100;
    private float MaxMP = 100;
    //spd & dot if class system get implemented
    private float currentHP;
    private float currentMP;

    private float damage;

    private int oracleLevel;

//nodes
    private CollisionShape2D HitBox;
    private PackedScene basicProjectile;
    private PackedScene chargeProjectile;
    private PackedScene spellOracle;
    private AnimatedSprite2D Sprite;

    private Timer hurtTimer;
    private Timer dashCooldown;
    //attackcooldowns
    private Timer BACooldown;
    private Timer CACooldown;
    private Timer SOCooldown;
    private Timer SECooldown;
    private Timer SQCooldown;

    public override void _Ready()
    {
        Globals.player = this;
        //Get nodes
        HitBox = GetNode<CollisionShape2D>("HitBox");
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        hurtTimer = GetNode<Timer>("HurtTimer");
        dashCooldown = GetNode("DashCooldown") as Timer;
        BACooldown = GetNode("BasicAttackCooldown") as Timer;
        CACooldown = GetNode("ChargeAttackCooldown") as Timer;
        SOCooldown = GetNode("OracleCooldown") as Timer;
        SECooldown = GetNode("SpellECooldown") as Timer;
        SQCooldown = GetNode("SpellQCooldown") as Timer;

        basicProjectile = (PackedScene)ResourceLoader.Load("res://Nodes/Game/basic_projectile.tscn");
        chargeProjectile = (PackedScene)ResourceLoader.Load("res://Nodes/Game/charge_projectile.tscn");
        spellOracle = (PackedScene)ResourceLoader.Load("res://Nodes/Game/spell_oracle.tscn");

        //set stats
        SetStats();

        //go to spawnpoint
        GlobalPosition = Globals.spawnPoint.Position;
    }


    //movement functions
    public Vector2 getInputs()
    {
        Vector2 direction = new();

        direction.X = Convert.ToInt32(Input.IsActionPressed("move_right")) + Convert.ToInt32(Input.IsActionPressed("move_right-alt")) - Convert.ToInt32(Input.IsActionPressed("move_left")) - Convert.ToInt32(Input.IsActionPressed("move_left-alt"));
        if (direction.X == 1) Sprite.FlipH = false;
        else if (direction.X == -1) Sprite.FlipH = true;
        if (Input.IsActionPressed("move_jump") || Input.IsActionPressed("move_jump-alt"))
        {
            if (IsOnFloor())
            {
                direction.Y = -1;
            }
        }
        if (Input.IsActionPressed("move_crouch") || Input.IsActionPressed("move_crouch-alt")) isCrouching = true; else isCrouching = false;
        if ((Input.IsActionPressed("move_dash") || Input.IsActionPressed("move_dash-alt")) && dashCooldown.TimeLeft == 0) dashed = true;
        return direction;
    }
    public void Movement(double delta)
    {
        //movement controls interrupted when dash is in progress
        if (isDashing)
        {
            if (currentDashSpeed > 0)
            {
                currentDashSpeed -= dashDecayRate * (float)delta;
                Velocity = dashVector * Mathf.Max(currentDashSpeed, 0);
            }
            else
            {
                isDashing = false;
            }
            MoveAndSlide();
            return;
        }

        //initiate dash
        if (dashed)
        {
            dashed = false;
            dashCooldown.Start();
            Dash();
        }

        //normal movement
        Vector2 input = getInputs();
        if (input.X != 0)
        {
            if (vel < maxSpeed) vel += delta * 2000;
            else if (vel > maxSpeed) vel -= delta * 2000;
            Velocity = new Vector2((float)(input.X * vel), Velocity.Y);
            prevDir = input.X;
        }
        else if (input.X == 0)
        {
            if (vel > 0)
            {
                vel -= delta * 2500;
            }
            else
            {
                vel = 0;
                prevDir = 0;
            }
            Velocity = new Vector2((float)(prevDir * vel), Velocity.Y);
        }
        if (input.Y != 0)
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y - jumpStrength);
        }

        CrouchApply();
        fall(delta);
    }
    public void Dash()
    {
        dashVector = (GetGlobalMousePosition() - GlobalPosition).Normalized();
        currentDashSpeed = dashSpeed;
        Velocity = dashVector * currentDashSpeed;
        isDashing = true;
        dashed = false;
    }
    public void fall(double delta)
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + (float)(GRAVITY * delta));
    }
    public void CrouchApply()
    {
        if (isCrouching)
        {
            Velocity = new Vector2((float)(Velocity.X / 1.5), Velocity.Y);
            if (Velocity.Y > 0)
            {
                Velocity = new Vector2(Velocity.X, (float)(Velocity.Y * 1.2));
            }
            if (HitBox.Shape is RectangleShape2D rectangleShape)
            {
                HitBox.Position = new Vector2(HitBox.Position.X, (float)2.143);
                rectangleShape.Size = new Vector2(defHB_X, defHB_Y - 5);
            }
        }
        else
        {
            if (HitBox.Shape is RectangleShape2D rectangleShape)
            {
                HitBox.Position = new Vector2(HitBox.Position.X, (float)-0.714);
                rectangleShape.Size = new Vector2(defHB_X, defHB_Y);

            }
        }

    }

    //attack functions
    public void BasicAttack()
    {
        Node2D node = (Node2D)basicProjectile.Instantiate();
        GetParent().GetParent().AddChild(node);
        if (node is BasicProjectile projectile)
        {
            projectile.Position = GlobalPosition;
            Vector2 direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
            projectile.Rotation = direction.Angle();
            projectile.direction = direction;
            projectile.damagePayload = damage;
        }

    }
    public void ChargeAttack(int chargeLevel)
    {
        Node2D node = (Node2D)chargeProjectile.Instantiate();
        GetParent().GetParent().AddChild(node);
        if (node is ChargeProjectile projectile)
        {
            projectile.Position = GlobalPosition;
            Vector2 direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
            projectile.chargeLevel = chargeLevel;
            projectile.Rotation = direction.Angle();
            projectile.direction = direction;
            projectile.damagePayload = damage;
            projectile.imports = true;
        }
    }
    public void OracleSpell()
    {
        Node2D node = (Node2D)spellOracle.Instantiate();
        GetParent().AddChild(node);
        if (node is SpellOracle oracle)
        {
            oracle.targetPosition = GlobalPosition;
            oracle.level = oracleLevel;
        }
    }

    //damage functions
    public void Hit(float damage, Vector2 hitVector)
    {
        if (!isHurt)
        {
            currentHP -= damage;
            //reset dash speed to avoid dash buffering
            currentDashSpeed = 0;
            if(currentHP <= 0)
            {
                isDead = true;
                return;
            }
            isHurt = true;
            if (hitVector != Vector2.Zero)
            {
                Velocity = hitVector;
                hurtTimer.WaitTime = 1;
            }
            else
            {
                hurtTimer.WaitTime = 0.5f;
            }
            hurtTimer.Start();

        }
    }

    //other functions
    public void SetStats()
    {
        if (Globals.hasSavefile)
        {
            //if loading from save
            //dont have save file yet 




        }
        else
        {
            //if new save
            //stats
            MaxHP = 100;
            MaxMP = 100;
            currentHP = MaxHP;
            currentMP = MaxMP;
            damage = 10;
            oracleLevel = 2;

            //cooldowns
            dashCooldown.WaitTime = 2f;
            BACooldown.WaitTime = 0.2f;
            CACooldown.WaitTime = 1f;
            SOCooldown.WaitTime = 10f;
            SECooldown.WaitTime = 5f;
            SQCooldown.WaitTime = 5f;
        }
        //reset state
        vel = 0;
        prevDir = 0;
        Velocity = Vector2.Zero;
        isHurt = false;
        isDead = false;
        isDashing = false;
        isCrouching = false;
        GlobalPosition = Globals.spawnPoint.Position;
    }

    //signal functions
    public void OnSpriteAnimationFinished()
    {
        if(Sprite.Animation == "die")
        { 
            //////////////////////////
        }
    }
    public void OnHurtTimerTimeout() { isHurt = false; }


    //other functions
    private void Animate()
    {
        //jump & fall
        if (!IsOnFloor() && Velocity.Y < 0)
        {
            Sprite.Play("jump");
        }
        else if (!IsOnFloor() && Velocity.Y > 0)
        {
            Sprite.Play("fall");
        }
        else if (IsOnFloor())
        {
            //crouch
            RectangleShape2D HBShape = (RectangleShape2D)HitBox.Shape;
            if (isCrouching && Sprite.Animation != "crouch")
            {
                crouchDown = true;
                Sprite.Play("crouch");
            }
            else if (!isCrouching && Sprite.Animation == "crouch" && crouchUp == false)
            {
                crouchUp = true;
                crouchDown = false;
                Sprite.Play("crouch", -1, true);
            }
            if (Sprite.Animation == "crouch" && crouchUp && Sprite.Frame == 0)
            {
                crouchUp = false;
            }

            //walk
            if (!crouchDown && !crouchUp)
            {
                if (Velocity.X != 0 && Sprite.Animation != "walk") Sprite.Play("walk");
                if (Velocity.X == 0 && Sprite.Animation != "idle")
                {
                    Sprite.Play("idle");
                }
            }
            else
            {
                //crouch walk anims here
            }
        }
    }
    private void Update(double delta)
    {
        if (!isDead)
        {
            //attacks
            if ((Input.IsActionPressed("attack_normal") || Input.IsActionPressed("attack_normal-alt")) && BACooldown.TimeLeft == 0 && !CAisCharging)
            {
                BasicAttack();
                BACooldown.Start();
            }
            //CA charge sequence control
            if ((Input.IsActionJustPressed("attack_charge") || Input.IsActionJustPressed("attack_charge-alt")) && CACooldown.TimeLeft == 0)
            {
                CAisCharging = true;
            }
            if (Input.IsActionJustReleased("attack_charge") || Input.IsActionJustReleased("attack_charge-alt"))
            {
                CAisCharging = false;
            }

            //charging & appropriate level
            if (CAisCharging)
            {
                CACharge += Mathf.Round((float)delta * 50);
                if (CACharge >= 100)
                {
                    chargeLevel = 4;
                }
                else if (CACharge >= 80)
                {
                    chargeLevel = 3;
                }
                else if (CACharge >= 60)
                {
                    chargeLevel = 2;
                }
                else if (CACharge >= 40)
                {
                    chargeLevel = 1;
                }

            }
            else
            {
                //launch CA
                if (CACharge > 40)
                {
                    ChargeAttack(chargeLevel);
                    CACooldown.Start();
                }
                CACharge = 0;
            }
            //oracle
            if ((Input.IsActionJustPressed("spell_oracle") || Input.IsActionJustPressed("spell_oracle-alt")) && SOCooldown.TimeLeft == 0)
            {
                OracleSpell();
                SOCooldown.Start();
            }

            //hit knockback management
            if (hurtTimer.TimeLeft > 0.5)
            {
                Velocity = new Vector2(
                    Velocity.X > 0 ? Mathf.Max(0,Velocity.X-(float)delta*700) 
                                   : Mathf.Min(0,Velocity.X+(float)delta*700),
                    Velocity.Y
                );
                fall(delta);
            }
            else
            {
                Movement(delta);
            }
            Animate();
            MoveAndSlide();

        }
        else
        {
            if(Sprite.Animation != "die")
                Sprite.Play("die");
        }
    }

    //getters/setters
    public bool GetIsDead() { return isDead; }
    public float GetHP() { return currentHP; }
    public float GetMP() { return currentMP; }
    

    //deltaloop
    public override void _PhysicsProcess(double delta)
    {
        if (Globals.playerControl)
        {
            Update(delta);
        }
    }
}
