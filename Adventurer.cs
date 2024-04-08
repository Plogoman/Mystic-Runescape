using Godot;
using Godot.Collections;

public class Adventurer : KinematicBody2D
{
	private const float Speed = 150.0f;
	private const float Gravity = 2500.0f;
	private const float JumpVelocity = 1000.0f;
	private const float Friction = 0.1f;
	private const float Acceleration = 0.25f;
	private const float DashSpeed = 1200.0f;
	private float DashTimer = 0.2f;
	private float DashTimerReset = 0.2f;
	private bool isDashAvailable = true;
	
	public override void _Ready()
	{
		
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector2 Velocity = new Vector2();
		int char;
		bool isDashing = false;
		if (!isDashing)
		{
			isDashing = false;
			int Direction = 0;
			if (Input.IsActionPressed("Left"))
			{
				Direction -= 1;
			}
			if (Input.IsActionPressed("Right"))
			{
				Direction += 1;
			}
			if (Direction != 0)
			{
				Velocity.x = Mathf.Lerp(Velocity.x, Direction * Speed, Acceleration);
			}
			else
			{
				Velocity.x = Mathf.Lerp(Velocity.x, 0, Friction);
			}
		}

		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("Jump"))
			{
				Velocity.y = -JumpVelocity;
			}
			isDashAvailable = true;
		}

		if (Input.IsActionJustPressed("Jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding())
		{
			Velocity.y = -JumpVelocity;
			Velocity.x = JumpVelocity;
		}
		else if (Input.IsActionJustPressed("Jump") && GetNode<RayCast2D>("RayCastRight").IsColliding())
		{
			Velocity.y = -JumpVelocity;
			Velocity.x = -JumpVelocity;			
		}
		
		if (isDashAvailable)
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
		
		if (isDashing)
		{
			DashTimer -= delta;
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
