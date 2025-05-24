using Godot;
using System.Collections.Generic;

public class SingleCoinBuilder : ICoinFormationBuilder
{
    public List<Pickupable> Build(Vector3 spawnPosition, float[] lanePositions, PackedScene coinScene, float speed)
    {
        var coins = new List<Pickupable>();
        int openLane = (int)(GD.Randi() % 3);

        var coin = coinScene.Instantiate() as Pickupable;
        coin.Position = new Vector3(lanePositions[openLane], 0, spawnPosition.Z);
        coin.Speed = speed;

        coins.Add(coin);
        return coins;
    }
}
