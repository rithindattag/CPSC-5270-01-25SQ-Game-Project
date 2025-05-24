using Godot;
using System.Collections.Generic;

public class RowCoinBuilder : ICoinFormationBuilder
{
    public List<Pickupable> Build(Vector3 spawnPosition, float[] lanePositions, PackedScene coinScene, float speed)
    {
        var coins = new List<Pickupable>();
        int openLane = (int)(GD.Randi() % 3);
        int count = (int)(GD.Randi() % 6) + 2;

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
