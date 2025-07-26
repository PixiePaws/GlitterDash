using Godot;
using System;

//This handles the logic for individual blood droplets aka the Node2D ones that check for collision
public partial class BloodDroplet : Node2D
{
	[Export] public float _Gravity = 980f; // Gravity applied to the blood droplet
	//[Export] public Vector2 _Velocity = new Vector2(80, -150); // Innitial velocity
	[Export] public float _AccelMin = 50f;
	[Export] public float _AccelMax = 100f;
	[Export] public float _Damping = 100f;
	[Export] public float _RayLength = 1f;

	public Vector2 _Velocity = Vector2.Zero; //Gets a value from the SpawnBloodDrop method in BloodParticleEffect.cs
	private RayCast2D _RayCast;
	private float _LifeTimer = 0f;
	private const float _MaxLife = 2f;
	private float _LinearAccel;

	//Optimazation stuff
	private int _FrameCounter = 0;

	public override void _Ready()
	{
		_LinearAccel = (float)GD.RandRange(_AccelMin, _AccelMax);

		_RayCast = GetNode<RayCast2D>("RayCast2D");
		_RayCast.Enabled = true;

		//Optimization removes the collision droplets if they leave the screen
		var Notifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		Notifier.ScreenExited += OnScreenExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		_FrameCounter++;

		//Max amount of time a droplet can exist <-- Optimization
		_LifeTimer += (float)delta;
		if (_LifeTimer > _MaxLife)
		{
			QueueFree();
			return;
		}

		float dt = (float)delta; //Delta time 

		//Acceleration
		_Velocity += _Velocity.Normalized() * _LinearAccel * dt;

		//Gravity down
		_Velocity += new Vector2(0, _Gravity) * dt;

		_Velocity = _Velocity.MoveToward(Vector2.Zero, _Damping * dt);

		Position += _Velocity * dt;

		if (_Velocity.Length() > 0.1f)
		{
			_RayCast.TargetPosition = _Velocity.Normalized() * _RayLength;
		}

		_RayCast.ForceRaycastUpdate(); // Manual update for raycast

		if (_FrameCounter % 2 == 0)
		{
			if (_RayCast.IsColliding())
			{
				Vector2 ImpactPoint = _RayCast.GetCollisionPoint();
				//GD.Print("hit at ", ImpactPoint, " existed for ", _LifeTimer); <-- Debugging
				SpawnDecal(ImpactPoint);
				QueueFree();
			}
		}
	}
	private void OnScreenExited()
	{
		QueueFree();
	}

	private void SpawnDecal(Vector2 position)
	{
		BloodDecalManager.Instance.AddDecal(position);
	}


}
