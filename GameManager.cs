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
		public float Gravity = 40f;
		public override void _Ready()
		{
			GlobalGameManager = this; 
			
			if(GlobalGameManager == null)
			{
				QueueFree();
			}
			MagicController = new MagicController();
			RespawnPoint = GetNode<Position2D>("RespawnPoint");
		}
		
		public void RespawnPlayer()
		{
			Player.Position = RespawnPoint.Position;
			Player.Show();
			Player.animatedSprite.Play("Idle");
			Player.Health = 5;
			Player.Mana = 100;
			InterfaceManager.UpdateMana(Player.MaxMana, Player.Mana);
			InterfaceManager.UpdateHealth(Player.MaxHealth, Player.Health);
		}

		private void _on_Area2D_body_entered(object body)
		{
			if (body is Adventurer)
			{
				RespawnPlayer();
			}
		}

		
		
		
		private void _on_Player_Death()
		{
			RespawnPlayer();
		}
		
	}
}
