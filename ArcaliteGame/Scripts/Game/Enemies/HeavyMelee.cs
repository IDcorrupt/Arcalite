using Godot;
using System;

public partial class HeavyMelee : Enemy
{
    public override void _Ready()
    {
        base._Ready();
        maxHP = 100 * Globals.diffMultipliers[Globals.Difficulty];
        currentHP = maxHP;
        damage = 20 * Globals.diffMultipliers[Globals.Difficulty];
        atkCD = 1;
        attackFrame = 2;
        jumpStrength = 450;
        shardDropRate = 50 * Mathf.RoundToInt(Globals.diffMultipliers[Globals.Difficulty]);
    }

    protected override void Attack()
    {
        int dir = 0;
        if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
            dir = 1;
        else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
            dir = -1;
        hitVector = new Vector2(200 * dir, -600);
        player.Hit(damage, hitVector);
    }
}
