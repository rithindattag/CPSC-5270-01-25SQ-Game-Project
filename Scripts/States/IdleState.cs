using Godot;

public class IdleState : IPlayerState
{
    public void Enter(Player player)
    {
        player.PlayAnimation("Idle");
    }

    public void Exit(Player player) { }

    public void Update(Player player, double delta)
    {
        // No transitions; stays idle until game starts
    }
}
