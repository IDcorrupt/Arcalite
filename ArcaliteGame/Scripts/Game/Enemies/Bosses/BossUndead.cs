using Godot;
using System;

public partial class BossUndead : Enemy
{
    //undead boss uses enemy base class because it's not a fullscreen boss
    private int specAtkMoveCD;
    private bool playerInSpecAttackRange;
    private bool specattackanimcast = false;
    private bool specattacked = false;
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
        maxHP = 350 * Globals.diffMultipliers[Globals.Difficulty];
        currentHP = maxHP;
        damage = 25 * Globals.diffMultipliers[Globals.Difficulty];
        atkCD = 1.5f;
        specAtkMoveCD = 2;
        specAtkCooldown.WaitTime = 10f;
        attackFrame = 4;
        jumpStrength = 600f;
        shardDropRate = 300 * Mathf.RoundToInt(Globals.diffMultipliers[Globals.Difficulty]);

    }

    private void SpecAttackAnimLeft_AnimationFinished() { specAttackAnimLeft.Frame = 0; specAttackAnimLeft.Hide(); }
    private void SpecAttackAnimRight_AnimationFinished() { specAttackAnimRight.Frame = 0; specAttackAnimRight.Hide(); }

    protected override void Attack()
    {
        if (!(sprite.Animation == "spec_attack"))
        {
            int dir = 0;
            if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
                dir = 1;
            else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
                dir = -1;
            hitVector = new Vector2(500 * dir, -400);
            player.Hit(damage, hitVector);
        }
    }

    private void EngageSpecialAttack()
    {
        //defaults
        sprite.Play("spec_attack");
        isAttacking = true;
        specattacked = false;
        Velocity = Vector2.Zero;
        speed = 0;    
        //attack transmit happens in process due to special method
    }
    private void SpecialAttackAnim()
    {
        specAttackAnimLeft.Show();
        specAttackAnimRight.Show();
        specAttackAnimRight.Play("attack");
        specAttackAnimLeft.Play("attack");
    }
    private void SpecialAttack()
    {
        GD.Print("specattack damage called");
        int dir = 0;
        if ((player.GlobalPosition - GlobalPosition).Normalized().X > 0)
            dir = 1;
        else if ((player.GlobalPosition - GlobalPosition).Normalized().X < 0)
            dir = -1;
        hitVector = new Vector2(700 * dir, -500);
        player.Hit(damage * 1.5f, hitVector);
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
            specattacked = false;
            specattackanimcast = false;
            sprite.Play("idle");
            specAtkCooldown.Start();
            atkCooldown.Start(specAtkMoveCD);
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (!isDead && !isHurt && !isAttacking && playerInSpecAttackRange && isChasing && specAtkCooldown.TimeLeft <=0)  //all of the conditions required for spec attack
        {
            EngageSpecialAttack();
        }
        if (isAttacking && !specattackanimcast && sprite.Animation == "spec_attack" && sprite.Frame >= 17) 
        {
            SpecialAttackAnim();
            specattackanimcast = true;
        }

        if (!specattacked && specattackanimcast && specAttackAnimLeft.Frame > 5 && specAttackAnimRight.Frame > 5 && playerInSpecAttackRange)
        {
            SpecialAttack();
            specattacked = true;
        }

    }


}
