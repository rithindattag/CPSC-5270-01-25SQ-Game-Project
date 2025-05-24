using Godot;
using System.Collections.Generic;

// Interface for building different coin formations
public interface ICoinFormationBuilder
{
    // Constructs and returns a list of coins in a specific formation
    List<Pickupable> Build(Vector3 spawnPosition, float[] lanePositions, PackedScene coinScene, float speed);
}
