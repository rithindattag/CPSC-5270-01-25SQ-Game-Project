using Godot;

public class IdleState : IPlayerState
{
    public void Enter(Player player)
    {
        GD.Print("Entered Idle State");
    }

    public void Exit(Player player)
    {
        GD.Print("Exited Idle State");
    }

    public void Update(Player player, double delta)
    {
        // Placeholder for idle logic
    }
}
