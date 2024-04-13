using Godot;
using System;

public class IceKnife : Spell
{
    private AnimationPlayer Player;
    [Export] 
    public bool AbleToMove;
    public override void _Ready()
    {
        Player = GetNode<AnimationPlayer>("AnimationPlayer");
        Player.Play("Cast");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (AbleToMove)
        {
            if (!Player.IsPlaying())
            {
                Player.Play("Idle");
            }
            if (FaceDirection)
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
    
    public override void SetUp(bool faceDirection)
    {
        GetNode<Sprite>("Sprite").FlipH = faceDirection;
        FaceDirection = faceDirection;
    }

    public override void CastSpell()
    {
        //throw new NotImplementedException();
    }

    public override void LoadResourcePath()
    {
        //throw new NotImplementedException();
    }

    public void _on_Area2D_body_entered(object body)
    {
        Player.Play("Finish");
        if (body is SlimeEnemy)
        {
            SlimeEnemy Slime = body as SlimeEnemy;
            Slime.TakeDamage(DamageAmount);
        }
        QueueFree();
    }
}
