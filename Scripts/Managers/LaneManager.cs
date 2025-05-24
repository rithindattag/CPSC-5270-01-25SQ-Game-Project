using Godot;
using System.Collections.Generic;

// Multiton pattern to manage lane-specific configurations
public class LaneManager
{
    private static Dictionary<int, LaneManager> _instances = new();

    public int LaneIndex { get; }
    public float LaneSpeedMultiplier { get; set; } = 1.0f;

    private LaneManager(int laneIndex)
    {
        LaneIndex = laneIndex;

        // Set speed multipliers for each lane
        switch (laneIndex)
        {
            case 0:
                LaneSpeedMultiplier = 0.9f; // Slower in left lane
                break;
            case 2:
                LaneSpeedMultiplier = 1.1f; // Faster in right lane
                break;
            default:
                LaneSpeedMultiplier = 1.0f; // Normal speed in center lane
                break;
        }
    }

    // Retrieve or create instance for a specific lane
    public static LaneManager GetInstance(int laneIndex)
    {
        if (!_instances.ContainsKey(laneIndex))
            _instances[laneIndex] = new LaneManager(laneIndex);

        return _instances[laneIndex];
    }
}
