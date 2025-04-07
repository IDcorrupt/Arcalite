using Godot;
using System;
using System.Linq.Expressions;
using System.Xml.XPath;
using static System.Net.Mime.MediaTypeNames;

public partial class BossMechArm : CharacterBody2D
{

    //stats
    private float acceleration = 2500f;
    private float maxSpeed = 350f;

    //variables
    //true if right, false if left
    [Export] private bool SIDE;
    
    private Vector2 origin;
    //sequencing for update loop    0 = no attack  |  1 = windup  |  2 = grace  |  3 = attack   (2 is empty rn)
    private int atkSequence = 0;
    //attack configs
    private float singleAtkWindupTime = 5 / Globals.diffMultipliers[Globals.Difficulty];
    private float doubleAtkWindupTime = 3 / Globals.diffMultipliers[Globals.Difficulty];
    private float sweepAtkWindupTime = 2 / Globals.diffMultipliers[Globals.Difficulty];
    private float laserAtkWindupTime = 4 / Globals.diffMultipliers[Globals.Difficulty];
    private int spikeAmount = 0;    //how many spikes the attacks generate (in each direction)
    private bool singleAtk = false;
    private bool doubleAtk = false;
    private bool sweepAtk = false;
    private bool laserAtk = false;
    private bool playerInRange = false;

    private Tween positionTween;
    private Tween rotateTween;
    private SceneTreeTimer spikeTimer;
    private Vector2 spikeSpawnLeft = Vector2.Zero;
    private Vector2 spikeSpawnRight = Vector2.Zero;

    public bool isSlowed = false;
    public float slowFactor = 1f;

    //components
    private PackedScene groundSpikeScene = (PackedScene)ResourceLoader.Load("res://Nodes/Game/enemies/projectiles/ground_spike.tscn");
    private Node2D spikeOrigin;
    private BossMech parent;
    private AnimatedSprite2D sprite;
    private Area2D playerDetect;
    private CollisionShape2D solidHitBox;   //so player can stand on arms ("safespace")
    private Timer launchSequenceTimer;
    private Timer atkWindup;
    private Timer atkGrace;




    //signals
    [Signal] public delegate void EyeFlashEventHandler(int type);
    [Signal] public delegate void AttackFinishedEventHandler(Enums.MechAttackType type);
    [Signal] public delegate void EngageLaserEventHandler();

    public override void _Ready()
    {
        base._Ready();


        parent = GetParent().GetParent() as BossMech;
        sprite = GetNode("Sprite") as AnimatedSprite2D;
        playerDetect = GetNode("PlayerDetect") as Area2D;
        solidHitBox = GetNode("PlayerHitBox/CollisionShape2D") as CollisionShape2D;
        spikeOrigin = GetNode("SpikeOrigin") as Node2D;
        launchSequenceTimer = GetNode("LaunchSequenceTimer") as Timer;
        atkWindup = GetNode("TargetingTimer") as Timer;
        atkGrace = GetNode("AwaitTimer") as Timer;
        launchSequenceTimer.Timeout += LaunchSequenceTimer_Timeout;

        solidHitBox.Disabled = false;
        //dynamic didn't work because of animplayer, need to change this when final sprites are done
        if (SIDE)
        {
            sprite.FlipH = false;
            origin = new Vector2(200, -25);
        }
        else
        {
            sprite.FlipH = true;
            origin = new Vector2(-200, -25);
        }
    }


    public void LaunchSequence(double time)
    {
        launchSequenceTimer.Start(time);
    }
    private void LaunchSequenceTimer_Timeout() 
    {
        SetCollisionMaskValue(3, true);
        sprite.Play("default");
    }

    public void OnPlayerDetectAreaEntered(Node2D body)
    {
        if (body is Player)
            playerInRange = true;
    }
    public void OnPlayerDetectAreaExited(Node2D body)
    {
        if(body is Player)
            playerInRange = false;
    }

