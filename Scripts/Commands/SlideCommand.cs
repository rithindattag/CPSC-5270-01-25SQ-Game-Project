using Godot;

// Command to trigger sliding by switching the player to SlideState
public class SlideCommand : ICommand
{
    public void Execute(Player player, ref Vector3 velocity)
    {
        player.ChangeState(new SlideState());
    }
}
