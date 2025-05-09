using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

    // Strategy Pattern: Defines different positions the player can move to
    enum Lane
    {
        Left = -1,
        Center = 0,
        Right = 1
    }

    Lane targetLane = Lane.Center;
    Lane currentLane = Lane.Center;

    int laneWidth = 2;
    double laneChangeSpeed = 5;

  private float _currentSlidingDuration;

  [Export]
  public float SlidingDuration = .2f;

  public override void _Ready()
  {
    base._Ready();   
    _currentSlidingDuration = SlidingDuration;
  }

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Physics Engine Tool: Gravity
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

    // Strategy Pattern: Smoothly move player toward the target lane.
    float targetX = (int)targetLane * laneWidth;
    Position = new Vector3(Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)), Position.Y, Position.Z);
        
    // Input changes lane strategy
    if(Input.IsActionJustPressed("ui_left") && targetLane > Lane.Left) {
      targetLane -= 1;
      }

    if(Input.IsActionJustPressed("ui_right") && targetLane < Lane.Right) {
      targetLane += 1;
      }

    // State Pattern: Slide mechanic using a timer to manage crouch state
    if(Input.IsActionJustPressed("ui_down") && _currentSlidingDuration <= 0) {
      GetNode<CollisionShape3D>("StandingCollision").Disabled = true;
      _currentSlidingDuration = SlidingDuration;
      }

    _currentSlidingDuration -= (float)delta;
    if(_currentSlidingDuration <= 0) {
      GetNode<CollisionShape3D>("StandingCollision").Disabled = false;
      }

		Velocity = velocity;
		MoveAndSlide();
	}
}
