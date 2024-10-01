using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private int _speed = 300;
    
    public void GetInput()
    {
        Vector2 inputdir = Input.GetVector("ui_left","ui_right"."ui_up","ui_down");
        Velocity = inputdir * _speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndCollide(Velocity * (float)delta);
    }

}
