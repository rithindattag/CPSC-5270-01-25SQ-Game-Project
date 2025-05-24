public class SwitchLeftStrategy : ILaneSwitchStrategy
{
    public int GetNextLane(int currentLane)
    {
        return currentLane > 0 ? currentLane - 1 : currentLane;
    }
}
