using Godot;
using System;

public partial class Ui : Control
{
    RichTextLabel HPnum;
    RichTextLabel MPnum;
    AnimatedSprite2D dashIcon;
    Sprite2D dashCooldownBar;
    AnimatedSprite2D CAIcon;
    Sprite2D CACooldownBar;

    //localized player for ease of use
    Player player;
    public override void _Ready()
    {
        HPnum = GetNode("HPNumDisplay") as RichTextLabel;
        MPnum = GetNode("MPNumDisplay") as RichTextLabel;
        dashIcon = GetNode("Icons/DashIcon") as AnimatedSprite2D;
        dashCooldownBar = GetNode("Icons/DashCooldownBar") as Sprite2D;
        CAIcon = GetNode("Icons/CAIcon") as AnimatedSprite2D;
        CACooldownBar = GetNode("Icons/CACooldownBar") as Sprite2D;
        CACooldownBar.Hide();
        dashCooldownBar.Hide();
    }

    public void onhelppressed()
    {
        SaveLoadHandler.Load();
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
        CACooldown.Finished += CACooldown_Finished; ;
        CACooldown.TweenProperty(CACooldownBar, "scale", new Vector2(32.0f, CACooldownBar.Scale.Y), cooldown);
        CAIcon.Play("Cooldown");
    }
    private void CACooldown_Finished()
    {
        CACooldownBar.Hide();
        CAIcon.Play("Ready");
    }

    public override void _Process(double delta)
    {
        if(Globals.player != null && player is null)
        {
            player = Globals.player;
            player.Dashed += Player_Dashed;
            player.ChargeAttacked += Player_ChargeAttacked;
        }
        HPnum.Text = "HP: " + Mathf.Round(Globals.player.GetHP()); 
        MPnum.Text = "MP: " + Mathf.Round(Globals.player.GetMP()); 
    }


}
