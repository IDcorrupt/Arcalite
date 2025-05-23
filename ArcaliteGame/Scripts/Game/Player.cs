using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;

public partial class Player : CharacterBody2D
{
//values
    //crouch
    private int defHB_X = 16;       //hitbox for crouch
    private int defHB_Y = 29;       //hitbox for crouch
    private bool isCrouching = false;    //crouch bool
    private bool crouchDown = false;    // up & down required for animations
    private bool crouchUp = false;

    //movement
    private int maxSpeed = 300;         //maximum X vector value
    private double vel = 0;             //X velocity
    private int jumpStrength = 450;     //jump height/strength
    private const float GRAVITY = Globals.GRAVITY;
    private float prevDir = 0;          //last movement direction for deceleration

    //dash
    private bool dashed = false;            //dash initiated
    private float dashSpeed = 2000f;        //initial dash speed
    private float dashDecayRate = 15000f;   //dash speed decay rate
    private float currentDashSpeed = 0f;    //current dash speed
    private bool isDashing = false;         //is dash currently active
    private Vector2 dashVector;             //fixed vector for dash endpoint -> dash follows mouse otherwise :3

    //attacks
    private bool CAisCharging = false;  //is charge attack currently charging
    private float CACharge = 0f;        //used to track charge progress
    private int chargeLevel = 0;
    private float speedmodifier = 1;
    private int BADispersion = 0;

    //EQ abilities
    private Enums.itemType spellItemE = Enums.itemType.empty;
    private Enums.itemType spellItemQ = Enums.itemType.empty;
    private bool rapidFire;
    private bool shielded;

    //other
    private bool isHurt = false;
    private bool isDead = false;
    private bool resting = false;
    private float HPRechargeAmount = 0;

//stats
    private float MaxHP = 100;
    private float MaxMP = 100;
    //spd & dot if class system get implemented
    private float currentHP;
    private float currentMP;

    private float damage;

    private int oracleLevel;

//components
    private CollisionShape2D HitBox;
    private AnimatedSprite2D Sprite;

    private Timer hurtTimer;
    private Timer dashCooldown;
    //attackcooldowns
    private float BACooldownStatic; //needed because rapidfire changes waittime
    private Timer BACooldown;
    private Timer CACooldown;
    private Timer SOCooldown;
    private Timer SECooldown;
    private Timer SQCooldown;
    private Timer spellETimer;
    private Timer spellQTimer;
    private Timer statRecharge;

    private GpuParticles2D FX_Charge;
    private AnimatedSprite2D FX_Rapid;
    private AnimatedSprite2D FX_Shield;
    //signals
    [Signal] public delegate void DashedEventHandler(float cooldown);
    [Signal] public delegate void ChargeAttackedEventHandler(float cooldown);
    [Signal] public delegate void OracleCastEventHandler(float cooldown);
    [Signal] public delegate void RapidFireCastEventHandler(float activeTime);   
    [Signal] public delegate void ShieldCastEventHandler(float activeTime);
    [Signal] public delegate void SpellEOverEventHandler(float cooldown);
    [Signal] public delegate void SpellQOverEventHandler(float cooldown);
    [Signal] public delegate void ItemsModifiedEventHandler();
    [Signal] public delegate void EnteredRestAreaEventHandler();
    [Signal] public delegate void ExitedRestAreaEventHandler();
    public override void _Ready()
    {
        Globals.player = this;
        //Get nodes
        HitBox = GetNode("HitBox") as CollisionShape2D;
        Sprite = GetNode("AnimatedSprite2D") as AnimatedSprite2D;
        hurtTimer = GetNode("Timers/HurtTimer") as Timer;
        dashCooldown = GetNode("Timers/DashCooldown") as Timer;
        BACooldown = GetNode("Timers/BasicAttackCooldown") as Timer;
        CACooldown = GetNode("Timers/ChargeAttackCooldown") as Timer;
        SOCooldown = GetNode("Timers/OracleCooldown") as Timer;
        SECooldown = GetNode("Timers/SpellECooldown") as Timer;
        SQCooldown = GetNode("Timers/SpellQCooldown") as Timer;
        spellETimer = GetNode("Timers/SpellETimer") as Timer;
        spellQTimer = GetNode("Timers/SpellQTimer") as Timer;
        statRecharge = GetNode("Timers/StatRecharge") as Timer;


        //basic attack cooldown doesn't change with "levels"
        BACooldownStatic = 0.2f;

        FX_Charge = GetNode("FX/Charge") as GpuParticles2D;
        FX_Rapid = GetNode("FX/Rapid") as AnimatedSprite2D;
        FX_Shield = GetNode("FX/Shield") as AnimatedSprite2D;



        //set stats
        SetStats();

        //go to spawnpoint
        GlobalPosition = Globals.spawnPoint.Position;
    }


