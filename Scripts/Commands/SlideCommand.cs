using Godot;

public class SlideCommand : ICommand
{
    public void Execute(Player player, ref Vector3 velocity)
    {
        player.ChangeState(new SlideState());
    }
}
