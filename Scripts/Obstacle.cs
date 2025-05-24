using Godot;
using System;

// Obstacle node that moves forward and implements composite lifecycle events
public partial class Obstacle : StaticBody3D, IGameElement
{
    public enum ObstacleType
    {
        Standard,
        Lower,
        Upper
    }

    [Export]
    public ObstacleType CurrentType = ObstacleType.Standard;

    public float Speed = 10;

    public override void _Process(double delta)
    {
        // Move the obstacle forward along the Z-axis
        Position = new Vector3(Position.X, Position.Y, Position.Z + Speed * (float)delta);
    }

    public void OnSpawn()
    {
        GD.Print("Obstacle spawned.");
    }

    public void OnDestroy()
    {
        GD.Print("Obstacle destroyed.");
        QueueFree();
    }
}
