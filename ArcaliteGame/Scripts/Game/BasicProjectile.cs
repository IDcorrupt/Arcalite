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
	public void HitEnemy()
	{

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

			if(collision != null && collision.GetCollider() is StaticBody2D)
			{
				targetHit = true;
				Vector2 collisionNormal = collision.GetNormal();

				HitTerrain(collisionNormal);
			}else if (collision != null && collision.GetCollider() is CharacterBody2D)
			{
				//enemies
			}
            
        }
    }
}
