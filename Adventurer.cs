using System;
using Godot;
using MysticRunescape;
using System.Collections.Generic;
public class Adventurer : KinematicBody2D
{
	private const float Speed = 60.0f;
	private float Gravity = 500.0f;
	private const float JumpVelocity = 150.0f;
	private const float Friction = 0.1f;
	private const float Acceleration = 0.25f;
	private const float DashSpeed = 250.0f;
	private bool isDashing = false;
	private float DashTimer = 0.2f;
	private float DashTimerReset = 0.2f;
	private bool isDashAvailable = true;
	private bool isWallJumping = false;
	private float WallJumpingTimer = 0.2f;
	private float WallJumpingTimerReset = 0.2f;
	private Vector2 Velocity = new Vector2();
	private bool isInAir = false;
	[Export] public PackedScene GhostPlayerInstance;
	public AnimatedSprite animatedSprite;
	public float MaxHealth = 5;
	public float Health = 5;
	public int value;
	private Vector2 FacingDirection = new Vector2(0,0);
	private bool isTakingDamage = false;

	[Signal]
	public delegate void Death();

	public float Mana = 10f;

	public float MaxMana = 100f;

	public float ManaTimerReset = 2f;

	public float ManaTimer = 2f;
	
	public float switchSpellCooldown = 0.5f;
	public float switchSpellTimer = 0f;

	
	
	

