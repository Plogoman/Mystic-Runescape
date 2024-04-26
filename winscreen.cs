using Godot;
using System;

public class winscreen : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private AnimatedSprite Sprite;
    public static winscreen win;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
 


    private void _on_Area2D_body_entered(object body)
    {
        if (body is Adventurer )
        {
            Sprite.Visible = true;
            GetTree().ChangeScene("res://winmenu.tscn");
        }
    }
}
