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
        maxHP = 60 * Globals.diffMultipliers[Globals.Difficulty];
        currentHP = maxHP;
        damage = 15 * Globals.diffMultipliers[Globals.Difficulty];
        atkCD = 1;
        attackFrame = 2;
        jumpStrength = 400;
        shardDropRate = Mathf.RoundToInt(30 * Globals.diffMultipliers[Globals.Difficulty]);
    }

    protected override void Attack()
    {
        int dir = 0;
        if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
            dir = 1;
        else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
            dir = -1;
        hitVector = new Vector2(300 * dir, -200);
        player.Hit(damage, hitVector);
    }
}
