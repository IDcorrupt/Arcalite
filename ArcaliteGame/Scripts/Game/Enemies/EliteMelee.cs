using Godot;
using System;

public partial class EliteMelee : Enemy
{
    Timer specAtkCooldown;
    public override void _Ready()
    {
        specAtkCooldown = GetNode("SpecialAttackCooldown") as Timer;
        base._Ready();
        maxHP = 125;
        currentHP = maxHP;
        damage = 20;
        atkCooldown.WaitTime = 1f;
        specAtkCooldown.WaitTime = 10f;
        jumpStrength = 600f;

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

    private void SpecialAttack()
    {

    }
    public void SpecialAttackRangeBodyEntered(Node2D body)
    {

    }
    public void SpecialAttackRangeBodyExited(Node2D body)
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
