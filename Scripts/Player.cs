// using Godot;
// using System;

// public partial class Player : CharacterBody3D
// {
//   [Signal]
// 	public delegate void ScoreUpdatedEventHandler(int score);

// 	public const float Speed = 5.0f;
// 	public const float JumpVelocity = 4.5f;

//     // Strategy Pattern: Defines different positions the player can move to
//     enum Lane
//     {
//         Left = -1,
//         Center = 0,
//         Right = 1
//     }

//     private Lane targetLane = Lane.Center;
//     private Lane currentLane = Lane.Center;

//     private int laneWidth = 2;
//     private double laneChangeSpeed = 5;

//     private bool dead = true;

//   private float _currentSlidingDuration;

//   [Export]
//   public float SlidingDuration = .2f;

//   public override void _Ready()
//   {
//     base._Ready();   
//     _currentSlidingDuration = SlidingDuration;
//   }

// 	public override void _PhysicsProcess(double delta)
// 	{
//     if(!dead) {
// 		Vector3 velocity = Velocity;

// 		// Physics Engine Tool: Gravity
// 		if (!IsOnFloor())
// 		{
// 			velocity += GetGravity() * (float)delta;
// 		}

// 		// Handle Jump
// 		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
// 		{
// 			velocity.Y = JumpVelocity;
// 		}

//     // Strategy Pattern: Smoothly move player toward the target lane.
//     float targetX = (int)targetLane * laneWidth;
//     Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);
        
//     // Input changes lane strategy
//     if(Input.IsActionJustPressed("ui_left") && targetLane > Lane.Left) {
//       targetLane -= 1;
//       }

//     if(Input.IsActionJustPressed("ui_right") && targetLane < Lane.Right) {
//       targetLane += 1;
//       }

//     // State Pattern: Slide mechanic using a timer to manage crouch state
//     if(Input.IsActionJustPressed("ui_down") && _currentSlidingDuration <= 0) {
//       GetNode<CollisionShape3D>("StandingCollision").Disabled = true;
//       _currentSlidingDuration = SlidingDuration;
//       }

//     _currentSlidingDuration -= (float)delta;
//     if(_currentSlidingDuration <= 0) {
//       GetNode<CollisionShape3D>("StandingCollision").Disabled = false;
//       }

// 		Velocity = velocity;
// 		MoveAndSlide();
//     }
// 	}

//   public void _on_area_3d_area_entered(Area3D area) {
//     if(area is Pickupable pickupable) {
//       if(pickupable.CurrentType == Pickupable.PickupableType.Coin) {
//           EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
//       }
//     }
//   }

//   public void _on_game_manager_start_game() {
//     Position = new Vector3(0,0,0);
//     dead = false;
//   }

//   public void _on_game_manager_gameover(int score) {
//     dead = true;
//   }
// }


// using Godot;
// using System;

// public partial class Player : CharacterBody3D
// {
//     [Signal]
//     public delegate void ScoreUpdatedEventHandler(int score);

//     public const float Speed = 5.0f;
//     public const float JumpVelocity = 4.5f;

//     enum Lane { Left = -1, Center = 0, Right = 1 }

//     private Lane targetLane = Lane.Center;
//     private int laneWidth = 2;
//     private double laneChangeSpeed = 5;
//     private bool dead = true;

//     [Export]
//     public float SlidingDuration = 0.2f;

//     private IPlayerState _currentState;

//     public override void _Ready()
//     {
//         ChangeState(new IdleState());
//     }

//     public override void _PhysicsProcess(double delta)
//     {
//         if (!dead)
//         {
//             _currentState?.Update(this, delta);

//             Vector3 velocity = Velocity;

//             if (!IsOnFloor())
//                 velocity += GetGravity() * (float)delta;

//             if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
//                 velocity.Y = JumpVelocity;

//             // Move between lanes
//             float targetX = (int)targetLane * laneWidth;
//             Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);

//             // if (Input.IsActionJustPressed("ui_left") && targetLane > Lane.Left)
//             //     targetLane--;

//             // if (Input.IsActionJustPressed("ui_right") && targetLane < Lane.Right)
//             //     targetLane++;

//             ILaneSwitchStrategy laneStrategy = null;

// if (Input.IsActionJustPressed("ui_left"))
//     laneStrategy = new SwitchLeftStrategy();
// else if (Input.IsActionJustPressed("ui_right"))
//     laneStrategy = new SwitchRightStrategy();
// else
//     laneStrategy = new NoOpSwitchStrategy(); // Optional

// if (laneStrategy != null)
//     _targetLane = laneStrategy.GetNextLane(_targetLane);


