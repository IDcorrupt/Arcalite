using Godot;
using System;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

public partial class Player : CharacterBody2D
{
    //values

    //crouch
    private int defHB_X = 16;       //hitbox for crouch
    private int defHB_Y = 38;       //hitbo for crouch
    private int isCrouching = 1;    //crouch bool (at least it should be bool but i didn't change it back)
    //movement
    private int max_speed = 300;        //maximum X vector value
    private double vel = 0;             //X velocity
    private int jump_strength = 450;    //jump height/strenght
    private float GRAVITY = 1500f;      //gravity, duh
    private float prevDir = 0;          //last movement direction for deceleration

    
    //dash
    private float dashCooldown = 1f;        //dash cooldown constant
    private float dashDelta = 0f;           //dash cooldown remaining
    private bool dashed = false;            //dash initiated
    private float dashSpeed = 2000f;        //initial dash speed
    private float dashDecayRate = 15000f;    //dash speed decay rate
    private float currentDashSpeed = 0f;    //current dash speed
    private bool isDashing = false;         //is dash currently active
    private Vector2 dashVector;             //fixed vector for dash endpoint -> dash follows mouse otherwise :3

    //attacks
    private float damage;               //damage value [CALCULATIONS TBD]
    private float BACooldown = 0.2f;    //basic attack cooldown constant
    private float BADelta = 0f;         //basic attack cooldown remaining
    private float CACooldown = 2f;      //charge attack cooldown constant
    private float CADelta = 0f;         //charge attack cooldown remaining
    private bool CACharging = false;    //is charge attack currently charging

    //nodes
    private CollisionShape2D HitBox;
    private PackedScene basicProjectile;
    private AnimatedSprite2D Sprite;
    
    public override void _Ready()
    {
        //Get nodes
        HitBox = GetNode<CollisionShape2D>("HitBox");
        Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        basicProjectile = (PackedScene)ResourceLoader.Load("res://Nodes/Game/basic_projectile.tscn");
        //go to spawnpoint
        Position = Globals.spawnPoint.Position;
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
        if(Input.IsActionPressed("move_crouch")) isCrouching = 2; else isCrouching = 1;
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
            if (vel < max_speed) vel += delta * 2000;
            else if (vel > max_speed) vel -= delta * 2000;
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
            Velocity = new Vector2(Velocity.X, Velocity.Y-jump_strength);
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
        //GD.Print("Player position: " + GlobalPosition);
        //GD.Print("Cursor position: "+ GetGlobalMousePosition());
        //GD.Print("DashVector: "+dashVector);
        //GD.Print("DashVector normalized: " + dashVector.Normalized()*400);
    }
    public void fall(double delta)
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + (float)(GRAVITY * delta));
    }
    public void CrouchApply()
    {
        if (isCrouching == 2)
        {
            Velocity = new Vector2 ((float)(Velocity.X/1.5), Velocity.Y);
            if (Velocity.Y > 0)
            {
                Velocity = new Vector2(Velocity.X, (float)(Velocity.Y * 1.2));
            }
            if (HitBox.Shape is RectangleShape2D rectangleShape)
            {
                rectangleShape.Size = new Vector2(defHB_X, defHB_Y/2);
            }
        }
        else
        {
            if (HitBox.Shape is RectangleShape2D rectangleShape)
            {
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
            projectile.damageDealt = 1;
        }

    }
    
    public override void _PhysicsProcess(double delta)
    {
        if (Globals.playerControl)
        {
            Movement(delta);

            if ((Input.IsActionPressed("attack_normal") || Input.IsActionPressed("attack_normal-alt")) && BADelta == 0 && !CACharging)
            {
                BasicAttack();
                BADelta = BACooldown;
            }




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
    }

}
