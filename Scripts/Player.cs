using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private int _speed = 300;
    private int jump_strength = 400;
    private float GRAVITY = 2000f;
    
    

    public void GetInput()
    {
        float direction = 0f;
        if (Input.IsActionPressed("ui_right"))
        {
            direction = 1f;
        }else if (Input.IsActionPressed("ui_left"))
        {
            direction = -1f;
        }
        if (Input.IsActionPressed("ui_select"))
        {
            if (IsOnFloor())
            {
                Velocity = new Vector2 (Velocity.X, Velocity.Y + jump_strength);
            }
        }
        Velocity = new Vector2(direction *_speed, Velocity.Y);
    }

    public void fall(double delta)
    {
        //Velocity = new Vector2(Velocity.X, Velocity.Y + (float)(GRAVITY*delta));
        Velocity = new Vector2 (Velocity.X, GRAVITY);
    }





    public bool getIsonFloor() { return IsOnFloor(); }
    public override void _PhysicsProcess(double delta)
    {
        //get inputs & set horizontal velocity
        GetInput();
        //set vertical velocity
        fall(delta);
        

        //move char
        MoveAndSlide();
    }

}
