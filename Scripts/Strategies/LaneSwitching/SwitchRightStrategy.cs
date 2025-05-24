// Strategy to move the player one lane to the right, if possible
public class SwitchRightStrategy : ILaneSwitchStrategy
{
    public int GetNextLane(int currentLane)
    {
        return currentLane < 2 ? currentLane + 1 : currentLane;
    }
}
