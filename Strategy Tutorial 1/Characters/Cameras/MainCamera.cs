using Godot;
using Godot.Collections;
using StrategyTutorial1.Buildings;
using StrategyTutorial1.Signals;

namespace StrategyTutorial1.Characters.Cameras;

public partial class MainCamera : Camera3D
{
    [Export()] public float RayLength { get; set; } = 100f;

    private uint _collisionMask;
    private IBuildable _buildable;
    private PackedScene _preview;
    private PackedScene _build;
    private Array _excluded;
    private World3D _world;

    public override void _Ready()
    {
        GameplaySignals gameplaySignals = GetNode<GameplaySignals>("/root/GameplaySignals");

        gameplaySignals.PlaceObject += GameplaySignalsOnPlaceObject;

        _excluded = new Array { this };

        _world = GetWorld3d();

        SetPhysicsProcess(false);
    }

    private void GameplaySignalsOnPlaceObject(PackedScene preview, PackedScene buildable, uint mask)
    {
        SetPhysicsProcess(preview != null);

        _preview = preview;

        _build = buildable;

        _collisionMask = mask;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("CancelAction"))
        {
            GameplaySignalsOnPlaceObject(null, null, 0);

            CleanPreview();

            return;
        }

        Dictionary collisions = DetectCollisions();

        if (collisions.Count > 0)
        {
            DetectBuildable((IBuildable)collisions["collider"]);
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

    private void DetectBuildable(IBuildable buildable)
    {
        if (buildable != _buildable)
        {
            CleanPreview();
        }

        if (Input.IsActionPressed("Action"))
        {
            buildable?.Build(_build);

            return;
        }

        if (_buildable == buildable)
        {
            return;
        }

        _buildable = buildable;

        buildable.PreviewShow(_preview);
    }

    private void CleanPreview()
    {
        _buildable?.PreviewHide();

        _buildable = null;
    }
}