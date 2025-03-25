using Godot;
using System;

public partial class LightRanged : Enemy
{
    private PackedScene projectile = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/projectiles/caster_projectile.tscn");
    private RayCast2D targetingLine;
    private Node2D launchLocation;
    private bool projShot = false;

    public override void _Ready()
    {
        base._Ready();
        targetingLine = GetNode("Targeting") as RayCast2D;
        launchLocation = GetNode("LaunchLocation") as Node2D;
        maxHP = 20 * Globals.diffMultipliers[Globals.Difficulty];
        currentHP = maxHP;
        damage = 10 * Globals.diffMultipliers[Globals.Difficulty];
        atkCooldown.WaitTime = 1.5f;
        jumpStrength = 400;
        shardDropRate = 30 * Mathf.RoundToInt(Globals.diffMultipliers[Globals.Difficulty]);
    }

    protected override void Flip(bool dir)
    {
        base.Flip(dir);
        if (dir)
        {
            //left
            launchLocation.Position = new Vector2(-9, 0);
        }
        else
        {
            //right
            launchLocation.Position = new Vector2(9, 0);
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
        if ((sprite.Animation == "attack1" || sprite.Animation == "attack2") && sprite.Frame >= 4 && !projShot)
        {
            //shoot
            CasterProjectile castproj = (CasterProjectile)projectile.Instantiate();
            AddSibling(castproj);
            castproj.GlobalPosition = launchLocation.GlobalPosition;
            Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
            castproj.direction = direction;
            castproj.Rotation = direction.Angle();
            castproj.damagePayload = damage;
            projShot = true;
        } 
    }
}
