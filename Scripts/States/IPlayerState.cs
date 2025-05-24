// Interface for player states used in the State pattern
public interface IPlayerState
{
    void Enter(Player player);      // Called when entering the state
    void Exit(Player player);       // Called when exiting the state
    void Update(Player player, double delta);  // Called every frame
}
