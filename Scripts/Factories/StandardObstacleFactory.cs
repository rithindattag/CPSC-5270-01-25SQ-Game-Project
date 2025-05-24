using Godot;

// Factory for creating standard Obstacle instances from a PackedScene
public class StandardObstacleFactory : IObstacleFactory
{
    private PackedScene _scene;

    public StandardObstacleFactory(PackedScene scene)
    {
        _scene = scene;
    }

    public Obstacle CreateObstacle()
    {
        return _scene.Instantiate() as Obstacle;
    }
}
