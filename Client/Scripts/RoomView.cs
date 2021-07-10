using Godot;
using System;
using CardGame;
using CardGame.Client;

public class RoomView : Node
{
    public int Id;
    public Label PlayerId { get; set; }
    public Label State;
    public Label TurnCount;
    private HealthBar PlayerHealth;
    private HealthBar RivalHealth;
    private MeshInstance Button;
    Mouse mouse = new Mouse();

    public void OnAttackDeclared()
    {
        mouse.OnAttackDeclared();
    }

    public void OnAttackCancelled()
    {
        mouse.OnAttackCancelled();
    }
    
    public override void _Ready()
    {
        AddChild(mouse);

        PlayerId = GetNode<Label>("GUI/ID");
        PlayerId.Text = Id.ToString();
        State = GetNode<Label>("GUI/State");
        TurnCount = GetNode<Label>("GUI/TurnCount");

        PlayerHealth = GetNode<HealthBar>("GUI/PlayerHealth");
        RivalHealth = GetNode<HealthBar>("GUI/RivalHealth");
        
        Button = GetNode<MeshInstance>("Table/Button");
        GetNode<Area>("Table/Button/Area").Connect("input_event", this, "OnButtonPressed");
        Id = GetParent().CustomMultiplayer.GetNetworkUniqueId();
    }

    public void OnGameUpdated(Room room, States state)
    {
        State.Text = state.ToString();
        SpatialMaterial mat = (SpatialMaterial) Button.GetSurfaceMaterial(0);
        mat.AlbedoColor = state == States.IdleTurnPlayer ? Colors.Aqua : Colors.Red;
    }

    public void DisplayHealth(Participant player, Room room)
    {
        HealthBar healthBar = player is Player ? PlayerHealth : RivalHealth;
        healthBar.DisplayHealth(player, room);
    }


    public void AddTurn() { }

    private void OnButtonPressed(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
    {
        if (input is InputEventMouseButton {Doubleclick: true})
        {
            Player player = (Player) GetParent<Room>().GetPlayer(true);
            player.EndTurn();
        }
    }
}
