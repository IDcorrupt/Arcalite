using Godot;
using System;

public partial class BasicProjectile : CharacterBody2D
{
	//values
	[Export] public Vector2 direction;
	[Export] public float damageDealt;

	private Object collider;

	//nodes
	private RayCast2D rayCast;
	private AnimatedSprite2D animatedSprite;
	public override void _Ready()
	{
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		rayCast = GetNode<RayCast2D>("RayCast2D");
        animatedSprite.Play("fly");
    }


    public bool CheckCollision()
	{
		if (rayCast.IsColliding())
		{
			collider = rayCast.GetCollider();
			if(collider is StaticBody2D) return true;
			return false;
		}
		return false;
	}
	public void Hit()
	{
		if (collider is StaticBody2D)
		{
			animatedSprite.Play("terrain_hit");
		}
	}

	public void AnimationFinished()
	{
		//QueueFree();
	}
	public override void _Process(double delta)
	{
		//[SELF NOTE] - GAME CRASHES WITH ALL FUNCTIONS, PLS CONTINUE DEBUG :33
		Vector2 vel = new Vector2(direction.X*(float)delta*1000, direction.Y*(float)delta*1000);
		MoveAndCollide(vel);
			
		

	}
}
