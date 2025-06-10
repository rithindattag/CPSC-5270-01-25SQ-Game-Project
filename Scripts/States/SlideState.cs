using Godot;

public class SlideState : IPlayerState
{
    private float _duration;
    private float _elapsed = 0f;
    [Export]
    public float SlidingDuration = 2f;


    public void Enter(Player player)
    {
        _duration = player.SlidingDuration;
        player.PlayAnimation("Crouch_Idle");

        var collider = player.GetNode<CollisionShape3D>("StandingCollision");
        if (collider != null)
            collider.Disabled = true;
    }

    public void Exit(Player player)
    {
        var collider = player.GetNode<CollisionShape3D>("StandingCollision");
        if (collider != null)
            collider.Disabled = false;
    }

    public void Update(Player player, double delta)
    {
        _elapsed += (float)delta;

        if (_elapsed >= _duration)
        {
            player.ChangeState(new RunState());
        }
    }
}
