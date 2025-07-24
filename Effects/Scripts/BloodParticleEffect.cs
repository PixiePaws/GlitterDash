using Godot;
using System;

namespace UnicornGame;
//This is the whole death/blood spray method. Call this when you want the huge blood splatter that leaves blood on terrain
public partial class BloodParticleEffect : CpuParticles2D
{
	[Export] public int _DropCount = 10;
	[Export] public float _SpraySpeed = 180f;
	[Export] public PackedScene _BloodDropScene;

	private CpuParticles2D _Particles;

	public void BloodSpray()
	{
		//Changes the particle property "Emitting" to true, which starts the particle effect.
		//It has a property called "OneShot" which makes it only run once.
		Emitting = true;

		for (int i = 0; i < _DropCount; i++)
		{
			SpawnBloodDrop();
		}
	}

	private void SpawnBloodDrop()
	{
		if (_BloodDropScene == null)
		{
			_BloodDropScene = GD.Load<PackedScene>("res://Effects/Scenes/CollisionDrops.tscn");
		}

		var Drop = _BloodDropScene.Instantiate<Node2D>();

		float Angle = (float)GD.RandRange(0, 2 * Math.PI);
		Vector2 Direction = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle)).Normalized();

		float Speed = (float)GD.RandRange(500, 650); //Same as particle effect blood, could change this to be dynamic but I am lazy lol
		Vector2 Velocity = Direction * Speed;

		// float Angle = (float)GD.RandRange(-Math.PI / 2 - 0.5f, -Math.PI / 2 + 0.5f);
		// Vector2 Dir = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle));
		// Vector2 Velocity = Dir * GD.Randf() * _SpraySpeed;

		Drop.Position = GlobalPosition;

		if (Drop is BloodDroplet bloodDrop)
		{
			bloodDrop._Velocity = Velocity;
		}

		GetTree().CurrentScene.AddChild(Drop);
	}
}