using Godot;
using System;
using System.Text;

public partial class Player : CharacterBody2D
{
    //values
    private int max_speed = 250;
    private int isCrouching = 1;
    private int acceleration = 800;
    private int friction = 4000;
    private int jump_strength = 450;
    private float GRAVITY = 1500f;
    private int defHB_X = 31;
    private int defHB_Y = 30;

    //nodes
    private CollisionShape2D HitBox;

    
    public override void _Ready()
    {
        // Get the CollisionShape2D node
        HitBox = GetNode<CollisionShape2D>("HitBox");
    }


    public Vector2 inputExp()
    {
        Vector2 direction = new();

        direction.X = Convert.ToInt32(Input.IsActionPressed("game_right")) - Convert.ToInt32(Input.IsActionPressed("game_left"));
        if (Input.IsActionPressed("game_jump"))
        {
            if (IsOnFloor())
            {
                direction.Y = -1;
            }
        }
        if(Input.IsActionPressed("game_crouch")) isCrouching = 2; else isCrouching = 1;
        return direction;
    }


    public void playerMovement(double delta)
    {
        //get input
        Vector2 input = inputExp();

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
            Velocity = new Vector2(Velocity.X + input.X * (float)(acceleration * delta), Velocity.Y);

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
            Velocity = new Vector2 ((float)(Velocity.X/1.3), Velocity.Y);
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
            playerMovement(delta);
        }



    }

}