//             // Trigger slide (enter state)
//             if (Input.IsActionJustPressed("ui_down") && _currentState is not SlideState)
//                 ChangeState(new SlideState());

//             Velocity = velocity;
//             MoveAndSlide();
//         }
//     }

//     public void ChangeState(IPlayerState newState)
//     {
//         _currentState?.Exit(this);
//         _currentState = newState;
//         _currentState?.Enter(this);
//     }

//     public void _on_area_3d_area_entered(Area3D area)
//     {
//         if (area is Pickupable pickupable)
//         {
//             if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
//             {
//                 EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
//                 pickupable.QueueFree();
//             }
//         }
//     }

//     public void _on_game_manager_start_game()
//     {
//         Position = new Vector3(0, 0, 0);
//         dead = false;
//         ChangeState(new IdleState());
//     }

//     public void _on_game_manager_gameover(int score)
//     {
//         dead = true;
//     }
// }


// using Godot;
// using System;

// public partial class Player : CharacterBody3D
// {
//     [Signal]
//     public delegate void ScoreUpdatedEventHandler(int score);

//     public const float Speed = 5.0f;
//     public const float JumpVelocity = 4.5f;

//     private enum Lane { Left = -1, Center = 0, Right = 1 }

//     private Lane targetLane = Lane.Center;
//     private int laneWidth = 2;
//     private double laneChangeSpeed = 5;
//     private bool dead = true;

//     [Export]
//     public float SlidingDuration = 0.2f;

//     private IPlayerState _currentState;

//     public override void _Ready()
//     {
//         ChangeState(new IdleState());
//     }

//     public override void _PhysicsProcess(double delta)
//     {
//         if (!dead)
//         {
//             _currentState?.Update(this, delta);

//             Vector3 velocity = Velocity;

//             if (!IsOnFloor())
//                 velocity += GetGravity() * (float)delta;

//             if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
//                 velocity.Y = JumpVelocity;

//             // STRATEGY PATTERN: Use lane switch strategy
//             ILaneSwitchStrategy laneStrategy = null;

//             if (Input.IsActionJustPressed("ui_left"))
//                 laneStrategy = new SwitchLeftStrategy();
//             else if (Input.IsActionJustPressed("ui_right"))
//                 laneStrategy = new SwitchRightStrategy();
//             else
//                 laneStrategy = new NoOpSwitchStrategy(); // Optional fallback

//             if (laneStrategy != null)
//             {
//                 int updated = laneStrategy.GetNextLane((int)targetLane);
//                 targetLane = (Lane)Mathf.Clamp(updated, (int)Lane.Left, (int)Lane.Right);
//             }

//             // Smooth movement between lanes
//             float targetX = (int)targetLane * laneWidth;
//             Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);

//             // STATE PATTERN: Enter sliding state
//             if (Input.IsActionJustPressed("ui_down") && _currentState is not SlideState)
//                 ChangeState(new SlideState());

//             Velocity = velocity;
//             MoveAndSlide();
//         }
//     }

//     public void ChangeState(IPlayerState newState)
//     {
//         _currentState?.Exit(this);
//         _currentState = newState;
//         _currentState?.Enter(this);
//     }

//     public void _on_area_3d_area_entered(Area3D area)
//     {
//         if (area is Pickupable pickupable)
//         {
//             if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
//             {
//                 EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
//                 pickupable.QueueFree();
//             }
//         }
//     }

//     public void _on_game_manager_start_game()
//     {
//         Position = new Vector3(0, 0, 0);
//         dead = false;
//         ChangeState(new IdleState());
//     }

//     public void _on_game_manager_gameover(int score)
//     {
//         dead = true;
//     }
// }



// using Godot;
// using System;

// public partial class Player : CharacterBody3D
// {
//     [Signal]
//     public delegate void ScoreUpdatedEventHandler(int score);

//     public const float Speed = 5.0f;
//     public const float JumpVelocity = 4.5f;

//     private enum Lane { Left = 0, Center = 1, Right = 2 }

//     private Lane targetLane = Lane.Center;
//     private int laneWidth = 2;
//     private double laneChangeSpeed = 5;
//     private bool dead = true;

//     [Export]
//     public float SlidingDuration = 0.2f;

//     private IPlayerState _currentState;

//     public override void _Ready()
//     {
//         ChangeState(new IdleState());
//     }

//     public override void _PhysicsProcess(double delta)
//     {
//         if (!dead)
//         {
//             _currentState?.Update(this, delta);

//             Vector3 velocity = Velocity;

//             if (!IsOnFloor())
//                 velocity += GetGravity() * (float)delta;

//             if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
//                 velocity.Y = JumpVelocity;

//             // STRATEGY PATTERN: Use lane switch strategy
//             ILaneSwitchStrategy laneStrategy = null;

