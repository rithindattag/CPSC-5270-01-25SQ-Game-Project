using Godot;
using System.Collections.Generic;

public class LaneManager
{
    // Multiton store — one LaneManager per lane index (0, 1, 2)
    private static Dictionary<int, LaneManager> _instances = new Dictionary<int, LaneManager>();

    // Lane-specific configuration
    public int LaneIndex { get; }
    public float LaneSpeedMultiplier { get; set; } = 1.0f;

    // Private constructor — forces access through GetInstance
    private LaneManager(int laneIndex)
    {
        LaneIndex = laneIndex;

        // Example: Adjust speeds by lane
        if (laneIndex == 0)
            LaneSpeedMultiplier = 0.9f; // Left lane is slightly slower
        else if (laneIndex == 2)
            LaneSpeedMultiplier = 1.1f; // Right lane is slightly faster
    }

    public static LaneManager GetInstance(int laneIndex)
    {
        if (!_instances.ContainsKey(laneIndex))
            _instances[laneIndex] = new LaneManager(laneIndex);

        return _instances[laneIndex];
    }
}
