using Godot;
using StrategyTutorial1.Signals;

namespace StrategyTutorial1.Buttons;

public partial class BuildingButton : Button
{
    [Export(PropertyHint.Layers3DPhysics)] public uint CollisionMask { get; set; }
    [Export()] public PackedScene Building { get; set; }

    private GameplaySignals _gameplaySignals;

    public override void _Ready()
    {
        _gameplaySignals = GetNode<GameplaySignals>("/root/GameplaySignals");

        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        _gameplaySignals.EmitSignal(nameof(GameplaySignals.PlaceObject), Building, CollisionMask);
    }
}