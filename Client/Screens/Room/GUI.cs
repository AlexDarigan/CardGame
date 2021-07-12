using Godot;
using System;

public class GUI : Control
{
    public Label TurnCount { get; set; }
    public Label State { get; set; }
    public Label Id { get; set; }
    public Label GameOver { get; set; }
    private Timer Timer { get; set; }
    
    public override void _Ready()
    {
        TurnCount = GetNode<Label>("TurnCount");
        Id = GetNode<Label>("ID");
        State = GetNode<Label>("State");
        GameOver = GetNode<Label>("GameOver");
    }

}
