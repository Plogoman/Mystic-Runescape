using Godot;

public class Adventurer : KinematicBody2D
{
	private const float Speed = 60.0f;
	private const float Gravity = 200.0f;
	private const float JumpVelocity = 200.0f;
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
	private AnimatedSprite animatedSprite;
	public int Health = 5;
	private int FacingDirection = 0;
	private bool isTakingDamage = false;

	[Signal]
	public delegate void Death();

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public override void _PhysicsProcess(float delta)
	{
		if (Health > 0)
		{
			if (!isDashing)
			{
				processMovement(delta);
			}

			if (IsOnFloor())
			{
				if (GetNode<RayCast2D>("RayCastDown").IsColliding() && Input.IsActionPressed("Down") &&
				    Input.IsActionJustPressed("Jump"))
				{
					Position = new Vector2(Position.x, Position.y + 2);
				}
				else if (Input.IsActionJustPressed("Jump"))
				{
					Velocity.y = -JumpVelocity;
					animatedSprite.Play("Jump");
					isInAir = true;
				}
				else
				{
					isInAir = false;
				}

				isDashAvailable = true;
			}

			processWallJump(delta);

			if (isDashAvailable)
			{
				processDash();
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
			else
			{
				Velocity.y += Gravity * delta;
			}

			MoveAndSlide(Velocity, Vector2.Up);
		}
	}

	private void processMovement(float delta)
	{
		FacingDirection = 0;
		if (!isTakingDamage)
		{
			if (Input.IsActionPressed("Left"))
			{
				FacingDirection -= 1;
				animatedSprite.FlipH = true;
			}

			if (Input.IsActionPressed("Right"))
			{
				FacingDirection += 1;
				animatedSprite.FlipH = false;
			}
		}

		if (FacingDirection != 0)
		{
			Velocity.x = Mathf.Lerp(Velocity.x, FacingDirection * Speed, Acceleration);
			if (!isInAir)
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

	private void processDash()
	{
		if (Input.IsActionJustPressed("Dash"))
		{
			if (Input.IsActionPressed("Left"))
			{
				Velocity.x = -DashSpeed;
				isDashing = true;
			}

			if (Input.IsActionPressed("Right"))
			{
				Velocity.x = DashSpeed;
				isDashing = true;
			}

			if (Input.IsActionPressed("Up"))
			{
				Velocity.y = -DashSpeed;
				isDashing = true;
			}

			if (Input.IsActionPressed("Right") && Input.IsActionPressed("Up"))
			{
				Velocity.x = DashSpeed;
				Velocity.y = -DashSpeed;
				isDashing = true;
			}

			if (Input.IsActionPressed("Left") && Input.IsActionPressed("Up"))
			{
				Velocity.x = -DashSpeed;
				Velocity.y = -DashSpeed;
				isDashing = true;
			}

			DashTimer = DashTimerReset;
			isDashAvailable = false;
		}
	}

	public void TakeDamage()
	{
		GD.Print("Adventurer has taken Damage");
		if (Health > 0)
		{
			Health -= 1;
			GD.Print("Current Health: " + Health);
			Velocity = MoveAndSlide(new Vector2(300f * -FacingDirection, -50), Vector2.Up);
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

	public void RespawnPlayer()
	{
		Show();
		Health = 5;
	}
}
