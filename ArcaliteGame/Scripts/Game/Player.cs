using Godot;
using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

public partial class Player : CharacterBody2D
{
    //values
        //crouch
        private int defHB_X = 16;       //hitbox for crouch
        private int defHB_Y = 29;       //hitbo for crouch
        private bool isCrouching = false;    //crouch bool (at least it should be bool but i didn't change it back)
        private bool crouchDown = false;    // up & down required for animations
        private bool crouchUp = false;      

        //movement
        private int maxSpeed = 300;         //maximum X vector value
        private double vel = 0;             //X velocity
        private int jumpStrength = 450;     //jump height/strength
        private const float GRAVITY = Globals.GRAVITY;
        private float prevDir = 0;          //last movement direction for deceleration
 
        //dash
        private float dashCooldown;        //dash cooldown constant
        private float dashDelta = 0f;           //dash cooldown remaining
        private bool dashed = false;            //dash initiated
        private float dashSpeed = 2000f;        //initial dash speed
        private float dashDecayRate = 15000f;   //dash speed decay rate
        private float currentDashSpeed = 0f;    //current dash speed
        private bool isDashing = false;         //is dash currently active
        private Vector2 dashVector;             //fixed vector for dash endpoint -> dash follows mouse otherwise :3

        //attacks
        private float BACooldown = 0.4f;    //basic attack cooldown constant
        private float BADelta = 0f;         //basic attack cooldown remaining
        private float CACooldown = 5f;      //charge attack cooldown constant
        private float CADelta = 0f;         //charge attack cooldown remaining
        private bool CAisCharging = false;  //is charge attack currently charging
        private float CACharge = 0f;        //used to track charge progress
        private int chargeLevel = 0;
        private float SOCooldown;           //Oracle cooldown constant
        private float SODelta = 0f;         //Oracle cooldown remaining

        //stats
        private float MaxHP = 100;
        private float MaxMP = 100;
        //spd & dot if class system get implemented
        private float ActualHP;
        private float ActualMP;

        private int oracleLevel;
    
        //nodes
        private CollisionShape2D HitBox;
        private PackedScene basicProjectile;
        private PackedScene chargeProjectile;
        private PackedScene spellOracle;
        private AnimatedSprite2D Sprite;
    
    public override void _Ready()
    {
        //Get nodes
        HitBox = GetNode<CollisionShape2D>("HitBox");
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
        
        direction.X = Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left"));
        if (direction.X == 1)Sprite.FlipH = false; 
        else if (direction.X == -1) Sprite.FlipH = true;
        if (Input.IsActionPressed("move_jump"))
        {
            if (IsOnFloor())
            {
                direction.Y = -1;
            }
        }
        if(Input.IsActionPressed("move_crouch")) isCrouching = true; else isCrouching = false;
        if (Input.IsActionPressed("move_dash") && dashDelta == 0) dashed = true;
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
            }else
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
            dashDelta = dashCooldown;
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
            Velocity = new Vector2(Velocity.X, Velocity.Y-jumpStrength);
        }

        
        if (dashDelta > 0)
        {
            dashDelta -= (float)delta;
        }else dashDelta = 0;
        CrouchApply();
        fall(delta);
        MoveAndSlide();
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
            Velocity = new Vector2 ((float)(Velocity.X/1.5), Velocity.Y);
            if (Velocity.Y > 0)
            {
                Velocity = new Vector2(Velocity.X, (float)(Velocity.Y * 1.2));
            }
            if (HitBox.Shape is RectangleShape2D rectangleShape)
            {
                HitBox.Position = new Vector2(HitBox.Position.X, (float)2.143);
                rectangleShape.Size = new Vector2(defHB_X, defHB_Y-5);
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
            projectile.damagePayload = 1;
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
            projectile.damagePayload = 1;
            projectile.imports = true;
        }
    }
    
    public void OracleSpell()
    {
        Node2D node = (Node2D)spellOracle.Instantiate();
        GetParent().GetParent().AddChild(node);
        if(node is SpellOracle oracle)
        {
            oracle.targetPosition = GlobalPosition;
            oracle.level = oracleLevel;
        }
    }

    //other functions
    private void SetStats()
    {
        if (Globals.hasSavefile)
        {
            //if loading from save
            //dont have save file yet 

        }
        else
        {
            //if new save
            MaxHP = 100;
            MaxMP = 100;
            ActualHP = MaxHP;
            ActualMP = MaxMP;
            oracleLevel = 4;
            dashCooldown = 2f;
            SOCooldown = 0f;
        }
    }
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



    public override void _PhysicsProcess(double delta)
    {
        if (Globals.playerControl)
        {

            if ((Input.IsActionPressed("attack_normal") || Input.IsActionPressed("attack_normal-alt")) && BADelta == 0 && !CAisCharging)
            {
                BasicAttack();
                BADelta = BACooldown;
            }
            if(Input.IsActionJustPressed("attack_charge") || Input.IsActionJustPressed("attack_charge-alt"))
            {
                CAisCharging = true;
            }
            if (Input.IsActionJustReleased("attack_charge") || Input.IsActionJustReleased("attack_charge-alt"))
            {
                CAisCharging = false;
            }
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
                if(CACharge > 40)
                {
                    ChargeAttack(chargeLevel);
                    CADelta = CACooldown;
                }
                CACharge = 0;
            }
            if ((Input.IsActionJustPressed("spell_oracle") || Input.IsActionJustPressed("spell_oracle-alt")) && SODelta == 0)
            {
                OracleSpell();
                SODelta = SOCooldown;
            }

            Movement(delta);
            Animate();
        }

        //cooldown resets
        if (BADelta > 0)
        {
            BADelta -= (float)delta;
        }else BADelta = 0;
        if (CADelta > 0)
        {
            CADelta -= (float)delta;
        }else CADelta = 0;
        if (SODelta > 0)
        {
            SODelta -= (float)delta;
        }else SODelta = 0;
    }

}
