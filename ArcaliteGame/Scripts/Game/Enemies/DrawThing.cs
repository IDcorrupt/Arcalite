using Godot;
using System;

public partial class DrawThing : Node2D
{
    [Export] RayCast2D Raycast;
    Vector2 to;
    public override void _Draw()
    {
        base._Draw();
        if (Raycast == null)
        {
            return;
        }
        Vector2 from = Raycast.GlobalPosition;
        to = Globals.player.GlobalPosition;

        Color lineColor = Raycast.GetCollider() is Player ? Colors.Green : Colors.Red;
        DrawLine(from - GlobalPosition, to - GlobalPosition, lineColor, 2.0f);

    }

    public override void _Process(double delta)
    {
        GD.Print("line endpoint: " + to);
        QueueRedraw(); // Redraw every frame
    }
}

