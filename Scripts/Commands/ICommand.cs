using Godot;

public interface ICommand
{
    void Execute(Player player, ref Vector3 velocity);
}
