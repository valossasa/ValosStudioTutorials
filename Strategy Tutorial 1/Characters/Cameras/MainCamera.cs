using Godot;
using Godot.Collections;
using StrategyTutorial1.Signals;

namespace StrategyTutorial1.Characters.Cameras;

public partial class MainCamera : Camera3D
{
    [Export()] public float RayLength { get; set; } = 100f;

    private uint _collisionMask;
    private PackedScene _build;
    private Array _excluded;
    private World3D _world;
    private Window _root;

    public override void _Ready()
    {
        GameplaySignals gameplaySignals = GetNode<GameplaySignals>("/root/GameplaySignals");

        gameplaySignals.PlaceObject += GameplaySignalsOnPlaceObject;

        _excluded = new Array { this };

        _world = GetWorld3d();
        
        _root = GetTree().Root;

        SetPhysicsProcess(false);
    }

    private void GameplaySignalsOnPlaceObject(PackedScene buildable, uint mask)
    {
        SetPhysicsProcess(buildable != null);

        _build = buildable;

        _collisionMask = mask;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("Cancel"))
        {
            GameplaySignalsOnPlaceObject(null, 0);
            
            return;
        }

        Dictionary collisions = DetectCollisions();

        if (collisions.Count > 0)
        {
            if (Input.IsActionPressed("Action"))
            {
                Node3D scene = _build.Instantiate<Node3D>();

                _root.AddChild(scene);

                scene.GlobalPosition = (Vector3)collisions["position"];
            }
        }
    }

    private Dictionary DetectCollisions()
    {
        PhysicsDirectSpaceState3D spaceState = _world.DirectSpaceState;

        Vector2 mouse = GetViewport().GetMousePosition();

        Vector3 from = ProjectRayOrigin(mouse);

        Vector3 to = from + ProjectRayNormal(mouse) * RayLength;

        PhysicsRayQueryParameters3D parameters = PhysicsRayQueryParameters3D.Create(from, to, _collisionMask, null);

        return spaceState.IntersectRay(parameters);
    }
}