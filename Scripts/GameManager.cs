// using Godot;
// using Godot.Collections;
// using System;
// using System.Collections.Generic;

// public partial class GameManager : Node3D
// {
//     [Signal]
//     public delegate void ScoreUpdatedEventHandler(int score);

//     [Signal]
//     public delegate void GameoverEventHandler(int score);

//     [Signal]
//     public delegate void StartGameEventHandler();

//     [Export]
//     public Array<PackedScene> ObstacleScenes { get; set; } = new Array<PackedScene>();

//     [Export]
//     public Array<PackedScene> PickupableScenes { get; set; } = new Array<PackedScene>();

//     [Export]
//     public float SpawnDistance { get; set; } = -20.0f;

//     [Export]
//     public float SpeedMultiplayer = 0.2f;

//     [Export]
//     public float SpeedIncreaseMulitplayer = 0.1f;

//     [Export]
//     public float PickupableSpawnChance = 0.3f;

//     public int Score = 0;

//     private readonly float[] _lanePositions = { -2.0f, 0.0f, 2.0f };
//     private List<IObstacleFactory> _obstacleFactories = new List<IObstacleFactory>();

//     public override void _Ready()
//     {
//         foreach (var scene in ObstacleScenes)
//         {
//             _obstacleFactories.Add(new StandardObstacleFactory(scene));
//         }

//         GetNode<Timer>("ObstacleSpawnTimer").Start();
//         GetNode<Timer>("Score").Start();
//     }

//     public override void _Process(double delta)
//     {
//         // Not used
//     }

//     public void _on_score_timeout()
//     {
//         Score += 1;
//         EmitSignal(SignalName.ScoreUpdated, Score);
//     }

//     public void _on_obstacle_spawn_timer_timeout()
//     {
//         GD.Print("Spawning Object");

//         float randValue = GD.Randf();
//         if (randValue < PickupableSpawnChance)
//         {
//             int sceneIndex = (int)(GD.Randi() % PickupableScenes.Count);
//             PackedScene pickupableScene = PickupableScenes[sceneIndex];
//             Node instance = pickupableScene.Instantiate();
//             if (instance is Pickupable pickupable)
//             {
//                 if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
//                 {
//                     spawnCoin(pickupable);
//                 }
//             }
//         }
//         else
//         {
//             int factoryIndex = (int)(GD.Randi() % _obstacleFactories.Count);
//             Obstacle baseObstacle = _obstacleFactories[factoryIndex].CreateObstacle();

//             if (baseObstacle.CurrentType == Obstacle.ObstacleType.Standard)
//             {
//                 int rand = (int)(GD.Randi() % 2);

//                 if (rand == 1)
//                 {
//                     int openLane = (int)(GD.Randi() % 3);
//                     baseObstacle.Position = new Vector3(_lanePositions[openLane], 0, SpawnDistance);
//                     baseObstacle.Speed += baseObstacle.Speed * SpeedMultiplayer;
//                     GetNode("ObstacleContainer").AddChild(baseObstacle);
//                 }
//                 else
//                 {
//                     int openLane = (int)(GD.Randi() % 3);

//                     for (int i = 0; i < 3; i++)
//                     {
//                         if (i != openLane)
//                         {
//                             Obstacle clone = _obstacleFactories[factoryIndex].CreateObstacle();
//                             clone.Position = new Vector3(_lanePositions[i], 0, SpawnDistance);
//                             clone.Speed += clone.Speed * SpeedMultiplayer;
//                             GetNode("ObstacleContainer").AddChild(clone);
//                         }
//                     }

//                     baseObstacle.QueueFree(); // Dispose unused base
//                 }
//             }
//             else
//             {
//                 baseObstacle.Position = new Vector3(_lanePositions[1], 0, SpawnDistance);
//                 baseObstacle.Speed += baseObstacle.Speed * SpeedMultiplayer;
//                 GetNode("ObstacleContainer").AddChild(baseObstacle);
//             }
//         }
//     }

//     private void spawnCoin(Pickupable pickupable)
//     {
//         int openLane = (int)(GD.Randi() % 3);
//         int formationType = (int)(GD.Randi() % 3);
//         GD.Print("Spawning Coin");

//         switch (formationType)
//         {
//             case 0: // Single coin
//                 pickupable.Position = new Vector3(_lanePositions[openLane], 0, SpawnDistance);
//                 pickupable.Speed += pickupable.Speed * SpeedMultiplayer;
//                 GetNode("ObstacleContainer").AddChild(pickupable);
//                 break;

