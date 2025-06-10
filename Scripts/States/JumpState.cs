using Godot;

public class JumpState : IPlayerState
{
    private bool hasJumped = false;

    public void Enter(Player player)
    {
        player.PlayAnimation("Jump");
        hasJumped = true;

        Vector3 velocity = player.Velocity;
        velocity.Y = Player.JumpVelocity;
        player.Velocity = velocity;
    }

    public void Exit(Player player) { }

    public void Update(Player player, double delta)
    {
        if (player.IsOnFloor() && hasJumped)
        {
            player.ChangeState(new RunState());
        }
    }
}
