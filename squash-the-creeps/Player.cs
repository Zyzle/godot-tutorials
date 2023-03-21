using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public int Speed = 14;

	[Export]
	public int FallAcceleration = 75;

	[Export]
	public int JumpInpulse = 20;

	[Export]
	public int BounceImpulse = 16;

	[Signal]
	public delegate void HitEventHandler();

	private Vector3 _velocity = Vector3.Zero;

	private void Die()
	{
		EmitSignal(nameof(Hit));
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1f;
		}

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
		}

		// Ground
		_velocity.X = direction.X * Speed;
		_velocity.Z = direction.Z * Speed;

		if (!IsOnFloor())
		{
			// Vertical
			_velocity.Y -= FallAcceleration * (float)delta;
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			_velocity.Y = JumpInpulse;
		}

		for (int index = 0; index < GetSlideCollisionCount(); index++)
		{
			KinematicCollision3D collision = GetSlideCollision(index);
			if (collision.GetCollider() is Mob mob && mob.IsInGroup("mob"))
			{
				if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
				{
					mob.Squash();
					_velocity.Y = BounceImpulse;
				}
			}
		}

		Velocity = _velocity;
		MoveAndSlide();
	}

	public void OnMobDetectorBodyEntered(Node3D body)
	{
		Die();
	}
}
