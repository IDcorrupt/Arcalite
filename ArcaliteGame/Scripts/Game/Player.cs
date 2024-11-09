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
    private float dashCooldown = 1f;
    private float dashDelta = 0f;
    private bool dashed = false;
    private float dashSpeed = 1000f;     // Initial dash speed
    private float dashDecayRate = 3000f; // Rate at which dash speed decays
    private float currentDashSpeed = 0f; // Variable to track the current dash speed
    private bool isDashing = false;      // Track if the player is dashing

    //nodes
    private CollisionShape2D HitBox;

    
    public override void _Ready()
    {
        // Get the CollisionShape2D node
        HitBox = GetNode<CollisionShape2D>("HitBox");
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

        if (dashed)
        {
            dashed = false;
            dashDelta = dashCooldown;
            Dash(delta);

        }
        if (dashDelta > 0)
        {
            dashDelta -= (float)delta;
        }else dashDelta = 0;
        CrouchApply();
        fall(delta);
        MoveAndSlide();
    }

    public void Dash(double delta)
    {
        //self note: DOES NOT WORK - CHECK BROWSER WHEN U GET BACK TO THIS
        
        Vector2 dashVector = GetGlobalMousePosition() - GlobalPosition;
        GD.Print("Player position: " + GlobalPosition);
        GD.Print("Cursor position: "+ GetGlobalMousePosition());
        GD.Print("DashVector: "+dashVector);
        GD.Print("DashVector normalized: " + dashVector.Normalized()*400);
        Velocity += dashVector.Normalized()*1000;
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
