using Godot;
using System;
using System.Collections.Generic;

public partial class Ui : Control
{
    //components
    //HP & MP
    Sprite2D HPBar;
    Sprite2D MPBar;
    Label hpNum;

    AnimatedSprite2D dashIcon;
    Sprite2D dashCooldownBar;
    AnimatedSprite2D CAIcon;
    Sprite2D CACooldownBar;
    AnimatedSprite2D OracleIcon;
    Sprite2D OracleCooldownBar;
    AnimatedSprite2D SpellEIcon;
    Sprite2D SpellECooldownBar; 
    AnimatedSprite2D SpellQIcon;
    Sprite2D SpellQCooldownBar;

    //equipped items
    Enums.itemType spellE = Enums.itemType.empty;
    Enums.itemType spellQ = Enums.itemType.empty;
    //localized player for ease of use
    Player player;
    public override void _Ready()
    {
        HPBar = GetNode("HPBar/HPBarMask") as Sprite2D;
        MPBar = GetNode("MPBar/MPBarMask") as Sprite2D;
        hpNum = GetNode("HPBar/hpnum") as Label;


        //cooldown components
        dashIcon = GetNode("Icons/Dash/Icon") as AnimatedSprite2D;
        dashCooldownBar = GetNode("Icons/Dash/CooldownBar") as Sprite2D;
        CAIcon = GetNode("Icons/ChargeAttack/Icon") as AnimatedSprite2D;
        CACooldownBar = GetNode("Icons/ChargeAttack/CooldownBar") as Sprite2D;
        OracleIcon = GetNode("Icons/Oracle/Icon") as AnimatedSprite2D;
        OracleCooldownBar = GetNode("Icons/Oracle/CooldownBar") as Sprite2D;
        SpellEIcon = GetNode("Icons/SpellE/Icon") as AnimatedSprite2D;
        SpellECooldownBar = GetNode("Icons/SpellE/CooldownBar") as Sprite2D;
        SpellQIcon = GetNode("Icons/SpellQ/Icon") as AnimatedSprite2D;
        SpellQCooldownBar = GetNode("Icons/SpellQ/CooldownBar") as Sprite2D;

        CACooldownBar.Hide();
        dashCooldownBar.Hide();
        OracleCooldownBar.Hide();
        SpellECooldownBar.Hide();
        SpellQCooldownBar.Hide();
    }

    private void Player_Dashed(float cooldown)
    {
        dashCooldownBar.Show();
        dashCooldownBar.Scale = new Vector2(0, dashCooldownBar.Scale.Y);
        var dashCooldown = GetTree().CreateTween();
        dashCooldown.Finished += DashCooldown_Finished;
        dashCooldown.TweenProperty(dashCooldownBar, "scale", new Vector2(32.0f, dashCooldownBar.Scale.Y), cooldown);
        dashIcon.Play("Cooldown");
    }
    private void DashCooldown_Finished()
    {
        dashCooldownBar.Hide();
        dashIcon.Play("Ready");
    }
    private void Player_ChargeAttacked(float cooldown)
    {
        CACooldownBar.Show();
        CACooldownBar.Scale = new Vector2(0, CACooldownBar.Scale.Y);
        var CACooldown = GetTree().CreateTween();
        CACooldown.Finished += CACooldown_Finished;
        CACooldown.TweenProperty(CACooldownBar, "scale", new Vector2(32.0f, CACooldownBar.Scale.Y), cooldown);
        CAIcon.Play("Cooldown");
    }
    private void CACooldown_Finished()
    {
        CACooldownBar.Hide();
        CAIcon.Play("Ready");
    }
    private void Player_OracleCast(float cooldown)
    {
        OracleCooldownBar.Show();
        OracleCooldownBar.Scale = new Vector2(0, OracleCooldownBar.Scale.Y);
        var oracleCooldown = GetTree().CreateTween();
        oracleCooldown.Finished += OracleCooldown_Finished;
        oracleCooldown.TweenProperty(OracleCooldownBar, "scale", new Vector2(32.0f, dashCooldownBar.Scale.Y), cooldown);
        OracleIcon.Play("Cooldown");
    }

    private void OracleCooldown_Finished()
    {
        OracleCooldownBar.Hide();
        OracleIcon.Play("Ready");
    }

