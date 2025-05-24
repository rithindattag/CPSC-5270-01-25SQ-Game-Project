using Godot;
using System;

// Represents a collectible item (e.g., coin) that moves forward and triggers scoring.
// Implements IGameElement for composite lifecycle handling.
public partial class Pickupable : Area3D, IGameElement
{
    public enum PickupableType
    {
        Coin
    }

    [Export]
    public int ScoreValue = 10;

    [Export]
    public PickupableType CurrentType = PickupableType.Coin;

    [Export]
    public float Speed = 10.0f;

    public override void _Process(double delta)
    {
        // Move the item forward along the Z-axis
        Position = new Vector3(Position.X, Position.Y, Position.Z + Speed * (float)delta);
    }

    public void OnSpawn()
    {
        GD.Print("Pickupable spawned.");
    }

    public void OnDestroy()
    {
        GD.Print("Pickupable destroyed.");
        QueueFree();
    }
}
