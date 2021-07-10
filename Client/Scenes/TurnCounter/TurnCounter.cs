using Godot;
using System;


public class TurnCounter : Label
{
    private int _count = 5;
    private int Count
    {
        get => _count;
        set
        {
            _count = value;
            Text = value.ToString();
        }
    }
    
    public void NextTurn() { Count++; }
    public override void _Ready() { Text = Count.ToString(); }
}
