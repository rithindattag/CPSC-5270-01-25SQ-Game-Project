public class SwitchRightStrategy : ILaneSwitchStrategy
{
    public int GetNextLane(int currentLane)
    {
        return currentLane < 2 ? currentLane + 1 : currentLane;
    }
}
