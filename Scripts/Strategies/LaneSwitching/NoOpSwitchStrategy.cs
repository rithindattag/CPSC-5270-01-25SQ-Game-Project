public class NoOpSwitchStrategy : ILaneSwitchStrategy
{
    public int GetNextLane(int currentLane)
    {
        return currentLane;
    }
}
