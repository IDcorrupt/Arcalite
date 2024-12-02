using Godot;
using System;

public partial class CameraSwitcherHorizontal : Node2D
{
    Area2D leftTrigger;
    Area2D rightTrigger;
    Area2D fullArea;
    string moveDir = string.Empty;
    string cameraMoved = string.Empty;
    bool leftExit = false;
    bool rightExit = false;
    Map Parent;

    public override void _Ready()
    {
        Parent = GetParent() as Map;
        leftTrigger = GetNode<Area2D>("LeftTrigger");
        rightTrigger = GetNode<Area2D>("RightTrigger");
        fullArea = GetNode<Area2D>("FullArea");
    }



    public void LeftTriggerEntered(Node2D node)
    {
        if (node is Player && moveDir == string.Empty)
        {
            moveDir = "right";
        }
        else if (node is Player && moveDir == "left")
        {
            Parent.CameraController("left");
            cameraMoved = "left";
        }
    }
    public void RightTriggerEntered(Node2D node)
    {
        if (node is Player && moveDir == string.Empty)
        {
            moveDir = "left";
        }
        else if (node is Player && moveDir == "right")
        {
            Parent.CameraController("right");
            cameraMoved = "right";
        }
    }
    public void LeftTriggerExited(Node2D node)
    {
        leftExit = true;
        if (rightExit)
        {
            moveDir = string.Empty;
            leftExit = false;
            rightExit = false;
        }


    }
    public void RightTriggerExited(Node2D node)
    {
        rightExit = true;
        if (leftExit)
        {
            moveDir = string.Empty;
            leftExit = false;
            rightExit = false;
        }
    }

    public void FullAreaExited(Node2D node)
    {
        if ((node.GlobalPosition - GlobalPosition).Normalized().X >0 && cameraMoved == "left")
        {
            cameraMoved = string.Empty;
            Parent.CameraController("right");
        }else if((node.GlobalPosition - GlobalPosition).Normalized().X < 0 && cameraMoved == "right"){
            cameraMoved = string.Empty;
            Parent.CameraController("left");

        }
    }
   
    public override void _Process(double delta)
    {

    }
}