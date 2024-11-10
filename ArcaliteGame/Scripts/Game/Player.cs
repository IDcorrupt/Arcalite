using Godot;
using System;
using System.Security.Principal;
using System.Text;

public partial class Player : CharacterBody2D
{
    //values
    private int defHB_X = 16;
    private int defHB_Y = 38;
    private int isCrouching = 1;

    private int max_speed = 300;
    private double vel = 0;
    private int jump_strength = 450;
    private float GRAVITY = 1500f;
    private float prevDir = 0;

    private int friction = 500;
    
    //dash
    private float dashCooldown = 1f;        //dash cooldown constant
    private float dashDelta = 0f;           //dash cooldown remaining
    private bool dashed = false;            //dash initiated
    private float dashSpeed = 2000f;        //initial dash speed
    private float dashDecayRate = 15000f;    //dash speed decay rate
    private float currentDashSpeed = 0f;    //current dash speed
    private bool isDashing = false;         //is dash currently active
    private Vector2 dashVector;             //fixed vector for dash endpoint -> dash follows mouse otherwise :3

    //nodes
    private CollisionShape2D HitBox;

    
    public override void _Ready()
    {
        //Get the CollisionShape2D node
        HitBox = GetNode<CollisionShape2D>("HitBox");
        //go to spawnpoint
        Position = Globals.spawnPoint.Position;
    }


    public Vector2 getInputs()
    {
        Vector2 direction = new();

        direction.X = Convert.ToInt32(Input.IsActionPressed("move_right")) - Convert.ToInt32(Input.IsActionPressed("move_left"));
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

    public void movementExp(double delta)
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
    public void playerMovement(double delta)
    {
        //get input
        Vector2 input = getInputs();

        if (input.X == 0)
        {
            //no input
            if (Velocity.X > (friction * delta))
            {
                //add friction to slow player
                //Velocity -= Velocity.Normalized() * (float)(friction*delta);
                Velocity = new Vector2 (Velocity.X - (float)(friction * delta), Velocity.Y);
            }
            else if (Velocity.X < (friction * delta * -1))
            {
                Velocity = new Vector2(Velocity.X + (float)(friction * delta), Velocity.Y);
            }
            //stop so it doesn't start moving the other way
            else Velocity = new Vector2(0, Velocity.Y);
        }
        else
        {
            //if input -> add corresponding velocity
            Velocity = new Vector2(Velocity.X + input.X * (float)(vel * delta), Velocity.Y);

        }
        //limit max speed
        if (Velocity.X > max_speed)
        {
            Velocity = new Vector2(max_speed, Velocity.Y);
        }
        else if (Velocity.X < -max_speed)
        {

            Velocity = new Vector2(-max_speed, Velocity.Y);
        }
        if (input.Y != 0)
        {
            Velocity = new Vector2 (Velocity.X,Velocity.Y -jump_strength);
        }
        //execute move
        CrouchApply();
        fall(delta);
        MoveAndSlide();

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


    public override void _PhysicsProcess(double delta)
    {
        if (Globals.playerControl)
        {
            //playerMovement(delta);
            movementExp(delta);
        }

        

    }

}
