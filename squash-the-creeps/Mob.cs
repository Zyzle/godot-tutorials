using Godot;
using System;

public partial class Mob : CharacterBody3D
{
	[Export]
	public int MinSpeed = 10;

	[Export]
	public int MaxSpeed = 18;

	private Vector3 _velocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void Initialize(Vector3 startPosition, Vector3 playerPosition)
	{
		LookAtFromPosition(startPosition, playerPosition, Vector3.Up);
		RotateY((float)GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));

		float randomSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);

		// Had to adapt this, seems right
		_velocity = Vector3.Forward * randomSpeed;
		_velocity = _velocity.Rotated(Vector3.Up, Rotation.Y);
		Velocity = _velocity;
	}

	public void OnVisibilityNotifierScreenExited()
	{
		QueueFree();
	}
}
