using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private int _speed = 300;

    
    public void GetInput()
    {
        float inputDir = Input.GetAxis("ui_left","ui_right");
        Velocity = Transform.X * inputDir * _speed;
        if(Input.IsActionPressed("Primary_attack")){
            
        }
    }



    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }

}