//             if (Input.IsActionJustPressed("ui_left"))
//                 laneStrategy = new SwitchLeftStrategy();
//             else if (Input.IsActionJustPressed("ui_right"))
//                 laneStrategy = new SwitchRightStrategy();
//             else
//                 laneStrategy = new NoOpSwitchStrategy(); // Optional fallback

//             if (laneStrategy != null)
//             {
//                 int updated = laneStrategy.GetNextLane((int)targetLane);
//                 updated = Mathf.Clamp(updated, (int)Lane.Left, (int)Lane.Right);
//                 targetLane = (Lane)updated;
//             }

//             // Smooth movement between lanes
//             float targetX = (int)targetLane * laneWidth - laneWidth; // Adjust to center lane on X = 0
//             Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);

//             // STATE PATTERN: Enter sliding state
//             if (Input.IsActionJustPressed("ui_down") && _currentState is not SlideState)
//                 ChangeState(new SlideState());

//             Velocity = velocity;
//             MoveAndSlide();
//         }
//     }

//     public void ChangeState(IPlayerState newState)
//     {
//         _currentState?.Exit(this);
//         _currentState = newState;
//         _currentState?.Enter(this);
//     }

//     public void _on_area_3d_area_entered(Area3D area)
//     {
//         if (area is Pickupable pickupable)
//         {
//             if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
//             {
//                 EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
//                 pickupable.QueueFree();
//             }
//         }
//     }

//     public void _on_game_manager_start_game()
//     {
//         Position = new Vector3(0, 0, 0);
//         dead = false;
//         ChangeState(new IdleState());
//     }

//     public void _on_game_manager_gameover(int score)
//     {
//         dead = true;
//     }
// }


// using Godot;
// using System;

// public partial class Player : CharacterBody3D
// {
//     [Signal]
//     public delegate void ScoreUpdatedEventHandler(int score);

//     public const float Speed = 5.0f;
//     public const float JumpVelocity = 4.5f;

//     private enum Lane { Left = 0, Center = 1, Right = 2 }

//     private Lane targetLane = Lane.Center;
//     private int laneWidth = 2;
//     private double laneChangeSpeed = 5;
//     private bool dead = true;

//     [Export]
//     public float SlidingDuration = 0.2f;

//     private IPlayerState _currentState;

//     public override void _Ready()
//     {
//         ChangeState(new IdleState());
//     }

//     public override void _PhysicsProcess(double delta)
//     {
//         if (!dead)
//         {
//             _currentState?.Update(this, delta);

//             Vector3 velocity = Velocity;

//             if (!IsOnFloor())
//                 velocity += GetGravity() * (float)delta;

//             // FACADE: Handle jump input
//             if (PlayerInputFacade.IsJump() && IsOnFloor())
//                 velocity.Y = JumpVelocity;

//             // STRATEGY PATTERN: Use lane switch strategy via input facade
//             ILaneSwitchStrategy laneStrategy = null;

//             if (PlayerInputFacade.IsMoveLeft())
//                 laneStrategy = new SwitchLeftStrategy();
//             else if (PlayerInputFacade.IsMoveRight())
//                 laneStrategy = new SwitchRightStrategy();
//             else
//                 laneStrategy = new NoOpSwitchStrategy(); // fallback

//             if (laneStrategy != null)
//             {
//                 int updated = laneStrategy.GetNextLane((int)targetLane);
//                 updated = Mathf.Clamp(updated, (int)Lane.Left, (int)Lane.Right);
//                 targetLane = (Lane)updated;
//             }

//             // Smooth movement between lanes
//             float targetX = (int)targetLane * laneWidth - laneWidth;
//             Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);

//             // STATE PATTERN: Enter sliding state
//             if (PlayerInputFacade.IsSlide() && _currentState is not SlideState)
//                 ChangeState(new SlideState());

//             Velocity = velocity;
//             MoveAndSlide();
//         }
//     }

//     public void ChangeState(IPlayerState newState)
//     {
//         _currentState?.Exit(this);
//         _currentState = newState;
//         _currentState?.Enter(this);
//     }

//     public void _on_area_3d_area_entered(Area3D area)
//     {
//         if (area is Pickupable pickupable)
//         {
//             if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
//             {
//                 EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
//                 pickupable.QueueFree();
//             }
//         }
//     }

//     public void _on_game_manager_start_game()
//     {
//         Position = new Vector3(0, 0, 0);
//         dead = false;
//         ChangeState(new IdleState());
//     }

//     public void _on_game_manager_gameover(int score)
//     {
//         dead = true;
//     }
// }



// using Godot;
// using System;

// public partial class Player : CharacterBody3D
// {
//     [Signal]
//     public delegate void ScoreUpdatedEventHandler(int score);

