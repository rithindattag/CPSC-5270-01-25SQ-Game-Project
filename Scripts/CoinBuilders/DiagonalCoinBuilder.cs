using Godot;
using System.Collections.Generic;

// Builds a diagonal coin formation across lanes
public class DiagonalCoinBuilder : ICoinFormationBuilder
{
    public List<Pickupable> Build(Vector3 spawnPosition, float[] lanePositions, PackedScene coinScene, float speed)
    {
        var coins = new List<Pickupable>();

        // Choose a starting lane and diagonal direction (left-to-right or right-to-left)
        int openLane = (int)(GD.Randi() % 3);
        int direction = GD.Randf() > 0.5f ? 1 : -1;

        // Place 3 coins diagonally across lanes
        for (int i = 0; i < 3; i++)
        {
            int index = Mathf.PosMod(openLane + (i * direction), 3);
            var coin = coinScene.Instantiate() as Pickupable;
            coin.Position = new Vector3(lanePositions[index], 0, spawnPosition.Z + (i * 2.0f));
            coin.Speed = speed;
            coins.Add(coin);
        }

        return coins;
    }
}
