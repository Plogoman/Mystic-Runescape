using Godot;
using System;

public class pause_menu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    private void _on_resume_pressed()
    {
        GetTree().Paused = false;
        Hide();
    }

    private void _on_restart_pressed()
    {
        GetTree().ChangeScene("res://First.tscn");
    }

    private void _on_quit_pressed()
    {
        GetTree().ChangeScene("res://menu.tscn");
    }
}