//     public const float Speed = 5.0f;
//     public const float JumpVelocity = 4.5f;

//     public enum Lane { Left = 0, Center = 1, Right = 2 }

//     public Lane CurrentLane = Lane.Center;
//     private int laneWidth = 2;
//     private double laneChangeSpeed = 5;
//     private bool dead = true;

//     [Export]
//     public float SlidingDuration = 0.2f;

//     private IPlayerState _currentState;

//     public override void _Ready()
//     {
//         ChangeState(new IdleState());
//     }

//     public override void _PhysicsProcess(double delta)
//     {
//         if (!dead)
//         {
//             _currentState?.Update(this, delta);

//             Vector3 velocity = Velocity;

//             if (!IsOnFloor())
//                 velocity += GetGravity() * (float)delta;

//             // COMMAND: Handle Jump
//             if (PlayerInputFacade.IsJump())
//                 new JumpCommand().Execute(this);

//             // COMMAND: Handle Lane Switch using Strategy
//             ICommand laneCommand = null;
//             if (PlayerInputFacade.IsMoveLeft())
//                 laneCommand = new MoveLaneCommand(new SwitchLeftStrategy());
//             else if (PlayerInputFacade.IsMoveRight())
//                 laneCommand = new MoveLaneCommand(new SwitchRightStrategy());

//             laneCommand?.Execute(this);

//             // Smooth lane movement
//             float targetX = (int)CurrentLane * laneWidth - laneWidth;
//             Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);

//             // COMMAND: Slide
//             if (PlayerInputFacade.IsSlide() && _currentState is not SlideState)
//                 new SlideCommand().Execute(this);

//             Velocity = velocity;
//             MoveAndSlide();
//         }
//     }

//     public void ChangeState(IPlayerState newState)
//     {
//         _currentState?.Exit(this);
//         _currentState = newState;
//         _currentState?.Enter(this);
//     }

//     public void _on_area_3d_area_entered(Area3D area)
//     {
//         if (area is Pickupable pickupable)
//         {
//             if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
//             {
//                 EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
//                 pickupable.QueueFree();
//             }
//         }
//     }

//     public void _on_game_manager_start_game()
//     {
//         Position = new Vector3(0, 0, 0);
//         dead = false;
//         ChangeState(new IdleState());
//     }

//     public void _on_game_manager_gameover(int score)
//     {
//         dead = true;
//     }
// }




using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Signal]
    public delegate void ScoreUpdatedEventHandler(int score);

    public const float Speed = 5.0f;
    public const float JumpVelocity = 5.0f; // Boosted for visibility

    public enum Lane { Left = 0, Center = 1, Right = 2 }

    public Lane CurrentLane = Lane.Center;
    private int laneWidth = 2;
    private double laneChangeSpeed = 5;
    private bool dead = true;

    [Export]
    public float SlidingDuration = 0.2f;

    private IPlayerState _currentState;

    public override void _Ready()
    {
        ChangeState(new IdleState());
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!dead)
    {
        _currentState?.Update(this, delta);

        Vector3 velocity = Velocity;

        // ✅ Apply gravity first
        if (!IsOnFloor())
            velocity += GetGravity() * (float)delta;

        // ✅ COMMAND: Jump (pass velocity by ref)
        if (PlayerInputFacade.IsJump())
            new JumpCommand().Execute(this, ref velocity);

        // ✅ COMMAND: Lane Switch using Strategy
        ICommand laneCommand = null;
        if (PlayerInputFacade.IsMoveLeft())
            laneCommand = new MoveLaneCommand(new SwitchLeftStrategy());
        else if (PlayerInputFacade.IsMoveRight())
            laneCommand = new MoveLaneCommand(new SwitchRightStrategy());

        laneCommand?.Execute(this, ref velocity); // Optional if needed

        // ✅ Smooth movement between lanes
        float targetX = (int)CurrentLane * laneWidth - laneWidth;
        Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);

        // ✅ COMMAND: Slide
        if (PlayerInputFacade.IsSlide() && _currentState is not SlideState)
            new SlideCommand().Execute(this, ref velocity);

        // ✅ Apply updated velocity and move
        Velocity = velocity;
        MoveAndSlide();
    }
    }

    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    public void _on_area_3d_area_entered(Area3D area)
    {
        if (area is Pickupable pickupable)
        {
            if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
            {
                EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
                pickupable.QueueFree();
            }
        }
    }

    public void _on_game_manager_start_game()
    {
        Position = new Vector3(0, 0, 0);
        dead = false;
        ChangeState(new IdleState());
    }

    public void _on_game_manager_gameover(int score)
    {
        dead = true;
    }
}
