using Godot;
using System;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

public partial class LightMeele : Enemy
{

    public override void _Ready()
    {
        base._Ready();
        maxHP = 50;
        currentHP = maxHP;
        damage = 5;
        atkCooldown.WaitTime = 0.5f;
    }

    protected override void Attack()
    {
        GD.Print("spec atk called");
        base.Attack();
        player.Hit(damage, this);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

    }
}
