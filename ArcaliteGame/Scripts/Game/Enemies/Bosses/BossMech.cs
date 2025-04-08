using Godot;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public partial class BossMech : CharacterBody2D
{
    //stats
    private float health;
    private float damage;
    private int shardDropRate;

    //variables
    //used to signal when boss finished its intro anim
    private bool ACTIVE = false;
    //true if right side, false if left
    private bool playerSide;
    private bool startup = false;
    private bool attacking = false;
    private bool dead = false;
    private bool vulnerable = false;
    private int laserCount;         //how many times does laser need to activate to trigger vulnerability
    private int laserSlashes = 3;   //how many "rounds" the laser will do
    private bool laserActive = false;
    public bool playerInRoom = false;

    //components
    private PackedScene itemScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/item.tscn");
    private PackedScene laserBeamScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/Enemies/projectiles/laser.tscn");
    private Laser laserBeam;

    private EnemyControl parent;
    private Node2D itemContainer;
    private AnimatedSprite2D sprite;
    private BossMechArm BossMechArmLeft;
    private BossMechArm BossMechArmRight;
    private CollisionPolygon2D hitBox;
    private AnimationPlayer animPlayer;
    private RayCast2D laserLine;
    private ShapeCast2D laserHitBox;

    //timers
    private Timer singleAttackCooldown;
    private Timer doubleAttackCooldown;
    private Timer sweepAttackCooldown;
    private Timer laserAttackCooldown;
    private Timer animFlashTimer;
    private Timer vulnerabilityTime;

    //tweens & scenetimers
    private Tween laserMove;
    private SceneTreeTimer laserGrace;

    //signals
    [Signal] public delegate void SetLaserTimeEventHandler(float time);

    public override void _Ready()
    {
        base._Ready();

        parent = GetParent() as EnemyControl;
        itemContainer = parent.GetParent().GetParent().GetNode("Items") as Node2D;
        sprite = GetNode("Sprite") as AnimatedSprite2D;
        BossMechArmLeft = GetNode("Arms/BossMechArmLeft") as BossMechArm;
        BossMechArmRight = GetNode("Arms/BossMechArmRight") as BossMechArm;
        hitBox = GetNode("CoreHitBox") as CollisionPolygon2D;
        animPlayer = GetNode("AnimPlayer") as AnimationPlayer;
        laserLine = GetNode("LaserLine") as RayCast2D;
        laserHitBox = GetNode("LaserHitBox") as ShapeCast2D;

        singleAttackCooldown = GetNode("Timers/SingleAttack") as Timer;
        doubleAttackCooldown = GetNode("Timers/DoubleAttack") as Timer;
        sweepAttackCooldown = GetNode("Timers/SweepAttack") as Timer;
        laserAttackCooldown = GetNode("Timers/Laser") as Timer;
        animFlashTimer = GetNode("Timers/AnimFlashTimer") as Timer;
        vulnerabilityTime = GetNode("Timers/Vulnerability") as Timer;


        health = 750 * Globals.diffMultipliers[Globals.Difficulty];
        damage = 30 * Globals.diffMultipliers[Globals.Difficulty];
        shardDropRate = Mathf.RoundToInt(500 * Globals.diffMultipliers[Globals.Difficulty]);
        //timings
        singleAttackCooldown.WaitTime = 5 / Globals.diffMultipliers[Globals.Difficulty];
        doubleAttackCooldown.WaitTime = 20 / Globals.diffMultipliers[Globals.Difficulty];
        sweepAttackCooldown.WaitTime = 30 / Globals.diffMultipliers[Globals.Difficulty];
        laserAttackCooldown.WaitTime = 90 / Globals.diffMultipliers[Globals.Difficulty];
        vulnerabilityTime.WaitTime = 20 / Globals.diffMultipliers[Globals.Difficulty];
        laserCount = Globals.Difficulty + 1;

        BossMechArmLeft.EyeFlash += BossMechArm_EyeFlash;
        BossMechArmRight.EyeFlash += BossMechArm_EyeFlash;
        BossMechArmLeft.AttackFinished += BossMechArm_AttackFinished;
        BossMechArmRight.AttackFinished += BossMechArm_AttackFinished;
        BossMechArmLeft.EngageLaser += LaserPowerUp;    //only 1 arm because both of them move no matter what, and this way it doesn't trigger twice
    }

    private void BossMechArm_AttackFinished(Enums.MechAttackType type)
    {
        attacking = false;
        switch (type)
        {
            case Enums.MechAttackType.Single:
                singleAttackCooldown.Start();
                break;
            case Enums.MechAttackType.Double:
                doubleAttackCooldown.Start();
                break;
            case Enums.MechAttackType.Sweep:
                sweepAttackCooldown.Start();
                break;
            case Enums.MechAttackType.Laser:
                laserAttackCooldown.Start();
                laserCount--;
                if(laserCount == 0)
                {
                    OpenCore();
                    laserCount = Globals.Difficulty + 1;
                }
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

            animPlayer.Play("BossMechStartup");
            BossMechArmLeft.LaunchSequence(animPlayer.CurrentAnimationLength);
            BossMechArmRight.LaunchSequence(animPlayer.CurrentAnimationLength);
        }
    }
    public void OnAnimationFinished(StringName anim)
    {
        if (anim.ToString() == "BossMechStartup")
        {
            singleAttackCooldown.Start();
            doubleAttackCooldown.Start();
            sweepAttackCooldown.Start();
            laserAttackCooldown.Start();
            sprite.Play("idle_closed");
            ACTIVE = true;
        }
        else if (anim.ToString() == "BossMechDeath")
        {
            parent.enemyAmount--;
            QueueFree();
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
    private void InitiateSweepAttack()
    {
        attacking = true;
        if (playerSide)
            BossMechArmRight.AttackSignal(Enums.MechAttackType.Sweep);
        else
            BossMechArmLeft.AttackSignal(Enums.MechAttackType.Sweep);
    }
    private void InitiateLaser()
    {
        attacking = true;
        BossMechArmLeft.AttackSignal(Enums.MechAttackType.Laser);
        BossMechArmRight.AttackSignal(Enums.MechAttackType.Laser);
    }
    private void LaserPowerUp()
    {
        //for animation
        laserBeam = laserBeamScene.Instantiate() as Laser;
        AddChild(laserBeam);
        laserBeam.TreeExiting += LaserBeam_TreeExiting;
        laserBeam.Position = laserLine.Position;
        laserBeam.RotateBeam(laserLine.TargetPosition.Angle());
        laserBeam.SetImpactPoint(laserLine.GetCollisionPoint() + new Vector2(0, -15));
        laserGrace = GetTree().CreateTimer(2);
        laserGrace.Timeout += LaserGrace_Timeout;
        laserActive = true;
        laserHitBox.CollideWithBodies = true;

    }
    private void LaserPowerDown()
    {
        laserBeam.TurnOff();
    }
    private void LaserBeam_TreeExiting()
    {
        laserSlashes = 3;
        laserLine.TargetPosition = new Vector2(-330, 199);
        laserHitBox.TargetPosition = laserLine.TargetPosition;
        laserActive = false;
        laserHitBox.CollideWithBodies = false;
    }
    private void EngageLaser()
    {
        laserMove = GetTree().CreateTween();
        laserMove.Finished += LaserMove_Finished;
        int dir = 0;
        if (laserLine.TargetPosition.X > 0)
            dir = -1;
        else 
            dir = 1;
        Vector2 targetPos = new Vector2(laserLine.TargetPosition.X + (660*dir), laserLine.TargetPosition.Y);
        laserMove.TweenProperty(laserLine, "target_position", targetPos, 2);
    }
    private void LaserGrace_Timeout()
    {
        laserGrace = null;
        EngageLaser();
    }
    private void LaserMove_Finished()
    {
        laserMove = null;
        laserSlashes--;
        if (laserSlashes > 0)
        {
            laserGrace = GetTree().CreateTimer(2);
            laserGrace.Timeout += LaserGrace_Timeout;
        }
        else
        {
            LaserPowerDown();
        }
    }

    private void OpenCore()
    {
        sprite.Play("cage_open");
        singleAttackCooldown.Paused = true;
        doubleAttackCooldown.Paused = true;
        sweepAttackCooldown.Paused = true;
        laserAttackCooldown.Paused = true;
        vulnerable = true;
        vulnerabilityTime.Start();
    }
    public void VulnerabilityTimeTimeout()
    {
        sprite.Play("cage_close");
        singleAttackCooldown.Paused = false;
        doubleAttackCooldown.Paused = false;
        sweepAttackCooldown.Paused = false;
        laserAttackCooldown.Paused = false;
        vulnerable = false;
        hitBox.Disabled = true;
    }
    public void Hit(float damage)
    {
        if (vulnerable && !dead)
        {
            sprite.Play("hurt");
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }
    private void Die()
    {
        dead = true;
        ACTIVE = false;
        DropItems(Enums.itemType.shield, 100);
        DropItems();
        animPlayer.Play("BossMechDeath");
        Globals.GameBeaten();
    }
    private void DropItems(Enums.itemType itemtype = Enums.itemType.shard, int customDropRate = 0)
    {
        Item item = null;
        if (itemtype == Enums.itemType.shard)
        {
            int dropamount = 0;
            if (shardDropRate > 100)
            {
                dropamount = Mathf.FloorToInt(shardDropRate / 100);
                for (int i = 0; i < dropamount; i++)
                {
                    item = itemScene.Instantiate() as Item;
                    item.type = itemtype;
                    if (item is not null)
                    {
                        item.GlobalPosition = GlobalPosition;
                        itemContainer.AddChild(item);
                        item = null;
                    }
                }
            }
            //regular calc & drop
            if (Math.RNG(shardDropRate - dropamount * 100))
            {
                item = itemScene.Instantiate() as Item;
                item.type = itemtype;
            }
        }
        else if (Math.RNG(customDropRate))
        {
            item = itemScene.Instantiate() as Item;
            item.type = itemtype;
        }


        if (item is not null)
        {
            item.GlobalPosition = GlobalPosition;
            itemContainer.AddChild(item);
        }
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
        if (sprite.Animation == "death")
        {
            
        }
        else if (sprite.Animation == "eye_flash_1")
            sprite.Play("idle_closed");
        else if (sprite.Animation == "eye_flash_2")
            sprite.Play("idle_closed");
        else if (sprite.Animation == "hurt")
            sprite.Play("idle_open");
        else if (sprite.Animation == "cage_open")
        {
            hitBox.Disabled = false;
            sprite.Play("idle_open");
        }
        else if (sprite.Animation == "cage_close")
            sprite.Play("idle_closed");
    }

    private void Update(double delta)
    {

        if (playerInRoom)
            StartupSequence();

        if (ACTIVE)
        {
            if (!Globals.player.GetIsDead() && !vulnerable && !dead)
            {
                //determine where player is in relation to itself
                playerSide = Globals.player.GlobalPosition.X > GlobalPosition.X ? true : false;
                //leaning
                if (!attacking)
                {
                    Movement(delta);
                }
                else Velocity = Vector2.Zero;

                //attacks
                if (!attacking && laserAttackCooldown.TimeLeft == 0)
                    InitiateLaser();
                if (!attacking && singleAttackCooldown.TimeLeft == 0)
                    InitiateSingleAttack();
                if (!attacking && doubleAttackCooldown.TimeLeft == 0)
                    InitiateDoubleAttack();
                if (!attacking && sweepAttackCooldown.TimeLeft == 0)
                    InitiateSweepAttack();

                if (laserActive)
                {
                    laserHitBox.TargetPosition = laserLine.TargetPosition;
                    laserBeam.RotateBeam(laserLine.TargetPosition.Angle());
                    laserBeam.SetImpactPoint(laserLine.GetCollisionPoint() + new Vector2(0, -15));
                    if (laserHitBox.IsColliding() && laserHitBox.GetCollider(0) is Player)  //0 is fine because this can only see player (and there is only 1 palyer)
                    {
                        Globals.player.HitTick(1 + Globals.Difficulty); //1; 2; 3 depending on diff
                    }
                }
            }




        }
        MoveAndSlide();
    }


    public float GetDamage() { return damage; }
    public EnemyControl GetController() { return GetParent() as EnemyControl; }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Update(delta);

    }

}
