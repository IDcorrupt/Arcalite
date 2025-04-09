using Godot;
using System;

public partial class HeavyRanged : Enemy
{
    private RayCast2D targetingLine;
    private Node2D launchLocation;
    public override void _Ready()
    {
        base._Ready();
        targetingLine = GetNode("Targeting") as RayCast2D;
        launchLocation = GetNode("LaunchLocation") as Node2D;
        maxHP = 50 * Globals.diffMultipliers[Globals.Difficulty];
        currentHP = maxHP;
        damage = 25 * Globals.diffMultipliers[Globals.Difficulty];
        atkCD = 2f;
        attackFrame = 2;
        jumpStrength = 400;
        shardDropRate = 60 * Mathf.RoundToInt(Globals.diffMultipliers[Globals.Difficulty]);
    }

    protected override void Flip(bool dir)
    {
        base.Flip(dir);
        if (dir)
        {
            //left
            launchLocation.Position = new Vector2(-9, -3);
        }
        else
        {
            //right
            launchLocation.Position = new Vector2(9, -3);
        }
    }
    protected override void EngageAttack()
    {
        if (targetingLine.GetCollider() is Player)
            base.EngageAttack();
    }
    protected override void Attack()
    {
        //shoot
        CasterProjectile castproj = PreloadRegistry.Game.Projectiles.casterProjectile.Instantiate() as CasterProjectile;
        AddSibling(castproj);
        castproj.GlobalPosition = GlobalPosition + new Vector2(0, -2);
        Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
        castproj.direction = direction;
        castproj.Rotation = direction.Angle();
        castproj.damagePayload = damage;
    }
    public override void Update(double delta)
    {
        if (targetingLine.GetCollider() is Player)
        {
            playerInAtkRange = true;
        }
        else playerInAtkRange = false;
        base.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        targetingLine.LookAt(player.GlobalPosition);
    }
}
