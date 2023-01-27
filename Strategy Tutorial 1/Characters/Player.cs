using Godot;

namespace StrategyTutorial1.Characters;

public partial class Player : StaticBody3D
{
	[Export()] public float Speed { get; set; } = 10f;
	private Vector3 _direction;
	
	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		_direction = new Vector3();
		
		if (Input.IsActionPressed("MoveLeft"))
		{
			_direction.x = -1;
		}
		else if (Input.IsActionPressed("MoveRight"))
		{
			_direction.x = 1;
		}

		if (Input.IsActionPressed("MoveForward"))
		{
			_direction.z = -1;
		}
		else if (Input.IsActionPressed("MoveBackward"))
		{
			_direction.z = 1;
		}

		MoveAndCollide(_direction * Speed * (float)delta);
		
	}
}
