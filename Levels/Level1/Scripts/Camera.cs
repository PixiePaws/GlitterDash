using Godot;
using System;

public partial class Camera : Camera2D
{

	[Export]
	public Node2D Player;

	private Vector2I size;

	/*
	public override void _Ready()
	{
		size = (Vector2I)GetViewportRect().Size;
		UpdateCameraPosition();
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateCameraPosition();
	}

	private void UpdateCameraPosition()
	{
		Vector2 currentCell = (Vector2)(Player.GlobalPosition / size);
		GlobalPosition = currentCell * size;
	} */
}
