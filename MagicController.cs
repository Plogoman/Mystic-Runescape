using Godot;
using System;
using MysticRunescape;

public class MagicController : Node
{
    public PackedScene EquippedSpell = ResourceLoader.Load("res://Spells/IceKnife.tscn") as PackedScene;
    public override void _Ready()
    {
        
    }

    public void CastSpell(bool FaceDirection)
    {
        Spell CurrentSpell = EquippedSpell.Instance() as Spell;
        CurrentSpell.SetUp(FaceDirection);
        if (FaceDirection)
        {
            CurrentSpell.GlobalPosition = GameManager.Player.GetNode<Position2D>("SpellCastLeft").GlobalPosition;
        }
        else
        {
            CurrentSpell.GlobalPosition = GameManager.Player.GetNode<Position2D>("SpellCastRight").GlobalPosition;
        }
        GameManager.GlobalGameManager.AddChild(CurrentSpell);
        GameManager.Player.UpdateMana(-CurrentSpell.ManaCost);
    }
}