	public List<Key> Keys = new List<Key>();
	public List<Key2> Keys2 = new List<Key2>();
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		GameManager.Player = this;
	}

	public override void _PhysicsProcess(float delta)
	{
		InterfaceManager.UpdateHealth(MaxHealth,Health);
		InterfaceManager.UpdateMana(MaxMana,Mana);
		if (Health > 0)
		{
			if (!isDashing)
			{
				processMovement(delta);
			}

			if (Input.IsActionJustPressed("PickUp"))
			{
				if (GetNode<RayCast2D>("RayCastLeft").IsColliding())
				{
					Node obj = (Node)GetNode<RayCast2D>("RayCastLeft").GetCollider();
					InteractWithItem(obj);
				}
				else if (GetNode<RayCast2D>("RayCastRight").IsColliding())
				{
					Node obj = (Node)GetNode<RayCast2D>("RayCastRight").GetCollider();
					InteractWithItem(obj);
				}
			}
			

			if (IsOnFloor())
			{
				if (GetNode<RayCast2D>("RayCastDown").IsColliding() && Input.IsActionPressed("Down") &&
				    Input.IsActionJustPressed("Jump"))
				{
					Position = new Vector2(Position.x, Position.y + 2);
					GetNode<AudioStreamPlayer>("jump").Play();
				}
				else if (Input.IsActionJustPressed("Jump"))
				{
					Velocity.y = -JumpVelocity;
					animatedSprite.Play("Jump");
					isInAir = true;
					GetNode<AudioStreamPlayer>("jump").Play();
				}
				else
				{
					isInAir = false;
				}

				isDashAvailable = true;
			}

			if (!IsOnFloor())
			{
				processWallJump(delta);
			}

			if (isDashing)
			{
				DashTimer -= delta;
				GhostPlayer ghost = GhostPlayerInstance.Instance() as GhostPlayer;
				Owner.AddChild(ghost);
				if (ghost != null)
					ghost.GlobalPosition = this.GlobalPosition;
				if (ghost != null)
					ghost.SetHValue(animatedSprite.FlipH);
				if (DashTimer <= 0)
				{
					isDashing = false;
					Velocity = new Vector2(0, 0);
				}
			}
			
			if(isInAir || !isDashing || !IsOnFloor())
			{
				Velocity.y += Gravity * delta;
			}
			

			if (Mana < 100 && ManaTimer <= 0)
			{
				UpdateMana(delta * 1);
				GD.Print(Mana);
			}
			else if(Mana != 100)
			{
				ManaTimer -= delta * 1;
			}

			if (Input.IsActionJustPressed("attack"))
			{
				if (GameManager.MagicController.currentCount == 0)
				{
					if (Mana >= 10)
					{
						attack();
					}
				}
				if (GameManager.MagicController.currentCount == 2)
				{
					if (Mana >= 70)
					{
						attack();
					}
				}
				if (GameManager.MagicController.currentCount == 1)
				{
					if (Mana >= 30)
					{
						attack();
					}
				}
			}

			if (switchSpellTimer > 0)
			{
				switchSpellTimer -= delta;
			}
			
			if (Input.IsActionPressed("switch_spell") && switchSpellTimer <= 0)
			{
				GameManager.MagicController.CycleSpell();
				
				switchSpellTimer = switchSpellCooldown;
			}

			MoveAndSlide(Velocity, Vector2.Up);
		}
	}

	private void attack()
	{
		GameManager.MagicController.CastSpell(GameManager.Player.GetNode<AnimatedSprite>("AnimatedSprite").FlipH);
	}

	private void InteractWithItem(Node obj)
	{
		if (obj.Owner is Pickupable)
		{
			if (obj.Owner is MagicPotion)
			{
				MagicPotion potion = obj.Owner as MagicPotion;
				potion.UsePotion();
			}
		}
	}

	private void processMovement(float delta)
	{
		FacingDirection = new Vector2(0, 0);
		if (!isTakingDamage)
		{
			if (Input.IsActionPressed("Left"))
			{
				FacingDirection.x -= 1;
				animatedSprite.FlipH = true;
				GetNode<AudioStreamPlayer>("run").Play();
			}

			if (Input.IsActionPressed("Right"))
			{
				FacingDirection.x += 1;
				animatedSprite.FlipH = false;
				GetNode<AudioStreamPlayer>("run").Play();
			}
			if (Input.IsActionPressed("Up"))
			{
				FacingDirection.y = -1;
			}
			
			if (Input.IsActionPressed("Down"))
			{
				FacingDirection.y = 1;
			}
			
			if (Input.IsActionJustPressed("Dash"))
			{
				if (isDashAvailable && Mana >= 10)
				{
					isDashing = true;
					DashTimer = DashTimerReset;
					isDashAvailable = false;
					UpdateMana(-10);
					GD.Print(Mana);
					ManaTimer = ManaTimerReset;
				}
			}

			if (Input.IsActionJustPressed("Escape"))
			{
				GetTree().ChangeScene("res://menu.tscn");
			}
		}

		if (FacingDirection.x != 0 || FacingDirection.y!=0)
		{
			if (isDashing)
			{
				Velocity.x = DashSpeed * FacingDirection.x;
				Velocity.y = DashSpeed * FacingDirection.y;
			}
			else
			{
				Velocity.x = Mathf.Lerp(Velocity.x, FacingDirection.x * Speed, Acceleration);
			}

			if (!isInAir && FacingDirection.x != 0)
				animatedSprite.Play("Run");
		}
		else
		{
			Velocity.x = Mathf.Lerp(Velocity.x, 0, Friction);
			if (Velocity.x < 5 && Velocity.x > -5)
			{
				if (!isInAir)
					animatedSprite.Play("Idle");
				isTakingDamage = false;
			}
		}
	}

	private void processWallJump(float delta)
	{
		if (Input.IsActionJustPressed("Jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding())
		{
			Velocity.y = -JumpVelocity;
			Velocity.x = JumpVelocity;
			animatedSprite.FlipH = false;
		}
		else if (Input.IsActionJustPressed("Jump") && GetNode<RayCast2D>("RayCastRight").IsColliding())
		{
			Velocity.y = -JumpVelocity;
			Velocity.x = -JumpVelocity;
			animatedSprite.FlipH = true;
		}
	}
	
	public void TakeDamage()
	{
		GD.Print("Adventurer has taken Damage");
		if (Health > 0)
		{
			Health -= 1;
			GD.Print("Current Health: " + Health);
			InterfaceManager.UpdateHealth(MaxHealth,Health);
			Velocity = MoveAndSlide(new Vector2(300f * -FacingDirection.x, -50), Vector2.Up);
			isTakingDamage = true;
			animatedSprite.Play("TakeDamage");
			if (Health <= 0)
			{
				Health = 0;
				animatedSprite.Play("Death");
				GD.Print("Player has Died");
			}
		}
	}
	
	private void _on_AnimatedSprite_animation_finished()
	{
		if (animatedSprite.Animation == "Death")
		{
			animatedSprite.Stop();
			Hide();
			GD.Print("Animation Finished");
			EmitSignal(nameof(Death));
		}
	}
	
	

	public void UpdateMana(float ManaAmount)
	{
		Mana += ManaAmount;
		if (Mana >= MaxMana)
		{
			Mana = MaxMana;
		}
		else if (Mana <= 0)
		{
			Mana = 0;
		}
	}
	public void UpdateHealth(float ManaAmount)
	{
		Health += ManaAmount;
		if (Health >= MaxMana)
		{
			Health = MaxHealth;
		}
		else if (Health <= 0)
		{
			Health = 0;
		}
	}

	
}
