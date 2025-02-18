using Godot;
using System;
using System.Net;

public partial class BasicProjectile : CharacterBody2D
{
	//values
	[Export] public Vector2 direction;
	[Export] public float damagePayload;

	public bool targetHit = false;


	//nodes
	private AnimatedSprite2D animatedSprite;
	public override void _Ready()
	{
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
        animatedSprite.Play("fly");

    }


    
	public void HitTerrain(Vector2 collisionNormal)
	{
        //calculate collision vector normal for animation rotation
        float hitAngle = collisionNormal.Angle() + Mathf.Pi/2;
        Rotation = hitAngle;

		//play explosion
		animatedSprite.Position = new Vector2(0, 0);
        animatedSprite.Play("terrain_hit");
	}
	public void HitEnemy(Enemy enemy)
	{
		//damage stuff
        int dir = 0;
        if ((enemy.GlobalPosition - GlobalPosition).Normalized().X > 0)
            dir = 1;
        else if ((enemy.GlobalPosition - GlobalPosition).Normalized().X < 0)
            dir = -1;
		Vector2 hitVector = new Vector2(dir*200, 0);
		enemy.Hit(damagePayload, hitVector);
		
        //animation
        animatedSprite.Position = new Vector2(0, 0);
        animatedSprite.Play("enemy_hit");
    }

	public void AnimationFinished()
	{
		QueueFree();
	}
	public override void _Process(double delta)
	{
		if (!targetHit)
		{
			Vector2 vel = direction * 800 * (float)delta;

            var collision = MoveAndCollide(vel);

			if(collision != null &&  (collision.GetCollider() is StaticBody2D || collision.GetCollider() is TileMapLayer))
			{
				targetHit = true;
				Vector2 collisionNormal = collision.GetNormal();

				HitTerrain(collisionNormal);
			}else if (collision != null && collision.GetCollider() is Enemy)
			{
				targetHit = true;
				HitEnemy(collision.GetCollider() as Enemy);
			}
            
        }
    }
}