//             case 1: // Coins in a straight line
//                 int numberOfCoins = (int)(GD.Randi() % 7);
//                 for (int i = 0; i < numberOfCoins; i++)
//                 {
//                     Pickupable coin = pickupable.Duplicate() as Pickupable;
//                     coin.Position = new Vector3(_lanePositions[openLane], 0, SpawnDistance + (i * 2.0f));
//                     coin.Speed += coin.Speed * SpeedMultiplayer;
//                     GetNode("ObstacleContainer").AddChild(coin);
//                 }
//                 pickupable.QueueFree();
//                 break;

//             case 2: // Diagonal coins
//                 int direction = GD.Randf() > 0.5f ? 1 : -1;
//                 for (int i = 0; i < 3; i++)
//                 {
//                     int index = Mathf.PosMod(openLane + (i * direction), 3);
//                     Pickupable coin = pickupable.Duplicate() as Pickupable;
//                     coin.Speed += coin.Speed * SpeedMultiplayer;
//                     coin.Position = new Vector3(_lanePositions[index], 0, SpawnDistance + (i * 2.0f));
//                     GetNode("ObstacleContainer").AddChild(coin);
//                 }
//                 pickupable.QueueFree();
//                 break;
//         }
//     }

//     public void _on_player_score_updated(int score)
//     {
//         Score += score;
//         EmitSignal(SignalName.ScoreUpdated, Score);
//         SpeedMultiplayer += SpeedIncreaseMulitplayer;
//     }

//     public void _on_kill_plane_area_entered(Area3D area)
//     {
//         EmitSignal(SignalName.Gameover, Score);
//         Array<Node> children = GetNode<Node3D>("ObstacleContainer").GetChildren();
//         foreach (var item in children)
//         {
//             item.QueueFree();
//         }
//         GetNode<Timer>("ObstacleSpawnTimer").Stop();
//         GetNode<Timer>("Score").Stop();
//     }

//     public void _on_ui_manager_start_game()
//     {
//         EmitSignal(SignalName.StartGame);
//         GetNode<Timer>("ObstacleSpawnTimer").Start();
//         GetNode<Timer>("Score").Start();
//         Score = 0;
//     }
// }


// using Godot;
// using Godot.Collections;
// using System;
// using System.Collections.Generic;

// public partial class GameManager : Node3D
// {
//     [Signal]
//     public delegate void ScoreUpdatedEventHandler(int score);

//     [Signal]
//     public delegate void GameoverEventHandler(int score);

//     [Signal]
//     public delegate void StartGameEventHandler();

//     [Export]
//     public Array<PackedScene> ObstacleScenes { get; set; } = new Array<PackedScene>();

//     [Export]
//     public Array<PackedScene> PickupableScenes { get; set; } = new Array<PackedScene>();

//     [Export]
//     public float SpawnDistance { get; set; } = -20.0f;

//     [Export]
//     public float SpeedMultiplayer = 0.2f;

//     [Export]
//     public float SpeedIncreaseMulitplayer = 0.1f;

//     [Export]
//     public float PickupableSpawnChance = 0.3f;

//     public int Score = 0;

//     private readonly float[] _lanePositions = { -2.0f, 0.0f, 2.0f };
//     private List<IObstacleFactory> _obstacleFactories = new List<IObstacleFactory>();

//     public override void _Ready()
//     {
//         foreach (var scene in ObstacleScenes)
//         {
//             _obstacleFactories.Add(new StandardObstacleFactory(scene));
//         }

//         // GetNode<Timer>("ObstacleSpawnTimer").Start();
//         // GetNode<Timer>("Score").Start();
//     }

//     public override void _Process(double delta)
//     {
//         // Not used
//     }

//     public void _on_score_timeout()
//     {
//         Score += 1;
//         EmitSignal(SignalName.ScoreUpdated, Score);
//     }

//     public void _on_obstacle_spawn_timer_timeout()
//     {
//         GD.Print("Spawning Object");

//         float randValue = GD.Randf();
//         if (randValue < PickupableSpawnChance)
//         {
//             int sceneIndex = (int)(GD.Randi() % PickupableScenes.Count);
//             PackedScene pickupableScene = PickupableScenes[sceneIndex];
//             Node instance = pickupableScene.Instantiate();
//             if (instance is Pickupable pickupable)
//             {
//                 if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
//                 {
//                     spawnCoin(pickupableScene);
//                     pickupable.QueueFree(); // Clean up original
//                 }
//             }
//         }
//         else
//         {
//             int factoryIndex = (int)(GD.Randi() % _obstacleFactories.Count);
//             Obstacle baseObstacle = _obstacleFactories[factoryIndex].CreateObstacle();

//             if (baseObstacle.CurrentType == Obstacle.ObstacleType.Standard)
//             {
//                 int rand = (int)(GD.Randi() % 2);

