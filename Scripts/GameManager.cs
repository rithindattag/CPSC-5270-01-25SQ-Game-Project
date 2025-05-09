using Godot;
using Godot.Collections;
using System;

public partial class GameManager : Node3D
{

	[Export]
	public Array<PackedScene> ObstacleScenes {get; set;} = new Array<PackedScene>();

	[Export]
	public Array<PackedScene> PowerUpScenes {get; set;} = new Array<PackedScene>();
		
	[Export]
	public float SpawnDistance {get; set;} = -20.0f;

	private readonly float[] _lanePositions = {-2.0f, 0.0f, 2.0f};

	public override void _Ready()
	{
		GetNode<Timer>("ObstacleSpawnTimer").Start();
		GetNode<Timer>("Score").Start();
	}

	public override void _Process(double delta)
	{
		// Nothing here yet
	}

	public void _on_score_timeout() {
		// Scoring logic (to be implemented)
	}

	public void _on_obstacle_spawn_timer_timeout() {
		GD.Print("Spawning Object");

		// Factory Method Pattern:
		// Randomly selct an obstacle type from a set of predefined obstacle scenes
		int sceneIndex = (int)(GD.Randi() % ObstacleScenes.Count);
		PackedScene obstacleScene = ObstacleScenes[sceneIndex];
		Node instance = obstacleScene.Instantiate();

		if(instance is Obstacle obstacleInstance) {
			if(obstacleInstance.CurrentType == Obstacle.ObstacleType.Standard) {
			GD.Print(GD.Randi() % 2);
			int rand = (int)(GD.Randi() % 2);

			if(rand == 1) {
			int openLane = (int)(GD.Randi() % 3);
			GD.Print("Spawning Obstacle");

			obstacleInstance.Position = new Vector3(_lanePositions[openLane], 0, SpawnDistance);
			GetNode("ObstacleContainer").AddChild(obstacleInstance);
		} else {
			int openLane = (int)(GD.Randi() % 3);

			// Prototype Pattern:
			// Instantiate multiple copies of the same obstacle across different lanes
			for(int i = 0; i < 3; i++) {
				if(i != openLane) {
					Obstacle currentObstacle = obstacleScene.Instantiate() as Obstacle;
					currentObstacle.Position = new Vector3(_lanePositions[i], 0, SpawnDistance);
					GetNode("ObstacleContainer").AddChild(currentObstacle);
				}
			}
			instance.QueueFree(); // Dispose of the unused base instance
		}
	} else {
				obstacleInstance.Position = new Vector3(_lanePositions[1], 0, SpawnDistance);
				GetNode("ObstacleContainer").AddChild(obstacleInstance);
			}
	  }
}
}
