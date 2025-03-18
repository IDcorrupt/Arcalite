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
        atkCooldown.WaitTime = 1;
        jumpStrength = 400;
        shardDropRate = 30;
    }

    protected override void Attack()
    {
        base.Attack();
        int dir = 0;
        if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
            dir = 1;
        else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
            dir = -1;
        hitVector = new Vector2(300 * dir, -200);
        player.Hit(damage, hitVector);
    }
}
