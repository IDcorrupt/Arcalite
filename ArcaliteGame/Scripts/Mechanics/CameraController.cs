using Godot;
using System;

public partial class CameraController : Node2D
{
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


    public void MoveCamera(string direction)
    {
        if (!cooling)
        {
            cooldownTimer.Start();
            cooling = true; 
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
    }

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
    }
    public override void _Process(double delta)
    {

    }
}
