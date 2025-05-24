// using Godot;
// using System;

// public partial class Pickupable : Area3D
// {
// 	public enum PickupableType
// 	{
// 		Coin,
// 	}

// 	[Export]
// 	public int ScoreValue = 10;

// 	[Export]
// 	public PickupableType CurrentType = PickupableType.Coin;

// 	public float Speed = 10;
// 	// Called when the node enters the scene tree for the first time.
// 	public override void _Ready()
// 	{
// 	}

// 	// Called every frame. 'delta' is the elapsed time since the previous frame.
// 	public override void _Process(double delta)
// 	{
// 		Position = new Vector3(Position.X, Position.Y, Position.Z + Speed * (float)delta);
// 	}

// 	public void _on_area_entered(Area3D area) {
// 		QueueFree();
// 	}
// }


// using Godot;
// using System;

// public partial class Pickupable : Area3D
// {
//     public enum PickupableType
//     {
//         Coin
//     }

//     [Export]
//     public int ScoreValue = 10;

//     [Export]
//     public PickupableType CurrentType = PickupableType.Coin;

//     [Export]
//     public float Speed = 10.0f;

//     public override void _Process(double delta)
//     {
//         Position = new Vector3(Position.X, Position.Y, Position.Z + Speed * (float)delta);
//     }

//     // Remove this! Let the Player handle pickup logic
//     // public void _on_area_entered(Area3D area) {
//     //     QueueFree();
//     // }
// }


using Godot;
using System;

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
