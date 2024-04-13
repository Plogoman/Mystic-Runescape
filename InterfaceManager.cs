using Godot;
using System;

public class InterfaceManager : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public static ProgressBar ManaBar;
    public static ProgressBar HealthBar;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ManaBar = GetNode("MainInterface/ManaBar") as ProgressBar;
        HealthBar = GetNode("MainInterface/HealthBar") as ProgressBar;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public static void UpdateHealth(float MaxHealth, float Health)
    {
        HealthBar.Value = (Health / MaxHealth) * HealthBar.MaxValue;
    }

    public static void UpdateMana(float MaxMana, float Mana)
    {
        ManaBar.Value = (Mana / MaxMana) * ManaBar.MaxValue;
    }
}
