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
			RespawnPoint = GetNode<Position2D>("RespawnPoint");
		}
		
		public void RespawnPlayer()
		{
			Player.Position = RespawnPoint.Position;
			Player.Show();
			Player.animatedSprite.Play("Idle");
			Player.Health = 5;
			InterfaceManager.UpdateHealth(Player.MaxHealth, Player.Health);
			InterfaceManager.UpdateMana(Player.MaxHealth, Player.Mana);
		}

		private void _on_Player_Death()
		{
			RespawnPlayer();
		}
		
	}
}