//                 if (rand == 1)
//                 {
//                     int openLane = (int)(GD.Randi() % 3);
//                     baseObstacle.Position = new Vector3(_lanePositions[openLane], 0, SpawnDistance);
//                     baseObstacle.Speed += baseObstacle.Speed * SpeedMultiplayer;
//                     GetNode("ObstacleContainer").AddChild(baseObstacle);
// 					if (baseObstacle is IGameElement element)
// {
//     element.OnSpawn();
// }

//                 }
//                 else
//                 {
//                     int openLane = (int)(GD.Randi() % 3);

//                     for (int i = 0; i < 3; i++)
//                     {
//                         if (i != openLane)
//                         {
//                             Obstacle clone = _obstacleFactories[factoryIndex].CreateObstacle();
//                             clone.Position = new Vector3(_lanePositions[i], 0, SpawnDistance);
//                             clone.Speed += clone.Speed * SpeedMultiplayer;
//                             GetNode("ObstacleContainer").AddChild(clone);
// 							if (baseObstacle is IGameElement element)
// {
//     element.OnSpawn();
// }

//                         }
//                     }

//                     baseObstacle.QueueFree(); // Dispose unused base
//                 }
//             }
//             else
//             {
//                 baseObstacle.Position = new Vector3(_lanePositions[1], 0, SpawnDistance);
//                 baseObstacle.Speed += baseObstacle.Speed * SpeedMultiplayer;
//                 GetNode("ObstacleContainer").AddChild(baseObstacle);
//             }
//         }
//     }

//     private void spawnCoin(PackedScene coinScene)
//     {
//         int formationType = (int)(GD.Randi() % 3);
//         Vector3 spawnPos = new Vector3(0, 0, SpawnDistance);

//         ICoinFormationBuilder builder = formationType switch
//         {
//             0 => new SingleCoinBuilder(),
//             1 => new RowCoinBuilder(),
//             2 => new DiagonalCoinBuilder(),
//             _ => new SingleCoinBuilder()
//         };

//         List<Pickupable> coins = builder.Build(spawnPos, _lanePositions, coinScene, 10.0f);

//         foreach (var coin in coins)
//         {
//             GetNode("ObstacleContainer").AddChild(coin);
// 			if (coin is IGameElement element)
//     {
//         element.OnSpawn();
//     }
//         }
//     }

//     public void _on_player_score_updated(int score)
//     {
//         Score += score;
//         EmitSignal(SignalName.ScoreUpdated, Score);
//         SpeedMultiplayer += SpeedIncreaseMulitplayer;
//     }

//     public void _on_kill_plane_area_entered(Area3D area)
//     {
//         EmitSignal(SignalName.Gameover, Score);
//         Array<Node> children = GetNode<Node3D>("ObstacleContainer").GetChildren();
//         // foreach (var item in children)
//         // {
//         //     item.QueueFree();
//         // }

// 		foreach (var item in children)
// {
//     if (item is IGameElement element)
//     {
//         element.OnDestroy();
//     }
//     else
//     {
//         item.QueueFree();
//     }
// }

//         GetNode<Timer>("ObstacleSpawnTimer").Stop();
//         GetNode<Timer>("Score").Stop();
//     }

//     public void _on_ui_manager_start_game()
//     {
//         EmitSignal(SignalName.StartGame);
//         GetNode<Timer>("ObstacleSpawnTimer").Start();
//         GetNode<Timer>("Score").Start();
//         Score = 0;
//     }
// }


