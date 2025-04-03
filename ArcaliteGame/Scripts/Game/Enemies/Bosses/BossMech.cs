using Godot;

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
    private bool startup = false;
    private bool attacking = false;
    private bool hurt = false;
    private bool dead = false;
    private int laserCount; //how many times does laser need to activate to trigger vulnerability
    public bool playerInRoom = false;

    //components
    private AnimatedSprite2D sprite;
    private BossMechArm BossMechArmLeft;
    private BossMechArm BossMechArmRight;
    private CollisionShape2D hitBox;
    private AnimationPlayer launchAnim;

    //timers
    private Timer singleAttackCooldown;
    private Timer doubleAttackCooldown;
    private Timer laserAttackCooldown;
    private Timer animFlashTimer;
    private Timer vulnerabilityCooldown;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode("Sprite") as AnimatedSprite2D;
        BossMechArmLeft = GetNode("Arms/BossMechArmLeft") as BossMechArm;
        BossMechArmRight = GetNode("Arms/BossMechArmRight") as BossMechArm;
        hitBox = GetNode("CollisionShape2D") as CollisionShape2D;
        launchAnim = GetNode("LaunchAnim") as AnimationPlayer;

        singleAttackCooldown = GetNode("Timers/SingleAttack") as Timer;
        doubleAttackCooldown = GetNode("Timers/DoubleAttack") as Timer;
        laserAttackCooldown = GetNode("Timers/Laser") as Timer;
        animFlashTimer = GetNode("Timers/AnimFlashTimer") as Timer;
        vulnerabilityCooldown = GetNode("Timers/Vulnerability") as Timer;


        health = 750 * Globals.diffMultipliers[Globals.Difficulty];
        damage = 30 * Globals.diffMultipliers[Globals.Difficulty];
        //timings
        singleAttackCooldown.WaitTime = 5 / Globals.diffMultipliers[Globals.Difficulty];
        doubleAttackCooldown.WaitTime = 10 / Globals.diffMultipliers[Globals.Difficulty];
        laserAttackCooldown.WaitTime = 20 / Globals.diffMultipliers[Globals.Difficulty];
        vulnerabilityCooldown.WaitTime = 10 / Globals.diffMultipliers[Globals.Difficulty];
        laserCount = Globals.Difficulty + 1;


        BossMechArmLeft.EyeFlash += BossMechArm_EyeFlash;
        BossMechArmRight.EyeFlash += BossMechArm_EyeFlash;
        BossMechArmLeft.AttackFinished += BossMechArm_AttackFinished;
        BossMechArmRight.AttackFinished += BossMechArm_AttackFinished;
    }

    private void BossMechArm_AttackFinished(Enums.MechAttackType type)
    {
        attacking = false;
        GD.Print("tpye: " + type);
        switch (type)
        {
            case Enums.MechAttackType.Single:
                singleAttackCooldown.Start();
                break;
            case Enums.MechAttackType.Double:
                doubleAttackCooldown.Start();
                break;
            case Enums.MechAttackType.Sweep:
                //if i implement it
                break;
            case Enums.MechAttackType.Laser:
                laserAttackCooldown.Start();
                break;
            default:
                break;
        }
    }

    private void BossMechArm_EyeFlash(int type)
    {
        sprite.Play($"eye_flash_{type.ToString()}");
    }

    private void StartupSequence()
    {
        if (!startup)
        {
            startup = true;

            launchAnim.Play("BossMechStartup");
            BossMechArmLeft.LaunchSequence(launchAnim.CurrentAnimationLength);
            BossMechArmRight.LaunchSequence(launchAnim.CurrentAnimationLength);
        }
    }
    public void OnAnimationFinished(StringName anim)
    {
        if (anim.ToString() == "BossMechStartup")
        {
            singleAttackCooldown.Start();
            doubleAttackCooldown.Start();
            laserAttackCooldown.Start();
            ACTIVE = true;
        }
    }
    private void InitiateSingleAttack()
    {
        attacking = true;
        if (playerSide)
            //right
            BossMechArmRight.AttackSignal(Enums.MechAttackType.Single);
        else
            //left
            BossMechArmLeft.AttackSignal(Enums.MechAttackType.Single);
    }
    private void InitiateDoubleAttack()
    {
        attacking = true;
        BossMechArmLeft.AttackSignal(Enums.MechAttackType.Double);
        BossMechArmRight.AttackSignal(Enums.MechAttackType.Double);
    }
    private void InitiateLaser()
    {

    }


    private void Hit(float damage)
    {
        sprite.Play("hurt");
    }

    private void Movement(double delta)
    {
        Vector2 playerDistance = Globals.player.GlobalPosition - GlobalPosition;
        Vector2 targetPos = playerDistance / 10;
        float maxSpeed = 150f;
        float acceleration = 2000f;
        if (Position.X < targetPos.X)
        {
            //right (+)
            if (Position.X + 2 < targetPos.X)
                //accel if far
                Velocity = new Vector2(Mathf.Clamp(Velocity.X + acceleration * (float)delta, 0, maxSpeed), Velocity.Y);
            else
            {
                //setpos if close
                Position = new Vector2(targetPos.X, Position.Y);
                Velocity = Vector2.Zero;
            }
        }
        else if (Position.X > targetPos.X)
        {
            //left (-)
            if (Position.X - 2 > targetPos.X)
                //accel if far
                Velocity = new Vector2(Mathf.Clamp(Velocity.X - acceleration * (float)delta, -maxSpeed, 0), Velocity.Y);
            else
            {
                //setpos if close
                Position = new Vector2(targetPos.X, Position.Y);
                Velocity = Vector2.Zero;
            }
        }
        else
        {
            Velocity = Vector2.Zero;
        }
    }
    public void OnSpriteAnimationFinished()
    {
        if (sprite.Animation == "eye_flash_1")
            sprite.Play("idle_closed");
        else if (sprite.Animation == "eye_flash_2")
            sprite.Play("idle_closed");
        else if (sprite.Animation == "hurt")
            sprite.Play("idle_open");
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
            //leaning
            if (!attacking)
            {
                Movement(delta);
            }else Velocity = Vector2.Zero;  

            //attacks
            if (!attacking && singleAttackCooldown.TimeLeft == 0)
                InitiateSingleAttack();
            if (!attacking && doubleAttackCooldown.TimeLeft == 0)
                InitiateDoubleAttack();
            if (!attacking && laserAttackCooldown.TimeLeft == 0)
                InitiateLaser();


        }
        if (playerInRoom)
        {
            StartupSequence();
        }




        MoveAndSlide();
        Animate();
    }


    public float GetDamage() { return damage; }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Update(delta);

    }

}
