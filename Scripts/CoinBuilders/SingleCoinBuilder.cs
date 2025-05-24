using Godot;
using System.Collections.Generic;

// Builds a single coin in one random lane
public class SingleCoinBuilder : ICoinFormationBuilder
{
    public List<Pickupable> Build(Vector3 spawnPosition, float[] lanePositions, PackedScene coinScene, float speed)
    {
        var coins = new List<Pickupable>();
        int openLane = (int)(GD.Randi() % 3);  // Select a random lane (0 to 2)

        var coin = coinScene.Instantiate() as Pickupable;
        coin.Position = new Vector3(lanePositions[openLane], 0, spawnPosition.Z);
        coin.Speed = speed;

        coins.Add(coin);
        return coins;
    }
}
