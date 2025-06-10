using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Signal]
    public delegate void ScoreUpdatedEventHandler(int score);

    public const float Speed = 5.0f;
    public const float JumpVelocity = 5.0f;

    public enum Lane { Left = 0, Center = 1, Right = 2 }
    public Lane CurrentLane = Lane.Center;

    private int laneWidth = 2;
    private double laneChangeSpeed = 5;
    private bool dead = true;

    [Export]
    public float SlidingDuration = 0.2f;

    private IPlayerState _currentState;

    private AnimationPlayer _anim;
    private Vector3 _previousPosition;

    public override void _Ready()
    {
        _anim = GetNode<AnimationPlayer>("Pirate/AnimationPlayer"); // ✅ Adjusted to your node path
        ChangeState(new IdleState());
        _previousPosition = Position;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (dead) return;

        _currentState?.Update(this, delta);

        Vector3 velocity = Velocity;

        if (!IsOnFloor())
            velocity += GetGravity() * (float)delta;

        if (PlayerInputFacade.IsJump())
            new JumpCommand().Execute(this, ref velocity);

        ICommand laneCommand = null;
        if (PlayerInputFacade.IsMoveLeft())
            laneCommand = new MoveLaneCommand(new SwitchLeftStrategy());
        else if (PlayerInputFacade.IsMoveRight())
            laneCommand = new MoveLaneCommand(new SwitchRightStrategy());

        laneCommand?.Execute(this, ref velocity);

        float targetX = (int)CurrentLane * laneWidth - laneWidth;
        Position = new Vector3(
            Mathf.Lerp(Position.X, targetX, (float)(delta * laneChangeSpeed)),
            Position.Y,
            Position.Z
        );

        if (PlayerInputFacade.IsSlide() && _currentState is not SlideState)
            new SlideCommand().Execute(this, ref velocity);

        Velocity = velocity;
        MoveAndSlide();

        _previousPosition = Position;
    }

    public void ChangeState(IPlayerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState?.Enter(this);
    }

    public void PlayAnimation(string name)
    {
        if (_anim != null && (_anim.CurrentAnimation != name || !_anim.IsPlaying()))
        {
            _anim.Play(name);
        }
    }

    public bool IsRunning(double delta)
    {
        float horizontalSpeed = Mathf.Abs(Position.X - _previousPosition.X) / (float)delta;
        return horizontalSpeed > 0.01f;
    }

    public void _on_area_3d_area_entered(Area3D area)
    {
        if (area is Pickupable pickupable && pickupable.CurrentType == Pickupable.PickupableType.Coin)
        {
            EmitSignal(SignalName.ScoreUpdated, pickupable.ScoreValue);
            pickupable.QueueFree();
        }
    }

    public void _on_game_manager_start_game()
    {
        Position = new Vector3(0, 0, 0);
        dead = false;
        ChangeState(new RunState());
    }

    public void _on_game_manager_gameover(int score)
    {
        dead = true;
    }
}

