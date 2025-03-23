using Godot;
using System;

public partial class HeavyMelee : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        maxHP = 75;
        currentHP = maxHP;
        damage = 15;
        atkCooldown.WaitTime = 1;
        jumpStrength = 450;
        shardDropRate = 50;
    }

    protected override void Attack()
    {
        base.Attack();
        int dir = 0;
        if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
            dir = 1;
        else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
            dir = -1;
        hitVector = new Vector2(200 * dir, -600);
        player.Hit(damage, hitVector);
    }
}
