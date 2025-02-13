using Godot;
using System;
using System.Net.Mail;

public partial class CameraController : Node2D
{
    //debug
    private bool freecamToggle = false;
    //debug


    private Camera2D camera;
    private Area2D topTrigger;
    private Area2D botTrigger;
    private Area2D leftTrigger;
    private Area2D rightTrigger;

    private Area2D enemyTrigger;

    Timer cooldownTimer;
    bool cooling = true;
    public override void _Ready()
    {
        camera = GetNode<Camera2D>("Camera");
        topTrigger = GetNode<Area2D>("Camera/CamTriggers/TopTrigger");
        botTrigger = GetNode<Area2D>("Camera/CamTriggers/BotTrigger");
        leftTrigger = GetNode<Area2D>("Camera/CamTriggers/LeftTrigger");
        rightTrigger = GetNode<Area2D>("Camera/CamTriggers/RightTrigger");

        enemyTrigger = GetNode<Area2D>("Camera/EnemyTrigger");

        cooldownTimer = GetNode<Timer>("CooldownTimer");
    }


    public void RespawnMove()
    {
        camera.PositionSmoothingEnabled = false;
        cooldownTimer.Start();
    }


    //camera movement
    public void MoveCamera(string direction)
    {

        GD.Print("moving: " + direction);
        switch (direction)
        {
            case "top":
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y - 360);
                break;
            case "right":
                camera.Position = new Vector2(camera.Position.X + 640, camera.Position.Y);
                break;
            case "bot":
                camera.Position = new Vector2(camera.Position.X, camera.Position.Y + 360);
                break;
            case "left":
                camera.Position = new Vector2(camera.Position.X - 640, camera.Position.Y);
                break;
            default:
                break;
        }
        
    }


    //MIGHT BE DEPRECATED CUZ OF NEW THING
    public void LeftTriggerEntered(Node2D body)
    {
        if(body is Player)
        {
            MoveCamera("left");
        }
    }
    public void LeftTriggerExited(Node2D body)
    {
        if(body is Player)
        {
            //right room check TBD
        }
    }
    public void RightTriggerEntered(Node2D body)
    {
        if (body is Player)
        {
            MoveCamera("right");
        }
    }
    public void RightTriggerExited(Node2D body)
    {
        if (body is Player)
        {
            //right room check TBD
        }
    }
    public void TopTriggerEntered(Node2D body)
    {
        if (body is Player)
        {
            MoveCamera("top");
        }
    }
    public void TopTriggerExited(Node2D body)
    {
        if (body is Player)
        {
            //right room check TBD
        }
    }
    public void BotTriggerEntered(Node2D body)
    {
        if (body is Player)
        {
            MoveCamera("bot");
        }
    }
    public void BotTriggerExited(Node2D body)
    {
        if (body is Player)
        {
            //right room check TBD
        }
    }

    public void CooldownTimerTimeout()
    {
        cooling = false;
        camera.PositionSmoothingEnabled = true;
    }
    public override void _Process(double delta)
    {
        //room correction / new room switcher

        if (camera.GlobalPosition.X - Globals.player.GlobalPosition.X > 330)
            MoveCamera("left");
        else if (camera.GlobalPosition.X - Globals.player.GlobalPosition.X < -330)
            MoveCamera("right");

        //VECTICAL CURRENTLY REMOVED
        if (camera.GlobalPosition.Y - Globals.player.GlobalPosition.Y > 190)
            MoveCamera("top");
        else if (camera.GlobalPosition.Y - Globals.player.GlobalPosition.Y < -190)
            MoveCamera("bot");

        //debug freecam
        if (Input.IsActionJustPressed("freecam_toggle"))
        {
            freecamToggle = !freecamToggle;
            GD.Print("Freecam toggled: " + freecamToggle);
        }
        if (freecamToggle)
        {
            if (Input.IsActionPressed("freecam_up"))
                camera.Offset = new Vector2(camera.Offset.X, camera.Offset.Y - 10);
            if (Input.IsActionPressed("freecam_down"))
                camera.Offset = new Vector2(camera.Offset.X, camera.Offset.Y + 10);
            if (Input.IsActionPressed("freecam_left"))
                camera.Offset = new Vector2(camera.Offset.X - 10, camera.Offset.Y);
            if (Input.IsActionPressed("freecam_right"))
                camera.Offset = new Vector2(camera.Offset.X + 10, camera.Offset.Y);
        }
        if (Input.IsActionJustPressed("freecam_reset"))
            camera.Offset = new Vector2(0, 0);

    }
}
