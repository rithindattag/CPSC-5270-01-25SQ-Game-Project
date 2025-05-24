using Godot;

public class MoveLaneCommand : ICommand
{
    private ILaneSwitchStrategy _strategy;

    public MoveLaneCommand(ILaneSwitchStrategy strategy)
    {
        _strategy = strategy;
    }

    public void Execute(Player player, ref Vector3 velocity)
    {
        int updated = _strategy.GetNextLane((int)player.CurrentLane);
        updated = Mathf.Clamp(updated, (int)Player.Lane.Left, (int)Player.Lane.Right);
        player.CurrentLane = (Player.Lane)updated;
    }
}
