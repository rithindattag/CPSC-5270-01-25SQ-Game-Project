using Godot;
using System;

public partial class Player : CharacterBody3D
{
    // Signal to notify the game manager of a score update
    [Signal]
    public delegate void ScoreUpdatedEventHandler(int score);

    // Constants
    public const float Speed = 5.0f;
    public const float JumpVelocity = 5.0f;

    // Enum representing possible lanes for the player
    public enum Lane { Left = 0, Center = 1, Right = 2 }

    // Current lane the player is in
    public Lane CurrentLane = Lane.Center;

    // Lane parameters
    private int laneWidth = 2;
    private double laneChangeSpeed = 5;

    // Game state flag
    private bool dead = true;

    // Duration for sliding state
    [Export]
    public float SlidingDuration = 0.2f;

    // Current state of the player (State Pattern)
    private IPlayerState _currentState;

    public override void _Ready()
    {
        // Set initial state
        ChangeState(new IdleState());
    }

    public override void _PhysicsProcess(double delta)
    {
        if (dead) return;

        // Update current state behavior
        _currentState?.Update(this, delta);

        // Get current velocity
        Vector3 velocity = Velocity;

        // Apply gravity
        if (!IsOnFloor())
            velocity += GetGravity() * (float)delta;

        // COMMAND PATTERN: Handle jump
        if (PlayerInputFacade.IsJump())
            new JumpCommand().Execute(this, ref velocity);

        // COMMAND PATTERN + STRATEGY: Handle lane switching
        ICommand laneCommand = null;
        if (PlayerInputFacade.IsMoveLeft())
            laneCommand = new MoveLaneCommand(new SwitchLeftStrategy());
        else if (PlayerInputFacade.IsMoveRight())
            laneCommand = new MoveLaneCommand(new SwitchRightStrategy());

        laneCommand?.Execute(this, ref velocity);

        // Smoothly move the player toward the target lane
        float targetX = (int)CurrentLane * laneWidth - laneWidth;
        Position = new Vector3(
            Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)),
            Position.Y,
            Position.Z
        );

        // COMMAND PATTERN: Handle sliding
        if (PlayerInputFacade.IsSlide() && _currentState is not SlideState)
            new SlideCommand().Execute(this, ref velocity);

        // Apply updated velocity
        Velocity = velocity;
        MoveAndSlide();
    }

    // Handles transition to a new player state
    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    // Detects when player collides with an Area3D
    public void _on_area_3d_area_entered(Area3D area)
    {
        if (area is Pickupable pickupable && pickupable.CurrentType == Pickupable.PickupableType.Coin)
        {
            EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
            pickupable.QueueFree();
        }
    }

    // Resets player on game start
    public void _on_game_manager_start_game()
    {
        Position = new Vector3(0, 0, 0);
        dead = false;
        ChangeState(new IdleState());
    }

    // Handles player death
    public void _on_game_manager_gameover(int score)
    {
        dead = true;
    }
}
