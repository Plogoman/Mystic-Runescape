using Godot;
using System;

namespace MysticRunescape
{
    public class ArcherEnemy : Node2D
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        private Adventurer player;

        private bool active = false;
        
        private bool abletoshoot = true;

        private float shootTimer = 1f;

        private float shootTimerReset = 1f;

        public bool IsShooting = false;

        private AnimatedSprite animatedSprite;
        
        private int health;

        [Export] 
        public PackedScene arrow;
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
          public override void _Process(float delta)
          {
              if (active)
              {
                  var angle = GlobalPosition.AngleToPoint(player.GlobalPosition);
                  if (Math.Abs(angle) > Mathf.Pi / 2)
                  {
                      animatedSprite.FlipH = false;
                  }
                  else
                  {
                      animatedSprite.FlipH = true;

                  }
                  if (abletoshoot)
                  {
                      var spaceState = GetWorld2d().DirectSpaceState;
                      Godot.Collections.Dictionary result = spaceState.IntersectRay(this.Position, player.Position,
                          new Godot.Collections.Array { this });
                      if (result != null)
                      {
                          if (result.Contains("collider"))
                          {
                              this.GetNode<Position2D>("ProjectileSpawn").LookAt(player.Position);
                              if (result["collider"] == player)
                              {
                                  animatedSprite.Play("drawback");
                                  IsShooting = true;
                              }
                          }
                      }
                  }
                  else
                  {
                      if (!IsShooting)
                      {
                          animatedSprite.Play("Idle");
                      }
                  }
              }

              if (shootTimer <= 0)
              {
                  abletoshoot = true;
              }
              else
              {
                  shootTimer -= delta;
              }
          }


        private void _on_Detection_Radius_body_entered(object body)
        {
            GD.Print("body has entered" + body);
            if (body is Adventurer)
            {
                player = body as Adventurer;
                active = true;
            }
        }

    

        private void _on_Detection_Radius_body_exited(object body)
        {
            GD.Print("body has exited" + body);
            if (body is Adventurer )
            {
                active = false;
            }
        }

        private void _on_AnimatedSprite_animation_finished()
        {
            if (animatedSprite.Animation == "drawback")
            {
                animatedSprite.Play("shoot");
                ShootAtPlayer();
            }

            if (animatedSprite.Animation == "shoot")
            {
                IsShooting = false;
            }
        }

        private void ShootAtPlayer()
        {
            GD.Print("shooting");
            Arrow arrow = this.arrow.Instance() as Arrow;
            Owner.AddChild(arrow);
            arrow.GlobalTransform = this.GetNode<Position2D>("ProjectileSpawn").GlobalTransform;
            abletoshoot = false;
            shootTimer = shootTimerReset;
        }
        
        
    }
}
