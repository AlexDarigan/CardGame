using Godot;
using System;
using CardGame;
using CardGame.Client;

public class RoomView : Node
{
    public int Id;
    public Label PlayerId { get; set; }
    public Label State;
    public TurnCounter TurnCounter;
    private HealthBar PlayerHealth;
    private HealthBar RivalHealth;
    public ChessClockButton ChessClockButton;
    Mouse mouse = new Mouse();

    public void OnAttackDeclared() { mouse.OnAttackDeclared(); }
    public void OnAttackCancelled() { mouse.OnAttackCancelled(); }
    
    public override void _Ready()
    {
        AddChild(mouse);

        PlayerId = GetNode<Label>("GUI/ID");
        PlayerId.Text = Id.ToString();
        State = GetNode<Label>("GUI/State");
        TurnCounter = GetNode<TurnCounter>("GUI/TurnCount");
        PlayerHealth = GetNode<HealthBar>("GUI/PlayerHealth");
        RivalHealth = GetNode<HealthBar>("GUI/RivalHealth");
        ChessClockButton = GetNode<ChessClockButton>("Table/ChessClockButton");
        Id = GetParent().CustomMultiplayer.GetNetworkUniqueId();
    }
    
    public void DisplayHealth(Participant player, Room room)
    {
        HealthBar healthBar = player is Player ? PlayerHealth : RivalHealth;
        healthBar.DisplayHealth(player, room);
    }
    
    private void OnButtonPressed(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
    {
        if (input is not InputEventMouseButton {Doubleclick: true}) return;
        Player player = (Player) GetParent<Room>().GetPlayer(true);
        player.EndTurn();
    }
}
