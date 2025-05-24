using Godot;

// Command to handle player jump logic
public class JumpCommand : ICommand
{
    public void Execute(Player player, ref Vector3 velocity)
    {
        // Apply jump only if the player is on the ground
        if (player.IsOnFloor())
        {
            velocity.Y = Player.JumpVelocity;
        }
    }
}
