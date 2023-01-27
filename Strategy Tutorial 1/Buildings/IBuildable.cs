using Godot;

namespace StrategyTutorial1.Buildings;

public interface IBuildable
{
    bool Buildable { get; set; }
    void PreviewShow(PackedScene scene);

    void PreviewHide();

    void Build(PackedScene scene);
}