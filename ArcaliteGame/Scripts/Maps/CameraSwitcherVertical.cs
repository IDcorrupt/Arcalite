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

    public override void _Ready()
    {
        Parent = GetParent() as Map;
        topTrigger = GetNode<Area2D>("TopTrigger");
        bottomTrigger = GetNode<Area2D>("BottomTrigger");
        fullArea = GetNode<Area2D>("FullArea");
    }



    public void TopTriggerEntered(Node2D node)
    {
        if (node is Player && moveDir == string.Empty)
        {
            moveDir = "bottom";
        }
        else if (node is Player && moveDir == "top")
        {
            Parent.CameraController("top");
            cameraMoved = "top";
        }
    }
    public void BottomTriggerEntered(Node2D node)
    {
        if (node is Player && moveDir == string.Empty)
        {
            moveDir = "top";
        }
        else if (node is Player && moveDir == "bottom")
        {
            Parent.CameraController("bottom");
            cameraMoved = "bottom";
        }
    }
    public void TopTriggerExited(Node2D node)
    {
        topExit = true;
        if (bottomExit)
        {
            moveDir = string.Empty;
            topExit = false;
            bottomExit = false;
        }


    }
    public void BottomTriggerExited(Node2D node)
    {
        bottomExit = true;
        if (topExit)
        {
            moveDir = string.Empty;
            topExit = false;
            bottomExit = false;
        }
    }

    public void FullAreaExited(Node2D node)
    {
        if ((node.GlobalPosition - GlobalPosition).Normalized().X > 0 && cameraMoved == "top")
        {
            cameraMoved = string.Empty;
            Parent.CameraController("bottom");
        }
        else if ((node.GlobalPosition - GlobalPosition).Normalized().X < 0 && cameraMoved == "bottom")
        {
            cameraMoved = string.Empty;
            Parent.CameraController("top");

        }
    }

}
