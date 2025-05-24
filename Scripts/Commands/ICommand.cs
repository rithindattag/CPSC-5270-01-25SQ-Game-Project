using Godot;

// Interface for executing player commands like jump, slide, or lane switch
public interface ICommand
{
    void Execute(Player player, ref Vector3 velocity);
}
