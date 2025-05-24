// Strategy that keeps the player in the current lane (no operation)
public class NoOpSwitchStrategy : ILaneSwitchStrategy
{
    public int GetNextLane(int currentLane)
    {
        return currentLane;
    }
}
