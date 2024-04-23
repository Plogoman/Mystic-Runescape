using Godot;
using System;

public abstract class LevelTransition : Area2D
{
    [Export] public PackedScene NextLevel;
    [Export] public Position2D RespawnPoint;

    public delegate void LevelTransitionHandler(PackedScene NextLevel);
    public event LevelTransitionHandler levelTransition;
    
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(_on_Body_Entered));
    }

    private void _on_Body_Entered(object body)
    {
        if (body is Adventurer)
        {
            levelTransition?.Invoke(NextLevel);
        }
    }
}
