using Godot;
using System;
using MysticRunescape;



public class IceKnife : Spell
{
    public string ResourcePath = "res://miscs/IceKnife.tscn";
    
    private AnimationPlayer player;
    [Export] public bool ableToMove;

    public override void _Ready()
    {
        player = GetNode<AnimationPlayer>("AnimationPlayer");
        player.Play("cast");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (ableToMove)
        {
            if (!player.IsPlaying())
            {
                player.Play("idle");
            }


            if (faceDirection)
            {
                Position -= (Transform.x * delta * Speed);
            }
            else
            {
                Position += (Transform.x * delta * Speed);
            }

            LifeSpan -= delta;
            if (LifeSpan < 0)
            {
                QueueFree();
            }
        }
    }

    public override void SetUp(bool facedirection)
    {
        GetNode<Sprite>("Sprite").FlipH = facedirection;
        faceDirection = facedirection;
    }

    public override void CastSpell()
    {
        
    }
    
    public override void LoadResourcePath()
    {
        
    }

    public void _on_Area2D_body_entered(object body)
    {
        if (body is SlimeEnemy)
        {
            SlimeEnemy slime = body as SlimeEnemy;
            slime.TakeDamage(DamageAmount);
            player.Play("finish");
        }
        if (!faceDirection)
        {
            player.Play("finish");
        }
        else
        {
            if (body is TileMap)
            {
                player.Play("finish");
            }
            else if (body is Door)
            {
                player.Play("finish");
            }
            else if (body is Door2)
            {
                player.Play("finish");
            }
            else if (body is Platform1)
            {
                player.Play("finish");
            }
            else
            {
                if (LifeSpan < 0)
                {
                    player.Play("finish");
                }
            }
        }
    }

    
    
}
