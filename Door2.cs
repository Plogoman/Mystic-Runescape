using Godot;
using System;
using MysticRunescape;
public class Door2 : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    
    public string DoorKey;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }


    public void _on_Area2D_body_entered(object body)
    {
        if (body is Adventurer)
        {
            if (GameManager.Player.Keys2.FindAll(k => k.DoorToOpen2 == DoorKey).Count != 0)
            {
                Key2 k = GameManager.Player.Keys2.Find(x => x.DoorToOpen2.Contains(DoorKey));
                GameManager.Player.Keys2.Remove(k);
                k.QueueFree();
                GD.Print("open Door");
                GetNode<AnimationPlayer>("AnimationPlayer").Play("DoorOpen2");
            }
            else
            {
                GD.Print("Need Key");
            }
            
        }
    }
}
