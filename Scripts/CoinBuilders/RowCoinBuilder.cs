using Godot;
using System.Collections.Generic;

// Builds a straight line of coins in a single lane
public class RowCoinBuilder : ICoinFormationBuilder
{
    public List<Pickupable> Build(Vector3 spawnPosition, float[] lanePositions, PackedScene coinScene, float speed)
    {
        var coins = new List<Pickupable>();

        int openLane = (int)(GD.Randi() % 3);              // Randomly choose a lane
        int count = (int)(GD.Randi() % 6) + 2;              // Generate 2 to 7 coins

        for (int i = 0; i < count; i++)
        {
            var coin = coinScene.Instantiate() as Pickupable;
            coin.Position = new Vector3(lanePositions[openLane], 0, spawnPosition.Z + (i * 2.0f));
            coin.Speed = speed;
            coins.Add(coin);
        }

        return coins;
    }
}
