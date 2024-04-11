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
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
