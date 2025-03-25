using Godot;
using System;

public partial class HeavyRanged : Enemy
{
    private PackedScene projectile = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/projectiles/caster_projectile.tscn");
    private RayCast2D targetingLine;
    private Node2D launchLocation;
    private bool projShot;

    public override void _Ready()
    {
        base._Ready();
        targetingLine = GetNode("Targeting") as RayCast2D;
        launchLocation = GetNode("LaunchLocation") as Node2D;
        maxHP = 40 * Globals.diffMultipliers[Globals.Difficulty];
        currentHP = maxHP;
        damage = 25 * Globals.diffMultipliers[Globals.Difficulty];
        atkCooldown.WaitTime = 2f;
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
    protected override void Attack()
    {
        if (targetingLine.GetCollider() is Player)
            base.Attack();
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
        if (!isAttacking)
            projShot = false;
        if ((sprite.Animation == "attack1" || sprite.Animation == "attack2") && sprite.Frame == 2 && !projShot)
        {
            //shoot
            CasterProjectile castproj = (CasterProjectile)projectile.Instantiate();
            AddSibling(castproj);
            castproj.GlobalPosition = GlobalPosition + new Vector2(0, -2);
            Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
            castproj.direction = direction;
            castproj.Rotation = direction.Angle();
            castproj.damagePayload = damage;
            projShot = true;
        }
    }
}
