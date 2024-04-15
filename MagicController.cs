using Godot;
using System;
using MysticRunescape;

public class MagicController : Node
{
    private PackedScene EquippedSpell = ResourceLoader.Load("res://miscs/IceKnife.tscn") as PackedScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void CastSpell(bool faceDirection)
    {
        
        Spell currentSpell = EquippedSpell.Instance() as Spell;
        currentSpell.SetUp(faceDirection);
        if (faceDirection)
        {
            currentSpell.GlobalPosition = GameManager.Player.GetNode<Position2D>("SpellCastLeft").GlobalPosition;

        }
        else
        {
            currentSpell.GlobalPosition = GameManager.Player.GetNode<Position2D>("SpellCastRight").GlobalPosition;

        }
        GameManager.GlobalGameManager.AddChild(currentSpell);
        GameManager.Player.UpdateMana(- currentSpell.ManaCost); 
    }




}
