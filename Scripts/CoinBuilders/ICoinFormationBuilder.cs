using Godot;
using System.Collections.Generic;

public interface ICoinFormationBuilder
{
    List<Pickupable> Build(Vector3 spawnPosition, float[] lanePositions, PackedScene coinScene, float speed);
}
