using Godot;
using Godot.Collections;

public class Platform1 : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private Array movelocations;

    private Tween tween;

    private KinematicBody2D platform;
    
    private int index = 1;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        movelocations = GetNode<Node>("MovementPostion").GetChildren();
        tween = GetNode<Tween>("KinematicBody2D/Tween");
        platform = GetNode<KinematicBody2D>("KinematicBody2D");
        _on_Tween_tween_completed(null,null);
       
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void _on_Tween_tween_completed(object obj, NodePath path)
    {
        index += 1;
        if (index > movelocations.Count - 1)
        {
            index = 0;
        }
        Position2D node = movelocations[index] as Position2D;
        tween.InterpolateProperty(platform, "position", platform.Position, node.Position, 2,
            Tween.TransitionType.Linear, Tween.EaseType.InOut);
        tween.Start();
    }
}
