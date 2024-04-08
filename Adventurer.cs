using Godot;
using Godot.Collections;

public class Adventurer : KinematicBody2D
{
	private const float Speed = 60.0f;
	private const float Gravity = 400.0f;
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
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}

	public override void _PhysicsProcess(float delta)
	{
		
		if (!isDashing)
		{
			processMovement(delta);
		}
		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("Jump"))
			{
				Velocity.y = -JumpVelocity;
				animatedSprite.Play("jump");
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

	private void processMovement(float delta)
	{
		int Direction = 0;
		if (Input.IsActionPressed("Left"))
		{
			Direction -= 1;
			animatedSprite.FlipH = true;
		}
		if (Input.IsActionPressed("Right"))
		{
			Direction += 1;
			animatedSprite.FlipH = false;
		}
		if (Direction != 0)
		{
			Velocity.x = Mathf.Lerp(Velocity.x, Direction * Speed, Acceleration);
			if (!isInAir)
				animatedSprite.Play("Run");
		}
		else
		{
			Velocity.x = Mathf.Lerp(Velocity.x, 0, Friction);
			if(!isInAir)
				animatedSprite.Play("Idle");
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
}