    //movement functions
    public Vector2 getInputs()
    {
        Vector2 direction = new();

        direction.X = Convert.ToInt32(Input.IsActionPressed("move_right")) + Convert.ToInt32(Input.IsActionPressed("move_right-alt")) - Convert.ToInt32(Input.IsActionPressed("move_left")) - Convert.ToInt32(Input.IsActionPressed("move_left-alt"));
        if (direction.X == 1) Sprite.FlipH = false;
        else if (direction.X == -1) Sprite.FlipH = true;
        if ((Input.IsActionPressed("move_jump") || Input.IsActionPressed("move_jump-alt")) && !CAisCharging)
        {
            if (IsOnFloor())
            {
                direction.Y = -1;
            }
        }
        if (Input.IsActionPressed("move_crouch") || Input.IsActionPressed("move_crouch-alt"))isCrouching = true;
        else isCrouching = false;
        if ((Input.IsActionPressed("move_dash") || Input.IsActionPressed("move_dash-alt")) && dashCooldown.TimeLeft == 0)
        {
            if (Globals.DashMode == 0)
            {
                dashVector = new Vector2()
                {
                    X = Convert.ToInt32(Input.IsActionPressed("move_right")) + Convert.ToInt32(Input.IsActionPressed("move_right-alt")) - Convert.ToInt32(Input.IsActionPressed("move_left")) - Convert.ToInt32(Input.IsActionPressed("move_left-alt")),
                    Y = Convert.ToInt32(Input.IsActionPressed("move_crouch")) + Convert.ToInt32(Input.IsActionPressed("move_crouch-alt")) - Convert.ToInt32(Input.IsActionPressed("move_jump")) - Convert.ToInt32(Input.IsActionPressed("move_jump-alt")),
                };
            }
            dashed = true;
        }

        return direction;
    }
    public void Movement(double delta)
    {
        //movement controls interrupted when dash is in progress
        if (isDashing)
        {
            if (currentDashSpeed > 0)
            {
                //gradually decay dash speed
                currentDashSpeed -= dashDecayRate * (float)delta;
                Velocity = dashVector * Mathf.Max(currentDashSpeed, 0);
            }
            else
            {
                //exit dash
                isDashing = false;
                EmitSignal(SignalName.Dashed, dashCooldown.WaitTime);
            }
            MoveAndSlide();
            return;
        }

        //initiate dash
        if (dashed && !CAisCharging)
        {
            dashed = false;
            dashCooldown.Start();
            Dash();
        }

        //normal movement
        Vector2 input = getInputs();
        if (input.X != 0)
        {
            if (vel < maxSpeed) vel += delta * 2000;
            else if (vel > maxSpeed) vel -= delta * 2000;
            Velocity = new Vector2((float)(input.X * vel) * speedmodifier, Velocity.Y);
            prevDir = input.X;
        }
        else if (input.X == 0)
        {
            if (vel > 0)
                vel -= delta * 2500;
            else
                vel = 0;
            Velocity = new Vector2((float)(prevDir * vel) * speedmodifier, Velocity.Y);
        }
        if (input.Y != 0)
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y - jumpStrength);
        }

        CrouchApply();
        Fall(delta);
    }
    public void Dash()
    {
        //set dash diretion and engage
        if (Globals.DashMode == 1)
            dashVector = (GetGlobalMousePosition() - GlobalPosition).Normalized();
        else if (dashVector == Vector2.Zero)
            dashVector = new Vector2(prevDir, 0);
        currentDashSpeed = dashSpeed;
        Velocity = dashVector * currentDashSpeed;
        isDashing = true;
        dashed = false;
    }
    public void Fall(double delta)
    {
        Velocity = new Vector2(Velocity.X, Velocity.Y + (float)(GRAVITY * delta));
    }
    public void CrouchApply()
    {
        if (isCrouching)
        {
            Velocity = new Vector2((float)(Velocity.X / 1.5), Velocity.Y);
            if (Velocity.Y > 0)
            {
                Velocity = new Vector2(Velocity.X, (float)(Velocity.Y * 1.2));
            }
            if (HitBox.Shape is RectangleShape2D rectangleShape)
            {
                HitBox.Position = new Vector2(HitBox.Position.X, (float)2.143);
                rectangleShape.Size = new Vector2(defHB_X, defHB_Y - 5);
            }
        }
        else
        {
            if (HitBox.Shape is RectangleShape2D rectangleShape)
            {
                HitBox.Position = new Vector2(HitBox.Position.X, (float)-0.714);
                rectangleShape.Size = new Vector2(defHB_X, defHB_Y);

            }
        }

    }

    //attack functions
    public void BasicAttack()
    {
        BasicProjectile projectile = PreloadRegistry.Game.Projectiles.basicProjectile.Instantiate() as BasicProjectile;
        GetParent().GetParent().AddChild(projectile);
        projectile.Position = GlobalPosition;
        Vector2 direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
        if (BADispersion > 0)
        {
            Random disp = new Random();
            direction = direction.Rotated(((float)disp.NextDouble()-0.5f)/2);
        }
        projectile.Rotation = direction.Angle();
        projectile.direction = direction;
        projectile.damagePayload = damage;

    }
    public void ChargeAttack(int chargeLevel)
    {
        ChargeProjectile projectile = PreloadRegistry.Game.Projectiles.chargeProjectile.Instantiate() as ChargeProjectile;
        GetParent().GetParent().AddChild(projectile);
        projectile.Position = GlobalPosition;
        Vector2 direction = (GetGlobalMousePosition() - GlobalPosition).Normalized();
        projectile.chargeLevel = chargeLevel;
        projectile.Rotation = direction.Angle();
        projectile.direction = direction;
        projectile.damagePayload = damage;
        projectile.imports = true;
        EmitSignal(SignalName.ChargeAttacked, CACooldown.WaitTime);
    }
    public void SpellOracle()
    {
        SpellOracle oracle = PreloadRegistry.Game.Projectiles.spellOracle.Instantiate() as SpellOracle;
        GetParent().AddChild(oracle);
        oracle.targetPosition = GlobalPosition;
        oracle.level = oracleLevel;
        EmitSignal(SignalName.OracleCast, SOCooldown.WaitTime);
    }
    public void SpellE()
    {
        switch (spellItemE)
        {
            case Enums.itemType.necklace:
                RapidFireAbility("E");
                break;
            case Enums.itemType.shield:
                ShieldAbility("E");
                break;
            default: 
                break;
        }
    }
    public void SpellQ()
    {
        switch (spellItemQ)
        {
            case Enums.itemType.necklace:
                RapidFireAbility("Q");
                break;
            case Enums.itemType.shield:
                ShieldAbility("Q");
                break;
            default: break;
        }
    }
    public void SpellETimerTimeout()
    {
        EmitSignal(SignalName.SpellEOver, SECooldown.WaitTime);
        switch (spellItemE)
        {

            case Enums.itemType.necklace:
                rapidFire = false;
                break;
            case Enums.itemType.shield:
                shielded = false;
                FX_Shield.SpriteFrames.SetAnimationLoop("default", false);
                break;
            default:
                break;
        }
        SECooldown.Start();
    }
    public void SpellQTimerTimeout()
    {
        EmitSignal(SignalName.SpellQOver, SQCooldown.WaitTime);
        switch (spellItemQ)
        {
            case Enums.itemType.necklace:
                rapidFire = false;
                break;
            case Enums.itemType.shield:
                shielded = false;
                FX_Shield.SpriteFrames.SetAnimationLoop("default", false);
                break;
            default:
                break;
        }
        SQCooldown.Start();
    }

    //item functions
    private void RapidFireAbility(string slot)
    {
        //necklace item ability: reduced attack cooldown, increased dispersion
        rapidFire = true;
        FX_Rapid.Play("default");
        currentMP -= 40;
        switch (slot)
        {
            case "E":
                spellETimer.WaitTime = 5;
                EmitSignal(SignalName.RapidFireCast, spellETimer.WaitTime);
                spellETimer.Start();
                break;
            case "Q":
                spellQTimer.WaitTime = 5;
                EmitSignal(SignalName.RapidFireCast, spellQTimer.WaitTime);
                spellQTimer.Start();
                break;
            default : break;
        }
    }
    private void ShieldAbility(string slot)
    {
        //attack immunity for 2 sec (time not final)
        shielded = true;
        FX_Shield.Play("default");
        FX_Shield.SpriteFrames.SetAnimationLoop("default", true);
        currentMP -= 20;
        switch (slot)
        {
            case "E":
                spellETimer.WaitTime = 2;
                EmitSignal(SignalName.ShieldCast, spellETimer.WaitTime);
                spellETimer.Start();
                break;
            case "Q":
                spellQTimer.WaitTime = 2;
                EmitSignal(SignalName.ShieldCast, spellQTimer.WaitTime);
                spellQTimer.Start();
                break;
            default: break;
        }

    }   
    public void PickupItem(Enums.itemType itemtype, float cooldown)
    {
        switch (itemtype)
        {
            case Enums.itemType.necklace:
                //equip only if other item doesn't have it already
                if (spellItemE is Enums.itemType.empty && spellItemQ != Enums.itemType.necklace)
                {
                    spellItemE = Enums.itemType.necklace;
                    SECooldown.WaitTime = cooldown;
                }
                else if (spellItemQ is Enums.itemType.empty && spellItemE != Enums.itemType.necklace)
                {
                    spellItemQ = Enums.itemType.necklace;
                    SQCooldown.WaitTime = cooldown;
                }
                break;
            case Enums.itemType.shield:
                if (spellItemE is Enums.itemType.empty && spellItemQ != Enums.itemType.shield)
                {
                    spellItemE = Enums.itemType.shield;
                    SECooldown.WaitTime = cooldown;
                }
                else if (spellItemQ is Enums.itemType.empty && spellItemE != Enums.itemType.shield)
                {
                    spellItemQ = Enums.itemType.shield;
                    SQCooldown.WaitTime = cooldown;
                }
                break;
            case Enums.itemType.shard:
                MaxHP += 10;
                currentHP += 10;
                damage += 2;
                break;
            default:
                break;
        }
        if(itemtype != Enums.itemType.shard)
            EmitSignal(SignalName.ItemsModified);

    }

    //damage functions
    public void Hit(float damage, Vector2 hitVector)
    {
        if (!isHurt && !shielded && !isDashing)
        {
            currentHP -= damage;
            
            //reset dash speed to avoid dash buffering
            currentDashSpeed = 0;
            if(currentHP <= 0)
            {
                currentHP = 0;
                isDead = true;
                return;
            }
            isHurt = true;
            if (hitVector != Vector2.Zero)
            {
                Velocity = hitVector;
                hurtTimer.WaitTime = 1;
            }
            else
            {
                hurtTimer.WaitTime = 0.5f;
            }
            hurtTimer.Start();

        }
    }
    public void HitTick(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            return;
        }
    }
    public void OnHurtTimerTimeout() { isHurt = false; }


    //other functions
    private void Animate()
    {
        //jump & fall
        if (!IsOnFloor() && Velocity.Y < 0)
        {
            Sprite.Play("jump");
        }
        else if (!IsOnFloor() && Velocity.Y > 0)
        {
            Sprite.Play("fall");
        }
        else if (IsOnFloor())
        {
            //crouch
            RectangleShape2D HBShape = (RectangleShape2D)HitBox.Shape;
            if (isCrouching && Sprite.Animation != "crouch")
            {
                crouchDown = true;
                Sprite.Play("crouch");
            }
            else if (!isCrouching && Sprite.Animation == "crouch" && crouchUp == false)
            {
                crouchUp = true;
                crouchDown = false;
                Sprite.Play("crouch", -1, true);
            }
            if (Sprite.Animation == "crouch" && crouchUp && Sprite.Frame == 0)
            {
                crouchUp = false;
            }

            //walk
            if (!crouchDown && !crouchUp)
            {
                if (Velocity.X != 0 && Sprite.Animation != "walk") Sprite.Play("walk");
                if (Velocity.X == 0 && Sprite.Animation != "idle")
                {
                    Sprite.Play("idle");
                }
            }
            else
            {
                //crouch walk anims here
            }
        }
    }
    public void Rest(bool state, float amount = 0)
    {
        if (state)
        {
            resting = true;
            if(amount > 0) 
                HPRechargeAmount = amount;
        }else
        {
            resting = false;
        }
    }
    private void Cooldowns()
    {
        bool statTimer = false;
        //mana always recharges (1/sec)
        if (statRecharge.TimeLeft == 0 && currentMP < MaxMP)
        {
            currentMP++;
            statTimer = true;
        }
        if (resting && statRecharge.TimeLeft == 0 && currentHP < MaxHP && HPRechargeAmount > 0)
        {
            currentHP++;
            HPRechargeAmount--;
            statTimer = true;
        }

        //hp recharges in rest zone + tick rate is 20/sec
        if (resting)  //when in rest zone
            statRecharge.WaitTime = 0.05f;
        else 
            statRecharge.WaitTime = 1f;

        if (statTimer)
            statRecharge.Start();
    }
    public void SetStats()
    {
        if (Globals.hasSavefile)
        {
            //if loading from save
            //stats
            MaxHP = (float)Convert.ToDecimal(Globals.currentSave[3]);
            MaxMP = (float)Convert.ToDecimal(Globals.currentSave[4]);
            currentHP = (float)Convert.ToDecimal(Globals.currentSave[5]);
            currentMP = (float)Convert.ToDecimal(Globals.currentSave[6]);
            damage = (float)Convert.ToDecimal(Globals.currentSave[7]);

            //items
            oracleLevel = 2;
            string[] items = Globals.currentSave[8].Split(";");
            spellItemE = (Enums.itemType)Convert.ToInt32(items[0]);
            spellItemQ = (Enums.itemType)Convert.ToInt32(items[1]);
        }
        else
        {
            //if new save
            //stats
            MaxHP = 100;
            MaxMP = 100;
            currentHP = MaxHP;
            currentMP = MaxMP;
            damage = 5;
            oracleLevel = 2;

        }

        //cooldowns
        dashCooldown.WaitTime = 2f;
        CACooldown.WaitTime = 5f;
        SOCooldown.WaitTime = 20f;
        SECooldown.WaitTime = 10f;
        SQCooldown.WaitTime = 10f;

        //reset state
        vel = 0;
        prevDir = 0;
        Velocity = Vector2.Zero;
        isHurt = false;
        isDead = false;
        isDashing = false;
        isCrouching = false;
        GlobalPosition = Globals.spawnPoint.Position;

        //call ui update just in case
        EmitSignal(SignalName.ItemsModified);
    }
    private void Update(double delta)
    {
        if (!isDead)
        {
            //attacks
            //BA
            if ((Input.IsActionPressed("attack_normal") || Input.IsActionPressed("attack_normal-alt")) && 
                BACooldown.TimeLeft == 0 && 
                !CAisCharging)
            {
                BasicAttack();
                BACooldown.Start();
            }
            //CA charge sequence control
            if ((Input.IsActionPressed("attack_charge") || Input.IsActionPressed("attack_charge-alt")) && 
                CACooldown.TimeLeft == 0)
            {
                CAisCharging = true;
                FX_Charge.Emitting = true;
                //charging to appropriate level
                CACharge += Mathf.Round((float)delta * 50);
                if (CACharge >= 100)
                {
                    chargeLevel = 4;
                    FX_Charge.AmountRatio = 1f;
                    speedmodifier = 0.2f;
                }
                else if (CACharge >= 80)
                {
                    chargeLevel = 3;
                    FX_Charge.AmountRatio = 0.8f;
                    speedmodifier = 0.4f;
                }
                else if (CACharge >= 60)
                {
                    chargeLevel = 2;
                    FX_Charge.AmountRatio = 0.6f;
                    speedmodifier = 0.6f;
                }
                else if (CACharge >= 40)
                {
                    chargeLevel = 1;
                    FX_Charge.AmountRatio = 0.4f;
                    speedmodifier = 0.8f;
                }
            }
            //shoot CA
            if (Input.IsActionJustReleased("attack_charge") || Input.IsActionJustReleased("attack_charge-alt"))
            {
                CAisCharging = false;
                if (CACharge > 40)
                {
                    ChargeAttack(chargeLevel);
                    CACooldown.Start();
                }
                CACharge = 0;
                FX_Charge.Emitting = false;
                FX_Charge.AmountRatio = 0.2f;
                speedmodifier = 1;
                dashed = false;
                dashVector = Vector2.Zero;
            }
            //oracle
            if ((Input.IsActionJustPressed("spell_oracle") || Input.IsActionJustPressed("spell_oracle-alt")) && 
                SOCooldown.TimeLeft == 0)
            {
                SpellOracle();
                SOCooldown.Start();
            }
            //spell E
            if ((Input.IsActionJustPressed("spell_slot1") || Input.IsActionJustPressed("spell_slot1-alt")) && 
                SECooldown.TimeLeft == 0 &&     //cooldown is good
                spellETimer.TimeLeft == 0)      //spell isn't currently active
            {
                SpellE();
            }
            if ((Input.IsActionJustPressed("spell_slot2") || Input.IsActionJustPressed("spell_slot2-alt")) &&
                SQCooldown.TimeLeft == 0 &&     //cooldown is good
                spellQTimer.TimeLeft == 0)      //spell isn't currently active
            {
                SpellQ();
            }
            //spell effects
            if (rapidFire)
            {
                BACooldown.WaitTime = BACooldownStatic/4;
                BADispersion = 1;
            }
            else
            {
                BACooldown.WaitTime = BACooldownStatic;
                BADispersion = 0;
            }
            //shield actual effect added in Hit() function
            if (shielded)
            {
                //add visuals for it here
            }else
            {
                //remove visuals
            }


            //hit knockback management
            if (hurtTimer.TimeLeft > 0.5)
            {
                Velocity = new Vector2(
                    Velocity.X > 0 ? Mathf.Max(0,Velocity.X-(float)delta*700) 
                                   : Mathf.Min(0,Velocity.X+(float)delta*700),
                    Velocity.Y
                );
                Fall(delta);
            }
            else
            {
                Movement(delta);
            }
            Animate();
            Cooldowns();
            MoveAndSlide();

        }
        else
        {
            if(Sprite.Animation != "die")
                Sprite.Play("die");
        }
    }

    //getters/setters
    public bool GetIsDead() { return isDead; }
    public float GetMaxHP() { return MaxHP; }
    public float GetMaxMP() { return MaxMP; }
    public float GetCurrentHP() { return currentHP; }
    //needed because exiting right after checkpoint doesn't save the recharging hp (caps at maxhp so it doesn't exceed it)
    public float GetPotentialHP() { return Mathf.Min(currentHP + HPRechargeAmount, MaxHP); }
    public float GetCurrentMP() { return currentMP; }
    public float GetAttackDamage() { return damage; }
    public List<int> GetEquips()
    {
        List<int> equips = new List<int>();
        equips.Add((int)spellItemE);
        equips.Add((int)spellItemQ);
        return equips;
    }

    //deltaloop
    public override void _PhysicsProcess(double delta)
    {

        if (Globals.playerControl)
        {
            Update(delta);
        }
    }
}
