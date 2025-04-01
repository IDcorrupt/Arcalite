using Godot;
using System;

public partial class BossMech : CharacterBody2D
{
//stats
    private float health;
    private float damage;

//variables
    //used to signal when boss finished its intro anim
    private bool ACTIVE = false;    
    //true if right side, false if left
    private bool playerSide;

    public bool playerInRoom = false;
    private bool startup = false;

//components
    private AnimatedSprite2D sprite;
    private BossMechArm BossMechArmLeft;
    private BossMechArm BossMechArmRight;
    private CollisionShape2D hitBox;
    private RayCast2D fightTrigger;
    private AnimationPlayer launchAnim;

//timers
    private Timer singleAttackCooldown;
    private Timer doubleAttackCooldown;
    private Timer laserAttackCooldown;
    private Timer vulnerabilityCooldown;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode("Sprite") as AnimatedSprite2D;
        BossMechArmLeft = GetNode("Arms/BossMechArmLeft") as BossMechArm;
        BossMechArmRight = GetNode("Arms/BossMechArmRight") as BossMechArm;
        hitBox = GetNode("CollisionShape2D") as CollisionShape2D;
        fightTrigger = GetNode("FightTrigger") as RayCast2D;
        launchAnim = GetNode("LaunchAnim") as AnimationPlayer;

        singleAttackCooldown = GetNode("Timers/SingleAttack") as Timer;
        doubleAttackCooldown = GetNode("Timers/DoubleAttack") as Timer;
        laserAttackCooldown = GetNode("Timers/Laser") as Timer;
        vulnerabilityCooldown = GetNode("Timers/Vulnerability") as Timer;


        health = 1000f * Globals.diffMultipliers[Globals.Difficulty];
        damage = 40 * Globals.diffMultipliers[Globals.Difficulty];



    }

    private void StartupSequence()
    {
        if (!startup)
        {
            GD.Print("staring launch sequence");
            startup = true;
            BossMechArmLeft.LaunchSequence();
            BossMechArmRight.LaunchSequence();
            launchAnim.Play("BossMechStartup");
        }
    }

    private void PrepareSingleAttack()
    {

    }
    private void SingleAttack()
    {

    }
    private void PrepareDoubleAttack()
    {

    }
    private void DoubleAttack()
    {

    }
    private void PrepareLaser()
    {

    }
    private void Laser()
    {

    }


    private void Hit(float damage)
    {

    }

    private void Animate()
    {

    }

    private void Update(double delta)
    {
        if (ACTIVE)
        {
            //determine where player is in relation to itself
            playerSide = Globals.player.GlobalPosition.X > GlobalPosition.X ? true : false;

        }
        if(playerInRoom)
        {
            StartupSequence();
        }





        Animate();
    }



    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Update(delta);

    }

}
