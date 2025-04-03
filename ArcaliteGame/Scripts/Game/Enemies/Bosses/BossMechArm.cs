using Godot;
using System;
using System.Linq.Expressions;

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
    //attackBools
    private bool singleAtk = false;
    private bool doubleAtk = false;
    private bool swipeAtk = false;
    private bool laserAtk = false;
    private bool playerInRange = false;

    private Tween movementTween;
    public bool isSlowed = false;
    public float slowFactor = 1f;

    //components
    private BossMech parent;
    private AnimatedSprite2D sprite;
    private Area2D playerDetect;
    private Timer launchSequenceTimer;
    private Timer atkWindup;
    private Timer atkGrace;

    //signals
    [Signal] public delegate void EyeFlashEventHandler(int type);
    [Signal] public delegate void AttackFinishedEventHandler(Enums.MechAttackType type);

    public override void _Ready()
    {
        base._Ready();
        parent = GetParent().GetParent() as BossMech;
        sprite = GetNode("Sprite") as AnimatedSprite2D;
        playerDetect = GetNode("AtkRange") as Area2D;
        launchSequenceTimer = GetNode("LaunchSequenceTimer") as Timer;
        atkWindup = GetNode("TargetingTimer") as Timer;
        atkGrace = GetNode("AwaitTimer") as Timer;
        launchSequenceTimer.Timeout += LaunchSequenceTimer_Timeout;

        //dynamic didn't work because of animplayer, need to change this when final sprites are done
        if (SIDE)
            origin = new Vector2(200, -25);
        else
            origin = new Vector2(-200, -25);
    }


    public void LaunchSequence(double time)
    {
        launchSequenceTimer.Start(time);
    }
    private void LaunchSequenceTimer_Timeout() 
    {
        SetCollisionMaskValue(3, true);
    }


    public void AtkRangeBodyEntered(Node2D body)
    {
        playerInRange = true;
    }
    public void AtkRangeBodyExited(Node2D body)
    {
        playerInRange = false;
    }
    public void AttackSignal(Enums.MechAttackType type)
    {
        CollisionShape2D atkRange = playerDetect.GetNode("CollisionShape2D") as CollisionShape2D;
        RectangleShape2D atkRect = atkRange.Shape as RectangleShape2D;
        switch (type)
        {
            case Enums.MechAttackType.Single:
                singleAtk = true;
                atkSequence = 1;
                atkWindup.Start(5);

                //set attack area
                atkRect.Size = new Vector2(140, atkRect.Size.Y);
                break;
            case Enums.MechAttackType.Double:
                doubleAtk = true;
                atkSequence = 1;
                atkWindup.Start(3);

                //set attack area
                atkRect.Size = new Vector2(250, atkRect.Size.Y);
                break;
            case Enums.MechAttackType.Sweep:
                atkSequence = 1;
                break;
            case Enums.MechAttackType.Laser:
                atkSequence = 1;
                break;
            default:
                break;
        }
    }
    public void AtkWindupTimeout()
    {
        Velocity = Vector2.Zero;
        EmitSignal(SignalName.EyeFlash, 1);
        atkSequence = 2;
        atkGrace.Start(0.5);

    }
    public void AtkGraceTimeout()
    {
        if(atkSequence == 2) 
            atkSequence = 3;
        else
            ReturnToOrigin();
    }
    private void SingleAttack()
    {
        atkSequence = 0;
        if (playerInRange)
        {
            int dir = 0;
            if ((Globals.player.GlobalPosition - GlobalPosition).Normalized().X > 0)
                dir = 1;
            else if ((Globals.player.GlobalPosition - GlobalPosition).Normalized().X < 0)
                dir = -1;
            Vector2 hitVector = new Vector2(700 * dir, -400);
            Globals.player.Hit(parent.GetDamage(), hitVector);
        }
        atkGrace.Start(2);
    }
    private void DoubleAttack()
    {
        atkSequence = 0;
        if (playerInRange)
        {
            int dir = 0;
            if ((Globals.player.GlobalPosition - GlobalPosition).Normalized().X > 0)
                dir = 1;
            else if ((Globals.player.GlobalPosition - GlobalPosition).Normalized().X < 0)
                dir = -1;
            Vector2 hitVector = new Vector2(700 * dir, -400);
            Globals.player.Hit(parent.GetDamage(), hitVector);
        }
        atkGrace.Start(3);
    }

    private void ReturnToOrigin()
    {
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
        else if (swipeAtk)
        {
            swipeAtk = false;
            EmitSignal(SignalName.AttackFinished, 2);
        }
        else if (laserAtk)
        {
            laserAtk = false;
            EmitSignal(SignalName.AttackFinished, 3);
        }
    }

    private void Animate()
    {

    }

    private void Update(double delta)
    {

        if (singleAtk)
        {
            if(atkSequence == 1)
            {
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
                if (movementTween == null)
                {
                    movementTween = GetTree().CreateTween();
                    movementTween.Finished += MovementTween_Finished;
                    movementTween.TweenProperty(this, "position", movePos, atkWindup.TimeLeft);  //using timeleft instead of static value for dynamicity & precision
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
        else if (swipeAtk)
        {

        }
        MoveAndSlide();
        Animate();
    }

    private void MovementTween_Finished()
    {
        //clear itself
        movementTween = null;
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