using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GameManager : Node3D
{
    [Signal]
    public delegate void ScoreUpdatedEventHandler(int score);

    [Signal]
    public delegate void GameoverEventHandler(int score);

    [Signal]
    public delegate void StartGameEventHandler();

    [Export]
    public Array<PackedScene> ObstacleScenes { get; set; } = new Array<PackedScene>();

    [Export]
    public Array<PackedScene> PickupableScenes { get; set; } = new Array<PackedScene>();

    [Export]
    public float SpawnDistance { get; set; } = -20.0f;

    [Export]
    public float SpeedMultiplayer = 0.2f;

    [Export]
    public float SpeedIncreaseMulitplayer = 0.1f;

    [Export]
    public float PickupableSpawnChance = 0.3f;

    public int Score = 0;

    private readonly float[] _lanePositions = { -2.0f, 0.0f, 2.0f };
    private List<IObstacleFactory> _obstacleFactories = new List<IObstacleFactory>();

    public override void _Ready()
    {
        foreach (var scene in ObstacleScenes)
        {
            _obstacleFactories.Add(new StandardObstacleFactory(scene));
        }
    }

    public override void _Process(double delta) { }

    public void _on_score_timeout()
    {
        Score += 1;
        EmitSignal(SignalName.ScoreUpdated, Score);
    }

    public void _on_obstacle_spawn_timer_timeout()
    {
        GD.Print("Spawning Object");

        float randValue = GD.Randf();
        if (randValue < PickupableSpawnChance)
        {
            int sceneIndex = (int)(GD.Randi() % PickupableScenes.Count);
            PackedScene pickupableScene = PickupableScenes[sceneIndex];
            Node instance = pickupableScene.Instantiate();
            if (instance is Pickupable pickupable)
            {
                if (pickupable.CurrentType == Pickupable.PickupableType.Coin)
                {
                    spawnCoin(pickupableScene);
                    pickupable.QueueFree(); // Clean up original
                }
            }
        }
        else
        {
            int factoryIndex = (int)(GD.Randi() % _obstacleFactories.Count);
            Obstacle baseObstacle = _obstacleFactories[factoryIndex].CreateObstacle();

            if (baseObstacle.CurrentType == Obstacle.ObstacleType.Standard)
            {
                int rand = (int)(GD.Randi() % 2);

                if (rand == 1)
                {
                    int openLane = (int)(GD.Randi() % 3);
                    LaneManager laneMgr = LaneManager.GetInstance(openLane);

                    baseObstacle.Position = new Vector3(_lanePositions[openLane], 0, SpawnDistance);
                    baseObstacle.Speed += baseObstacle.Speed * SpeedMultiplayer * laneMgr.LaneSpeedMultiplier;

                    GetNode("ObstacleContainer").AddChild(baseObstacle);
                    if (baseObstacle is IGameElement element)
                        element.OnSpawn();
                }
                else
                {
                    int openLane = (int)(GD.Randi() % 3);

                    for (int i = 0; i < 3; i++)
                    {
                        if (i != openLane)
                        {
                            Obstacle clone = _obstacleFactories[factoryIndex].CreateObstacle();
                            LaneManager laneMgr = LaneManager.GetInstance(i);

                            clone.Position = new Vector3(_lanePositions[i], 0, SpawnDistance);
                            clone.Speed += clone.Speed * SpeedMultiplayer * laneMgr.LaneSpeedMultiplier;

                            GetNode("ObstacleContainer").AddChild(clone);
                            if (clone is IGameElement element)
                                element.OnSpawn();
                        }
                    }

                    baseObstacle.QueueFree(); // Dispose unused base
                }
            }
            else
            {
                LaneManager laneMgr = LaneManager.GetInstance(1);

                baseObstacle.Position = new Vector3(_lanePositions[1], 0, SpawnDistance);
                baseObstacle.Speed += baseObstacle.Speed * SpeedMultiplayer * laneMgr.LaneSpeedMultiplier;

                GetNode("ObstacleContainer").AddChild(baseObstacle);
            }
        }
    }

    private void spawnCoin(PackedScene coinScene)
    {
        int formationType = (int)(GD.Randi() % 3);
        Vector3 spawnPos = new Vector3(0, 0, SpawnDistance);

        ICoinFormationBuilder builder = formationType switch
        {
            0 => new SingleCoinBuilder(),
            1 => new RowCoinBuilder(),
            2 => new DiagonalCoinBuilder(),
            _ => new SingleCoinBuilder()
        };

        List<Pickupable> coins = builder.Build(spawnPos, _lanePositions, coinScene, 10.0f);

        foreach (var coin in coins)
        {
            GetNode("ObstacleContainer").AddChild(coin);
            if (coin is IGameElement element)
                element.OnSpawn();
        }
    }

    public void _on_player_score_updated(int score)
    {
        Score += score;
        EmitSignal(SignalName.ScoreUpdated, Score);
        SpeedMultiplayer += SpeedIncreaseMulitplayer;
    }

    public void _on_kill_plane_area_entered(Area3D area)
    {
        EmitSignal(SignalName.Gameover, Score);

        Array<Node> children = GetNode<Node3D>("ObstacleContainer").GetChildren();
        foreach (var item in children)
        {
            if (item is IGameElement element)
                element.OnDestroy();
            else
                item.QueueFree();
        }

        GetNode<Timer>("ObstacleSpawnTimer").Stop();
        GetNode<Timer>("Score").Stop();
    }

    public void _on_ui_manager_start_game()
    {
        EmitSignal(SignalName.StartGame);
        GetNode<Timer>("ObstacleSpawnTimer").Start();
        GetNode<Timer>("Score").Start();
        Score = 0;
		SpeedMultiplayer = 0.2f;
    }
}
