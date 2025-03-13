using Godot;
using System;

public partial class LightRanged : Enemy
{
    private PackedScene projectile = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/projectiles/caster_projectile.tscn");
    private RayCast2D targetingLine;
    public override void _Ready()
    {
        base._Ready();
        targetingLine = GetNode("Targeting") as RayCast2D;
        maxHP = 20;
        currentHP = maxHP;
        damage = 10;
        atkCooldown.WaitTime = 1.5f;
        jumpStrength = 400;
        shardDropRate = 100;
    }
    
    protected override void Attack()
    {
        base.Attack();
        if (targetingLine.GetCollider() is Player)
        {
            //shoot
            CasterProjectile castproj = (CasterProjectile)projectile.Instantiate();
            AddSibling(castproj);
            castproj.GlobalPosition = GlobalPosition;
            Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
            castproj.direction = direction;
            castproj.Rotation = direction.Angle();
            castproj.damagePayload = damage;
        }
    }

     public override void Update(double delta)
     {
        if(targetingLine.GetCollider() is Player)
        {
            playerInAtkRange = true;
        }else playerInAtkRange = false;
        base.Update(delta);
     }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        targetingLine.LookAt(player.GlobalPosition);
    }
}