    public void AttackSignal(Enums.MechAttackType type)
    {
        spikeSpawnLeft = Vector2.Zero;
        spikeSpawnRight = Vector2.Zero;
        switch (type)
        {
            case Enums.MechAttackType.Single:
                singleAtk = true;
                atkSequence = 1;
                atkWindup.Start(singleAtkWindupTime);
                spikeAmount = 5;
                break;
            case Enums.MechAttackType.Double:
                doubleAtk = true;
                atkSequence = 1;
                atkWindup.Start(doubleAtkWindupTime);
                spikeAmount = 10;
                break;
            case Enums.MechAttackType.Sweep:
                sweepAtk = true;
                SetCollisionMaskValue(3, false);
                atkSequence = 1;
                atkWindup.Start(sweepAtkWindupTime);
                break;
            case Enums.MechAttackType.Laser:
                laserAtk = true;
                atkSequence = 1;
                atkWindup.Start(laserAtkWindupTime);
                break;
            default:
                break;
        }
    }
    public void AtkWindupTimeout()
    {
        if (laserAtk)
            EmitSignal(SignalName.EyeFlash, 2);
        else
            EmitSignal(SignalName.EyeFlash, 1);
        Velocity = Vector2.Zero;
        atkSequence = 2;
        atkGrace.Start(0.5);
    }
    public void AtkGraceTimeout()
    {
        if (atkSequence == 2)
        {
            if (laserAtk)
                EmitSignal(SignalName.EngageLaser);
            atkSequence = 3;
        }
        else
            ReturnToOrigin();
    }
    private void SingleAttack()
    {
        atkSequence = 0;
        if(spikeAmount > 0)
            CreateSpikes();
        atkGrace.Start(2);
    }
    private void DoubleAttack()
    {
        atkSequence = 0;
        if (spikeAmount > 0)
            CreateSpikes();
        atkGrace.Start(3);
    }
    private void SweepAttack()
    {
        Vector2 hitVector = new Vector2(1200 * (SIDE ? -1 : 1), -500);
        Globals.player.Hit(parent.GetDamage(), hitVector);
    }
    private void CreateSpikes()
    {
        if(spikeSpawnLeft == Vector2.Zero)
            spikeSpawnLeft = spikeOrigin.GlobalPosition + new Vector2(-10, 0);
        if(spikeSpawnRight == Vector2.Zero)
            spikeSpawnRight = spikeOrigin.GlobalPosition + new Vector2(10, 0);
        GroundSpike leftSpike = groundSpikeScene.Instantiate() as GroundSpike;
        parent.GetController().AddChild(leftSpike);
        leftSpike.SetStats(parent.GetDamage(), true);
        leftSpike.GlobalPosition = spikeSpawnLeft;
        leftSpike.Name = "LeftGroundSpike"+spikeAmount.ToString();
        GroundSpike rightSpike = groundSpikeScene.Instantiate() as GroundSpike;
        parent.GetController().AddChild(rightSpike);
        rightSpike.SetStats(parent.GetDamage(), false);
        rightSpike.GlobalPosition = spikeSpawnRight;
        leftSpike.Name = "RightGroundSpike"+spikeAmount.ToString();

        spikeTimer = GetTree().CreateTimer(0.1);
        spikeTimer.Timeout += SpikeTimer_Timeout;
    }
    private void SpikeTimer_Timeout()
    {
        spikeAmount--;
        spikeTimer = null;
        if(spikeAmount > 0)
        {
            spikeSpawnLeft += new Vector2(-20, 0);
            spikeSpawnRight += new Vector2(20, 0);
            CreateSpikes();
        }
    }

    private void ReturnToOrigin()
    {
        RotateElements(0, 1);
        var returnal = GetTree().CreateTween();
        returnal.Finished += Returnal_Finished;
        returnal.TweenProperty(this, "position", origin, 1);
    }
    private void Returnal_Finished()
    {
        if (singleAtk)
        {
            singleAtk = false;
            EmitSignal(SignalName.AttackFinished, 0);
        }
        else if (doubleAtk)
        {
            doubleAtk = false;
            EmitSignal(SignalName.AttackFinished, 1);
        }
        else if (sweepAtk)
        {
            sweepAtk = false;
            SetCollisionMaskValue(3, true);
            EmitSignal(SignalName.AttackFinished, 2);
        }
        else if (laserAtk)
        {
            laserAtk = false;
            EmitSignal(SignalName.AttackFinished, 3);
        }
    }
    private void RotateTween_Finished()
    {
        //clear itself
        rotateTween = null;
    }
    private void MovementTween_Finished()
    {
        //clear itself
        positionTween = null;
    }
    private void RotateElements(float degrees, float timer)
    {
        StaticBody2D hbparent = solidHitBox.GetParent() as StaticBody2D;
        float radians = Mathf.DegToRad(degrees);
        var spriteRotTween = GetTree().CreateTween();
        var PDRotTween = GetTree().CreateTween();
        var HBRotTween = GetTree().CreateTween();

        spriteRotTween.TweenProperty(sprite, "rotation", radians, timer);
        PDRotTween.TweenProperty(playerDetect, "rotation", radians, timer);
        HBRotTween.TweenProperty(hbparent, "rotation", radians, timer);
    }

