using Godot;
using Godot.Collections;

namespace StrategyTutorial1.Characters.Cameras;

public partial class SimpleCamera : Camera3D
{
    [Export()] public float RayLength { get; set; } = 100f;

    [Export(PropertyHint.Layers3dPhysics)] public uint CollisionMask { get; set; }
    [Export()] public PackedScene Building { get; set; }

    private Array<RID> _excluded;
    private World3D _world;
    private Window _root;

    public override void _Ready()
    {
        _excluded = new Array<RID>();

        _world = GetWorld3d();

        _root = GetTree().Root;
    }

    public override void _PhysicsProcess(double delta)
    {
        Dictionary collisions = DetectCollisions();

        if (collisions.Count > 0)
        {
            if (Input.IsActionPressed("Action"))
            {
                Node3D scene = Building.Instantiate<Node3D>();

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

        PhysicsRayQueryParameters3D parameters = PhysicsRayQueryParameters3D.Create(from, to, CollisionMask, _excluded);

        return spaceState.IntersectRay(parameters);
    }
}