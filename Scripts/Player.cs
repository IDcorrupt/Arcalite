using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private int _speed = 300;
    
    public void GetInput(){
        Vector2 direction = Input.GetVector("ui_left","ui_right"."ui_up","ui_down");
        Velocity = direction * _speed;
    }
    public void override

}
