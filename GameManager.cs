using Godot;

namespace MysticRunescape
{
	public class GameManager : Node2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		[Export] public Position2D RespawnPoint;
		// Called when the node enters the scene tree for the first time.
		public static GameManager GlobalGameManager;
		public static Adventurer Player;
		public static MagicController MagicController;
		public LevelTransition CurrentLevelTransition;
		public PackedScene CurrentLevel;
		
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
			
			CurrentLevelTransition = GetNode<LevelTransition>("LevelTransition");
			CurrentLevelTransition.levelTransition += TransitionToNextLevel;
			RespawnPoint = GetNode<Position2D>("PlayerSpawnPoint");
		}

		public void LoadInitialLevel()
		{
			CurrentLevel = ResourceLoader.Load<PackedScene>("res://Levels/Level1.tscn");
			LoadLevel(CurrentLevel);
		}

		public void LoadLevel(PackedScene Level)
		{
			if (GetTree().CurrentScene is Node2D currentScene)
			{
				currentScene.QueueFree();
			}

			var NewLevelInstance = (Node2D)Level.Instance();
			GetTree().Root.AddChild(NewLevelInstance);
			GetTree().CurrentScene = NewLevelInstance;

			CurrentLevelTransition = NewLevelInstance.GetNode<LevelTransition>("LevelTransition");
			CurrentLevelTransition.Connect("level_transition", this, nameof(TransitionToNextLevel));
		}
		
		public void RespawnPlayer()
		{
			Player.Position = CurrentLevelTransition.RespawnPoint.GlobalPosition;
			Player.Show();
			Player.animatedSprite.Play("Idle");
			Player.Health = Player.MaxHealth;
			InterfaceManager.UpdateHealth(Player.MaxHealth, Player.Health);
			InterfaceManager.UpdateMana(Player.MaxHealth, Player.Mana);
		}

		private void _on_Player_Death()
		{
			RespawnPlayer();
		}

		public void TransitionToNextLevel(PackedScene NextLevel)
		{
			CurrentLevel = NextLevel;
			LoadLevel(CurrentLevel);
		}
	}
}
