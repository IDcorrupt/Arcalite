using Godot;
using System;
using System.Text;

public partial class Player : CharacterBody2D
{
    private int max_speed = 500;
    private int acceleration = 1600;
    private int friction = 4000;
    private int jump_strength = 610;
    private float GRAVITY = 2000f;
    
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
        MoveAndSlide();

    }

    public void fall(double delta)
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + (float)(GRAVITY * delta));
    }

    
    public override void _PhysicsProcess(double delta)
    {
        //controls
        fall(delta);
        playerMovement(delta);

        RichTextLabel text = GetNode<RichTextLabel>("./flordebug/isonfloorlabel");
        text.Text = $"Velocity X: {Velocity.X} \n Velocity Y: {Velocity.Y}";
    }

}
