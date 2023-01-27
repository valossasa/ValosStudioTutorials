using Godot;

namespace StrategyTutorial1.Signals;

public partial class GameplaySignals : Node
{
    [Signal]
    public delegate void PlaceObjectEventHandler(PackedScene preview, PackedScene buildable, uint mask);
}