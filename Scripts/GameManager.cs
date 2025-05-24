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
            // Handle coin spawn
            int sceneIndex = (int)(GD.Randi() % PickupableScenes.Count);
            PackedScene pickupableScene = PickupableScenes[sceneIndex];
            Node instance = pickupableScene.Instantiate();

            if (instance is Pickupable pickupable && pickupable.CurrentType == Pickupable.PickupableType.Coin)
            {
                spawnCoin(pickupableScene);
                pickupable.QueueFree();
            }
        }
        else
        {
            // Handle obstacle spawn via Factory
            int factoryIndex = (int)(GD.Randi() % _obstacleFactories.Count);
            Obstacle baseObstacle = _obstacleFactories[factoryIndex].CreateObstacle();

            if (baseObstacle.CurrentType == Obstacle.ObstacleType.Standard)
            {
                int rand = (int)(GD.Randi() % 2);

                if (rand == 1)
                {
                    // Single obstacle with lane variation
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
                    // Spawn obstacle on all but one lane
                    int openLane = (int)(GD.Randi() % 3);

                    for (int i = 0; i < 3; i++)
                    {
                        if (i == openLane) continue;

                        Obstacle clone = _obstacleFactories[factoryIndex].CreateObstacle();
                        LaneManager laneMgr = LaneManager.GetInstance(i);
                        clone.Position = new Vector3(_lanePositions[i], 0, SpawnDistance);
                        clone.Speed += clone.Speed * SpeedMultiplayer * laneMgr.LaneSpeedMultiplier;

                        GetNode("ObstacleContainer").AddChild(clone);
                        if (clone is IGameElement element)
                            element.OnSpawn();
                    }

                    baseObstacle.QueueFree(); // Discard unused original
                }
            }
            else
            {
                // Handle non-standard obstacle types
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
        SpeedMultiplayer = 0.2f; // Reset speed to base on new game start
    }
}
