using Godot;

// Represents the player's sliding state
public class SlideState : IPlayerState
{
    private float _duration;
    private float _elapsed = 0f;

    public void Enter(Player player)
    {
        _duration = player.SlidingDuration;

        // Disable the standing collider while sliding
        var collider = player.GetNode<CollisionShape3D>("StandingCollision");
        if (collider != null)
            collider.Disabled = true;
    }

    public void Exit(Player player)
    {
        // Re-enable the standing collider when slide ends
        var collider = player.GetNode<CollisionShape3D>("StandingCollision");
        if (collider != null)
            collider.Disabled = false;
    }

    public void Update(Player player, double delta)
    {
        _elapsed += (float)delta;

        // Return to idle after sliding duration ends
        if (_elapsed >= _duration)
        {
            player.ChangeState(new IdleState());
        }
    }
}
