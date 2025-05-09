using Godot;
using System;

// Composite Support: This obstacle is part of a node tree (scene) with visual and physical components.
public partial class Obstacle : StaticBody3D
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
	
	// Lifecycle: Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Currently empty - logic could be added for animation, collision, etc.
	}

	// Updates position every frame to move the obstacle forward (toward the player)
	public override void _Process(double delta)
	{
		Position = new Vector3(Position.X, Position.Y, Position.Z + Speed * (float)delta);
	}
}
