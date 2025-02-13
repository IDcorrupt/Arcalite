using Godot;
using System;

public partial class CasterProjectile : CharacterBody2D
{
    [Export] public bool isSlowed;
    [Export] public float slowFactor;

    public Vector2 direction;
    public float damagePayload;

    private bool targetHit;

    private AnimatedSprite2D sprite;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;

        sprite.Play("fly");
    }

    private void OnAnimationFinished()
    {
        QueueFree();    
    }
    private void HitPlayer(Player player)
    {
        player.Hit(damagePayload, Vector2.Zero);
        sprite.Play("hitPlayer");
    }

    private void HitTerrain(Vector2 collisionNormal)
    {
        //calculate collision vector normal for animation rotation
        float hitAngle = collisionNormal.Angle() + Mathf.Pi / 2;
        Rotation = hitAngle;

        //play explosion
        sprite.Position = new Vector2(0, 0);
        sprite.Play("hitTerrain");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!targetHit)
        {
            Vector2 vel = direction * 700 * (float)delta;
            if (isSlowed)
            {
                vel *= slowFactor;
            }else
                slowFactor = 0;
            GD.Print("proj vel: " + vel);
            GD.Print("slowfactor: " + slowFactor);
            var collision = MoveAndCollide(vel);

            if (collision != null && (collision.GetCollider() is StaticBody2D || collision.GetCollider() is TileMapLayer))
            {
                targetHit = true;
                Vector2 collisionNormal = collision.GetNormal();

                HitTerrain(collisionNormal);
            }
            else if (collision != null && collision.GetCollider() is Player)
            {
                targetHit = true;
                HitPlayer(collision.GetCollider() as Player);
            }
        }
    }
}
