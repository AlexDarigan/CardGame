using System;
using Godot;
using JetBrains.Annotations;

[UsedImplicitly]
public class RoomView : Node
{
    public event Action RivalHeartPressed; 
    private int Id { get; set; }
    private Label PlayerId { get; set; }
    public Label State;
    public TurnCounter TurnCounter;
    public HealthBar PlayerHealth;
    public HealthBar RivalHealth;
    public ChessClockButton ChessClockButton;
    private Mouse Mouse { get; }= new Mouse();

    public void OnAttackDeclared() { Mouse.OnAttackDeclared(); }
    public void OnAttackCancelled() { Mouse.OnAttackCancelled(); }
    
    public override void _Ready()
    {
        AddChild(Mouse);
        PlayerId = GetNode<Label>("GUI/ID");
        State = GetNode<Label>("GUI/State");
        TurnCounter = GetNode<TurnCounter>("GUI/TurnCount");
        PlayerHealth = GetNode<HealthBar>("GUI/PlayerHealth");
        RivalHealth = GetNode<HealthBar>("GUI/RivalHealth");
        ChessClockButton = GetNode<ChessClockButton>("Table/ChessClockButton");
        Id = GetParent().CustomMultiplayer.GetNetworkUniqueId();
        PlayerId.Text = Id.ToString();

        GetNode<Button>("RivalHeart").Connect("pressed", this, "OnRivalHeartPressed");
    }

    public void OnRivalHeartPressed() { RivalHeartPressed?.Invoke(); }
    
  
}
