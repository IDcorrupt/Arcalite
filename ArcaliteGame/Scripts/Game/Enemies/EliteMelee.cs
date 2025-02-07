using Godot;
using System;

public partial class EliteMelee : Enemy
{
    public override void _Ready()
    {
        base._Ready();

    }

    protected override void Attack()
    {
        base.Attack();  
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
