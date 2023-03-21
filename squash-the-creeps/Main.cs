using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnMobTimerTimeout()
	{
		Mob mob = (Mob)MobScene.Instantiate();
		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		Vector3 playerPosition = GetNode<Player>("Player").Position;
		mob.Initialize(mobSpawnLocation.Position, playerPosition);

		AddChild(mob);
	}

	public void OnPlayerHit()
	{
		GetNode<Timer>("MobTimer").Stop();
	}
}
