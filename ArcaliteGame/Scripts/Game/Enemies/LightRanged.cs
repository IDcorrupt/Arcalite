using Godot;
using System;

public partial class LightRanged : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        maxHP = 20;
        currentHP = maxHP;
        damage = 10;
        atkCooldown.WaitTime = 3;
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

    }
}
