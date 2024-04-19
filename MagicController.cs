using Godot;
using System;
using System.Collections.Generic;
using MysticRunescape;

public class MagicController : Node
{
    public PackedScene EquippedSpell;
    public List<PackedScene> AvSpells = new List<PackedScene>();

    private int currentCount;
    // Called when the node enters the scene tree for the first time.
    public MagicController()
    {
        IceKnife iceKnife = new IceKnife();
        AvSpells.Add(ResourceLoader.Load(iceKnife.ResourcePath) as PackedScene);
        EquippedSpell = AvSpells[0];
    }

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

    public void CycleSpell()
    {
        currentCount += 1;
        if (AvSpells.Count -1 < currentCount)
        {
            currentCount = 0;
        }
        EquippedSpell = AvSpells[currentCount];
    }



}