    private void Player_RapidFireCast(float activeTime)
    {
        if (spellE == Enums.itemType.necklace)
            SpellEIcon.Play(spellE.ToString() + "_Active");
        else if (spellQ == Enums.itemType.necklace)
            SpellQIcon.Play(spellQ.ToString() + "_Active");
    }
    private void Player_SpellEOver(float cooldown)
    {
        if (spellE != Enums.itemType.empty)
        {
            SpellECooldownBar.Show();
            SpellECooldownBar.Scale = new Vector2(0, SpellECooldownBar.Scale.Y);
            var cooldownTween = GetTree().CreateTween();
            cooldownTween.Finished += SpellECooldown_Finished;
            cooldownTween.TweenProperty(SpellECooldownBar, "scale", new Vector2(32.0f, SpellECooldownBar.Scale.Y), cooldown);
            SpellEIcon.Play(spellE.ToString()+"_Cooldown");
        }
    }
    private void SpellECooldown_Finished()
    {
        SpellECooldownBar.Hide();
        SpellEIcon.Play(spellE.ToString()+"_Ready");
    }
    private void Player_ShieldCast(float activeTime)
    {
        if (spellE == Enums.itemType.shield)
            SpellEIcon.Play(spellE.ToString() + "_Active");
        else if(spellQ == Enums.itemType.shield) 
            SpellQIcon.Play(spellQ.ToString() + "_Active");
    }
    private void Player_SpellQOver(float cooldown)
    {
        if (spellQ != Enums.itemType.empty)
        {
            SpellQCooldownBar.Show();
            SpellQCooldownBar.Scale = new Vector2(0, SpellQCooldownBar.Scale.Y);
            var cooldownTween = GetTree().CreateTween();
            cooldownTween.Finished += SpellQCooldown_Finished;
            cooldownTween.TweenProperty(SpellQCooldownBar, "scale", new Vector2(32.0f, SpellQCooldownBar.Scale.Y), cooldown);
            SpellQIcon.Play(spellQ.ToString()+"_Cooldown");
        }
    }
    private void SpellQCooldown_Finished()
    {
        SpellQCooldownBar.Hide();
        SpellQIcon.Play(spellQ.ToString()+"_Ready");
    }

    private void UpdateItems(List<int> items)
    {
        spellE = (Enums.itemType)items[0];
        switch (spellE)
        {
            case Enums.itemType.empty:
                SpellEIcon.Hide();
                SpellECooldownBar.Hide();
                break;
            case Enums.itemType.necklace:
                SpellEIcon.Play(spellE.ToString() + "_Ready");
                SpellEIcon.Show();
                break;
            case Enums.itemType.shield:
                SpellEIcon.Play(spellE.ToString() + "_Ready");
                SpellEIcon.Show();
                break;
            default:
                break;
        }
        spellQ = (Enums.itemType)items[1];
        switch (spellQ)
        {
            case Enums.itemType.empty:
                SpellQIcon.Hide();
                SpellQCooldownBar.Hide();
                break;
            case Enums.itemType.necklace:
                SpellQIcon.Play(spellQ.ToString() + "_Ready");
                SpellQIcon.Show();
                break;
            case Enums.itemType.shield:
                SpellQIcon.Play(spellQ.ToString() + "_Ready");
                SpellQIcon.Show();
                break;
            default:
                break;
        }
    }

    private void UpdateStats()
    {
        hpNum.Text = Mathf.Round(player.GetCurrentHP()) + "/" + Mathf.Round(player.GetMaxHP());

        float HPRatio = Mathf.Round(Globals.player.GetCurrentHP()) / Globals.player.GetMaxHP();
        float MPRatio = Mathf.Round(Globals.player.GetCurrentMP()) / Globals.player.GetMaxMP();
        if (HPBar.Texture is PlaceholderTexture2D HPtexture)
            HPtexture.Size = new Vector2(HPRatio * 200, HPtexture.Size.Y);
        if(MPBar.Texture is PlaceholderTexture2D MPtexture)
            MPtexture.Size = new Vector2(MPRatio * 200, MPtexture.Size.Y);
    }

    public override void _Process(double delta)
    {
        if(Globals.player != null && player is null)
        {
            player = Globals.player;
            player.Dashed += Player_Dashed;
            player.ChargeAttacked += Player_ChargeAttacked;
            player.OracleCast += Player_OracleCast;
            player.RapidFireCast += Player_RapidFireCast;
            player.ShieldCast += Player_ShieldCast;
            player.SpellEOver += Player_SpellEOver;
            player.SpellQOver += Player_SpellQOver;
            player.ItemsModified += Player_ItemsModified;
            UpdateItems(player.GetEquips());
        }

        UpdateStats();
    }



    private void Player_ItemsModified() { UpdateItems(player.GetEquips()); }
}
