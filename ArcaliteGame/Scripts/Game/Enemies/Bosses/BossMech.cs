using Godot;
using System;

public partial class BossMech : CharacterBody2D
{
    //stats
    private float health;
    private float damage;

    //components
    private AnimatedSprite2D sprite;
    private BossMechArm BossMechArmLeft;
    private BossMechArm BossMechArmRight;
    private CollisionShape2D hitBox;

    public override void _Ready()
    {
        base._Ready();
        health = 1000f * Globals.diffMultipliers[Globals.Difficulty];
        damage = 40 * Globals.diffMultipliers[Globals.Difficulty];

    }

    private void PrepareSingleAttack()
    {

    }
    private void SingleAttack()
    {

    }
    private void PrepareDoubleAttack()
    {

    }
    private void DoubleAttack()
    {

    }
    private void PrepareLaser()
    {

    }
    private void Laser()
    {

    }







    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

    }

}
