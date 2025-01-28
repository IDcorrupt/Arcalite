using Godot;
using System;

public partial class CameraSwitcherVertical : Node2D
{
    Area2D topTrigger;
    Area2D bottomTrigger;
    Area2D fullArea;
    string moveDir = string.Empty;
    string cameraMoved = string.Empty;
    bool topExit = false;
    bool bottomExit = false;
    Map Parent;

    Sprite2D botShow;
    Sprite2D topShow;

    public override void _Ready()
    {
        Parent = GetParent() as Map;
        topTrigger = GetNode<Area2D>("TopTrigger");
        bottomTrigger = GetNode<Area2D>("BottomTrigger");
        fullArea = GetNode<Area2D>("FullArea");

        botShow = GetNode<Sprite2D>("BottomTrigger/display");
        topShow = GetNode<Sprite2D>("TopTrigger/display");
        topShow.Visible = false;
        botShow.Visible = false;
    }



    public void TopTriggerEntered(Node2D node)
    {
        topShow.Visible = true;
        if (node is Player && moveDir == string.Empty)
        {
            GD.Print("top entered first, moving down");
            moveDir = "bottom";
        }
        else if (node is Player && moveDir == "top")
        {
            GD.Print("top entered second, initiating camera move up");
            Parent.CameraController("top");
            cameraMoved = "top";
        }
    }
    public void BottomTriggerEntered(Node2D node)
    {
        botShow.Visible = true;
        if (node is Player && moveDir == string.Empty)
        {
            GD.Print("bot entered first, moving up");
            moveDir = "top";
        }
        else if (node is Player && moveDir == "bottom")
        {
            GD.Print("bot entered second, initiating camera move down");
            Parent.CameraController("bottom");
            cameraMoved = "bottom";
        }
    }
    public void TopTriggerExited(Node2D node)
    {
        GD.Print("top exited");
        topShow.Visible = false;
        topExit = true;
        if (bottomExit)
        {
            GD.Print("top exited second, resetting switcher");
            moveDir = string.Empty;
            topExit = false;
            bottomExit = false;
        }


    }
    public void BottomTriggerExited(Node2D node)
    {
        GD.Print("bot exited");
        botShow.Visible = false;
        bottomExit = true;
        if (topExit)
        {
            GD.Print("bot exited second, resetting switcher");
            moveDir = string.Empty;
            topExit = false;
            bottomExit = false;
        }
    }

    public void FullAreaExited(Node2D node)
    {
        if ((node.GlobalPosition - GlobalPosition).Normalized().Y > 0 && cameraMoved == "top")
        {
            cameraMoved = string.Empty;
            Parent.CameraController("bottom");
        }
        else if ((node.GlobalPosition - GlobalPosition).Normalized().Y < 0 && cameraMoved == "bottom")
        {
            cameraMoved = string.Empty;
            Parent.CameraController("top");

        }
    }

}
