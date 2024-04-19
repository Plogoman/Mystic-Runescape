using Godot;

namespace MysticRunescape
{
	public class GameManager : Node2D
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";
		[Export]
		public Position2D RespawnPoint;
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
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
		public void RespawnPlayer()
		{
		Adventurer ad = GetNode<Adventurer>("Player");
		ad.GlobalPosition = RespawnPoint.GlobalPosition;
		if (ad.GlobalPosition != RespawnPoint.GlobalPosition)
		{
			ad.GlobalPosition = RespawnPoint.GlobalPosition;
		}
		ad.RespawnPlayer();
		}

		private void _on_Player_Death()
		{
			RespawnPlayer();
		}
		
	}
}
