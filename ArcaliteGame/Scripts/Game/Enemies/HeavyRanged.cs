using Godot;
using System;

public partial class HeavyRanged : Enemy
{
    private PackedScene projectile = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/projectiles/caster_projectile.tscn");
    private RayCast2D targetingLine;
    public override void _Ready()
    {
        base._Ready();
        targetingLine = GetNode("Targeting") as RayCast2D;
        maxHP = 40;
        currentHP = maxHP;
        damage = 25;
        atkCooldown.WaitTime = 2f;
        jumpStrength = 400;
        shardDropRate = 70;
    }

    protected override void Attack()
    {
        base.Attack();
        if (targetingLine.GetCollider() is Player)
        {
            //shoot
            CasterProjectile castproj = (CasterProjectile)projectile.Instantiate();
            AddSibling(castproj);
            castproj.GlobalPosition = GlobalPosition + new Vector2(0, -2);
            Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
            castproj.direction = direction;
            castproj.Rotation = direction.Angle();
            castproj.damagePayload = damage;
        }
    }

    protected override void Animate()
    {
        base.Animate();
        if(sprite.Animation == "die")
        {
            sprite.Scale = new Vector2(0.5f, 0.5f);
            sprite.Position = new Vector2(0, 0);
        }else if(sprite.Animation == "attack")
        {
            sprite.Scale = new Vector2(0.5f, 0.5f);
            sprite.Position = new Vector2(0, 0);
        }
        else
        {
            sprite.Scale = new Vector2(1, 1);
            sprite.Position = new Vector2(2, -8.5f);
        }
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
