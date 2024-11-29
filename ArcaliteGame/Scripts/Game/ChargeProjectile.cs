using Godot;
using System;

public partial class ChargeProjectile : CharacterBody2D
{
    //values
    [Export] public int chargeLevel;
    [Export] public Vector2 direction;
    [Export] public float rotation;
    [Export] public float damagePayload;
    [Export] public bool imports = false; //needed because the export values don't arrive in time to be useful in the ready func

    public bool targetHit = false;

    //nodes
    private AnimatedSprite2D animatedSprite;
    public override void _Ready()
    {
        //anim setup
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite.Play("fly");
    }

    public void HitTerrain(Vector2 collisionNormal)
    {
        //calculate collision vector normal for animation rotation
        float hitAngle = collisionNormal.Angle() + Mathf.Pi;
        Rotation = hitAngle;

        //play explosion
        animatedSprite.Position = new Vector2(-16, 0);
        animatedSprite.Play("terrain_hit");
    }
    public void HitEnemy()
    {
        animatedSprite.Position = new Vector2(0, 0);
        animatedSprite.Play("enemy_hit");
    }

    public void AnimationFinished()
    {
		QueueFree();
    }

    public override void _Process(double delta)
    {
        if (imports)
        {
            imports = false;
            //determine charge characteristics
            switch (chargeLevel)
            {
                case 1:
                    Scale = new Vector2(1, 1);
                    break;
                case 2:
                    Scale = new Vector2((float)1.3, (float)1.3);
                    break;
                case 3:
                    Scale = new Vector2((float)1.6, (float)1.6);
                    break;
                case 4: 
                    Scale = new Vector2(2, 2);
                    break;
                default:
                    break;
            }
            
        }
        if (!targetHit)
        {
            Vector2 vel = direction * 300 * (float)delta;

            var collision = MoveAndCollide(vel);

            if (collision != null && collision.GetCollider() is StaticBody2D)
            {
                targetHit = true;
                Vector2 collisionNormal = collision.GetNormal();

                HitTerrain(collisionNormal);
            }
            else if (collision != null && collision.GetCollider() is CharacterBody2D)
            {
                Node collider = collision.GetCollider() as Node;
                if (collider.HasMeta("Type"))
                {
                    if ((string)collider.GetMeta("Type") == "Enemy")
                    {
                        targetHit = true;
                        HitEnemy();
                    }
                }

            }

        }
    }
}
