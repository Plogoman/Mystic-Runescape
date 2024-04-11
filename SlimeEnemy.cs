using Godot;
using System;

public class SlimeEnemy : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private AnimatedSprite Sprite;
    private RayCast2D BottomLeft;
    private RayCast2D BottomRight;
    private Vector2 Velocity;
    private const float Gravity = 200.0f;
    private const float Speed = 30.0f;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        BottomLeft = GetNode<RayCast2D>("RayCastLeft");
        BottomRight = GetNode<RayCast2D>("RayCastRight");
        Velocity.x = Speed;
    }


    public override void _PhysicsProcess(float delta)
    {
        Velocity.y += Gravity * delta;
        if (Velocity.y > Gravity)
        {
            Velocity.y = Gravity;
        }
        if (!BottomRight.IsColliding())
        {
            Velocity.x = -Speed;
            Sprite.FlipH = false;
        }
        else if (!BottomLeft.IsColliding())
        {
            Velocity.x = Speed;
            Sprite.FlipH = true;
        }
        if (!Sprite.Playing)
        {
            Sprite.Play("Jump");
        }
        MoveAndSlide(Velocity, Vector2.Up);
    }
    
    public void _on_Area2D_body_entered(object body)
    {
        GD.Print("Body: " + body + " has entered");
        if (body is KinematicBody2D)
        {
            if (body is Adventurer)
            {
                Adventurer PC = body as Adventurer;
                PC.TakeDamage();
            }
        }
    }
}
