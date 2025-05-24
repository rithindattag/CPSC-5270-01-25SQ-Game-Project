using Godot;

// Command to switch the player's lane using a strategy
public class MoveLaneCommand : ICommand
{
    private ILaneSwitchStrategy _strategy;

    public MoveLaneCommand(ILaneSwitchStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Execute(Player player, ref Vector3 velocity)
    {
        // Get the next lane index using strategy and clamp it within valid range
        int updated = _strategy.GetNextLane((int)player.CurrentLane);
        updated = Mathf.Clamp(updated, (int)Player.Lane.Left, (int)Player.Lane.Right);

        // Apply lane switch
        player.CurrentLane = (Player.Lane)updated;
    }
}
