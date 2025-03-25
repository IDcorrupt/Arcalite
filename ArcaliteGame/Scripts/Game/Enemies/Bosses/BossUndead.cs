using Godot;
using System;

public partial class BossUndead : Enemy
{
    //undead boss uses enemy base class because it's not a fullscreen boss

    private bool playerInSpecAttackRange;
    Timer specAtkCooldown;
    AnimatedSprite2D specAttackAnimLeft;
    AnimatedSprite2D specAttackAnimRight;
    public override void _Ready()
    {
        base._Ready();
        specAtkCooldown = GetNode("SpecialAttackCooldown") as Timer;
        specAttackAnimLeft = GetNode("SpecAttackAnim/Left") as AnimatedSprite2D;
        specAttackAnimRight = GetNode("SpecAttackAnim/Right") as AnimatedSprite2D;
        specAttackAnimLeft.AnimationFinished += SpecAttackAnimLeft_AnimationFinished;
        specAttackAnimRight.AnimationFinished += SpecAttackAnimRight_AnimationFinished;
        specAttackAnimLeft.Hide();
        specAttackAnimRight.Hide();
        maxHP = 120 * Globals.diffMultipliers[Globals.Difficulty];
        currentHP = maxHP;
        damage = 25 * Globals.diffMultipliers[Globals.Difficulty];
        atkCooldown.WaitTime = 2f;
        specAtkCooldown.WaitTime = 10f;
        jumpStrength = 600f;
        shardDropRate = 300 * Mathf.RoundToInt(Globals.diffMultipliers[Globals.Difficulty]);

    }

    private void SpecAttackAnimLeft_AnimationFinished() { specAttackAnimLeft.Hide(); }
    private void SpecAttackAnimRight_AnimationFinished() { specAttackAnimRight.Hide(); }

    protected override void Attack()
    {
        base.Attack();
        int dir = 0;
        if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
            dir = 1;
        else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
            dir = -1;
        hitVector = new Vector2(500 * dir, -400);
        player.Hit(damage, hitVector);
    }




    private void SpecialAttack()
    {
        //defaults
        isAttacking = true;
        sprite.Play("spec_attack");
        Velocity = Vector2.Zero;
        speed = 0;    
        //attack transmit happens in process due to special method
    }
    public void SpecialAttackRangeBodyEntered(Node2D body)
    {
        if(body is Player)
            playerInSpecAttackRange = true;
    }
    public void SpecialAttackRangeBodyExited(Node2D body)
    {
        if(body is Player)
            playerInSpecAttackRange = false;
    }

    protected override void Die()
    {
        //special drop
        DropItems(Enums.itemType.necklace, 100);
        base.Die();
    }

    public override void OnSpriteAnimationFinished()
    {
        base.OnSpriteAnimationFinished();   
        if (sprite.Animation == "spec_attack")
        {
            isAttacking = false;
            sprite.Play("idle");
            specAtkCooldown.Start();
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (!isDead && !isHurt && !isAttacking && playerInSpecAttackRange && isChasing && specAtkCooldown.TimeLeft <=0)  //all of the conditions required for spec attack
        {
            SpecialAttack();
        }
        if(isAttacking && sprite.Animation == "spec_attack" && sprite.Frame >= 17) 
        {
            specAttackAnimLeft.Show();
            specAttackAnimRight.Show();
            specAttackAnimRight.Play("attack");
            specAttackAnimLeft.Play("attack");
        }

        if (isAttacking && sprite.Animation == "spec_attack" && specAttackAnimLeft.Frame > 5 && specAttackAnimRight.Frame > 5 && playerInSpecAttackRange)
        {
            int dir = 0;
            if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
                dir = 1;
            else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
                dir = -1;
            hitVector = new Vector2(700 * dir, -500);
            player.Hit(damage * 1.5f, hitVector);
        }

    }


}
