using Godot;

public class JumpCommand : ICommand
{
    public void Execute(Player player, ref Vector3 velocity)
    {
        GD.Print("✅ JumpCommand executed!");
        if (player.IsOnFloor())
        {
            velocity.Y = Player.JumpVelocity;
        }
    }
}
