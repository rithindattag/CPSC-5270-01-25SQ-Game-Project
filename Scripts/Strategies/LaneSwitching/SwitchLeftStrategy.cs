// Strategy to move the player one lane to the left, if possible
public class SwitchLeftStrategy : ILaneSwitchStrategy
{
    public int GetNextLane(int currentLane)
    {
        return currentLane > 0 ? currentLane - 1 : currentLane;
    }
}
