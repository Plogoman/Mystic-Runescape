using Godot;
using System;

public class LevelTransition : Area2D
{
	[Export] public PackedScene NextLevel;
	[Export] public Position2D RespawnPoint;

	public delegate void LevelTransitionHandler(PackedScene nextLevel);
	public event LevelTransitionHandler levelTransition;

	public override void _Ready()
	{
		Connect("body_entered", this, nameof(_on_Body_Entered));
	}

	private void _on_Body_Entered(Node body)
	{
		if (body is Adventurer player)
		{
			levelTransition?.Invoke(NextLevel);
		}
	}
}