    private void Update(double delta)
    {

        if (singleAtk)
        {
            if(atkSequence == 1)
            {
                //disable solid hitbox so player can't stand on top of arm and avoid attack
                solidHitBox.Disabled = true;
                Vector2 target = Globals.player.GlobalPosition - parent.GlobalPosition;
                if (Position.X < target.X)
                {
                    //right (+)
                    if (Position.X + 20 < target.X)
                        //accel
                        Velocity = new Vector2(Mathf.Clamp(Velocity.X+acceleration*(float)delta, 0, maxSpeed*slowFactor), Velocity.Y);
                    else
                        //decel
                        Velocity = new Vector2(Mathf.Clamp(Velocity.X - acceleration * (float)delta, 0, maxSpeed*slowFactor), Velocity.Y);
                }
                else if(Position.X > target.X)
                {
                    //left (-)
                    if (Position.X - 20 > target.X)
                        //accel
                        Velocity = new Vector2(Mathf.Clamp(Velocity.X - acceleration * (float)delta, -maxSpeed*slowFactor, 0), Velocity.Y);
                    else
                        //decel
                        Velocity = new Vector2(Mathf.Clamp(Velocity.X + acceleration * (float)delta, -maxSpeed * slowFactor, 0), Velocity.Y);
                }
                else
                {
                    Velocity = Vector2.Zero;
                }
            }
            if(atkSequence == 3)
            {
                solidHitBox.Disabled = true;
                if (!IsOnFloor())
                {
                    Velocity = new Vector2(Velocity.X, 1000*slowFactor);
                }
                else
                {
                    Velocity = Vector2.Zero;
                    SingleAttack();
                }
            }
                
        }
        else if (doubleAtk)
        {
            if(atkSequence == 1)
            {
                Vector2 movePos = new Vector2(SIDE ? 225 : -225, Position.Y-50);
                if (positionTween == null)
                {
                    positionTween = GetTree().CreateTween();
                    positionTween.Finished += MovementTween_Finished;
                    positionTween.TweenProperty(this, "position", movePos, atkWindup.TimeLeft);  //using timeleft instead of static value for dynamicity & precision
                }
            }
            if(atkSequence == 3)
            {
                if (!IsOnFloor())
                {
                    Velocity = new Vector2(Velocity.X, 1000 * slowFactor);
                }
                else
                {
                    Velocity = Vector2.Zero;
                    DoubleAttack();
                }
            }
        }
        else if (sweepAtk)
        {
            if (atkSequence == 1)
            {
                Vector2 movePos = new Vector2(SIDE ? 320 : -320, 85);
                float rotation = SIDE ? 90 : -90;
                if (positionTween == null)
                {
                    positionTween = GetTree().CreateTween();
                    positionTween.Finished += MovementTween_Finished;
                    positionTween.TweenProperty(this, "position", movePos, atkWindup.TimeLeft);  //using timeleft instead of static value for dynamicity & precision
                    RotateElements(rotation, (float)atkWindup.TimeLeft);
                }

            }
            if(atkSequence == 3)
            {
                solidHitBox.OneWayCollision = false;

                if ((SIDE ? Position.X < -300 : Position.X > 300) && Velocity == Vector2.Zero)
                {
                    solidHitBox.OneWayCollision = true;
                    atkSequence = 0;
                    atkGrace.Start(1);
                    return;
                }
                if (playerInRange)
                {
                    SweepAttack();
                }
                if (SIDE ? Position.X > -300 : Position.X < 300)
                {
                    //accel
                    Velocity = new Vector2()
                    {
                        X = Velocity.X + 800 * (float)delta * (SIDE ? -1 : 1),
                        Y = 0
                    };
                }
                else
                {
                    //decel
                    if (SIDE)
                    {
                        //right arm -> going left
                        Velocity = new Vector2()
                        {
                            X = Mathf.Clamp(Velocity.X + 4000 * (float)delta, -1000000, 0),
                            Y = 0
                        };
                    }
                    else
                    {
                        //left arm -> going right
                        Velocity = new Vector2()
                        {
                            X = Mathf.Clamp(Velocity.X - 4000 * (float)delta, 0,  1000000),
                            Y = 0
                        };
                    }
                }
            }
        }
        else if (laserAtk)
        {
            if(atkSequence == 1)
            {
                Vector2 movePos = new Vector2(SIDE ? 150 : -150, Position.Y);
                if (positionTween == null)
                {
                    positionTween = GetTree().CreateTween();
                    positionTween.Finished += MovementTween_Finished;
                    positionTween.TweenProperty(this, "position", movePos, atkWindup.TimeLeft);  //using timeleft instead of static value for dynamicity & precision
                }
            }if(atkSequence == 3)
            {
                atkSequence = 0;
                atkGrace.Start(14);
            }
        }
        MoveAndSlide();
    }



    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        //modifier for oracle functionality
        if (!isSlowed)
        {
            slowFactor = 1;
        }

        Update(delta);
    }
}
