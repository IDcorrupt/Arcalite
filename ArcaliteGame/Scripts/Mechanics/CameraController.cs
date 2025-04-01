using Godot;
using System;
using System.Net.Mail;

public partial class CameraController : Node2D
{
    //debug
    private bool freecamToggle = false;
    //debug
    
    
    [Signal] public delegate void CameraMovedEventHandler(string dir);

    private Camera2D camera;
    private StaticBody2D solidBorder;
    private Area2D enemyTrigger;
    private CollisionShape2D hitbox;


    Timer cooldownTimer;
    bool cooling = true;
    public override void _Ready()
    {
        camera = GetNode<Camera2D>("Camera");
        solidBorder = GetNode("Camera/edgeCollisions") as StaticBody2D;
        hitbox = GetNode("Camera/Hitbox/CollisionShape2D") as CollisionShape2D;
        enemyTrigger = GetNode<Area2D>("Camera/EnemyTrigger");

        cooldownTimer = GetNode<Timer>("CooldownTimer");
    }


    public void RespawnMove()
    {
        camera.PositionSmoothingEnabled = false;
        hitbox.Disabled = true;
        LockPlayer(false);
        cooldownTimer.Start();
    }

    public void LockPlayer(bool value)
    {
        solidBorder.SetCollisionLayerValue(3, value);
    }

    //camera movement
    public void MoveCamera(string direction)
    {

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
        EmitSignal(SignalName.CameraMoved, direction);
    }

    public void CooldownTimerTimeout()
    {
        cooling = false;
        hitbox.Disabled = false;
        camera.PositionSmoothingEnabled = true;
    }
    public override void _Process(double delta)
    {
        //room correction

        if (camera.GlobalPosition.X - Globals.player.GlobalPosition.X > 330)
            MoveCamera("left");
        else if (camera.GlobalPosition.X - Globals.player.GlobalPosition.X < -330)
            MoveCamera("right");

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
