using Godot;
using System;

public abstract class Spell : Node2D
{
    private string ResourcePath;
    [Export]
    public int DamageAmount;
    [Export]
    public int Speed;
    [Export]
    public float ManaCost;
    public abstract void CastSpell();
    public abstract void LoadResourcePath();
    public abstract void SetUp();
}