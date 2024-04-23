// GameManager.cs
using Godot;
using System;

public class GameManager : Node2D
{
    public static GameManager GlobalGameManager;
    public static Adventurer Player;
    public static MagicController MagicController;
    private Area2D currentLevelTransition;
    private PackedScene currentLevel;

    public override void _Ready()
    {
        if (GlobalGameManager == null)
        {
            GlobalGameManager = this;
        }
        else
        {
            QueueFree();
        }

        MagicController = new MagicController();
        LoadInitialLevel();
    }

    private void LoadInitialLevel()
    {
        // Load the first level scene
        currentLevel = ResourceLoader.Load<PackedScene>("res://Levels/Level1.tscn");
        LoadLevel(currentLevel);
    }

    public void LoadLevel(PackedScene level)
    {
        // Unload the current level, if any
        if (GetTree().CurrentScene is Node2D currentScene)
        {
            currentScene.QueueFree();
        }

        // Load the new level
        var newLevelInstance = (Node2D)level.Instance();
        GetTree().Root.CallDeferred("add_child", newLevelInstance);
        GetTree().CurrentScene = newLevelInstance;

        // Get a reference to the LevelTransition node
        currentLevelTransition = newLevelInstance.GetNode<Area2D>("LevelTransition");
        currentLevelTransition.Connect("LevelTransition", this, nameof(TransitionToNextLevel));
    }

    private void TransitionToNextLevel(PackedScene nextLevel)
    {
        currentLevel = nextLevel;
        LoadLevel(currentLevel);
    }

    public void RespawnPlayer()
    {
        Player.Position = currentLevelTransition.GetNode<Position2D>("RespawnPoint").GlobalPosition;
        Player.Show();
        Player.animatedSprite.Play("Idle");
        Player.Health = Player.MaxHealth;
        InterfaceManager.UpdateHealth(Player.MaxHealth, Player.Health);
        InterfaceManager.UpdateMana(Player.MaxMana, Player.Mana);
    }

    private void _on_Player_Death()
    {
        RespawnPlayer();
    }
}