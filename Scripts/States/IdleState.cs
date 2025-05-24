using Godot;

// Represents the player's idle state
public class IdleState : IPlayerState
{
    public void Enter(Player player)
    {
        // Optional: Play idle animation or reset movement
        // GD.Print("Entered Idle State");
    }

    public void Exit(Player player)
    {
        // Optional: Cleanup before exiting idle state
        // GD.Print("Exited Idle State");
    }

    public void Update(Player player, double delta)
    {
        // Currently no idle behavior
    }
}
