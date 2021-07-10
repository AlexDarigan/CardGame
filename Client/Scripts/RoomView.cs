using Godot;
using System;
using CardGame;
using CardGame.Client;

public class RoomView : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public event Action PassPlayPressed;
    public event Action EndTurnPressed;
    //public int turnCount = 1;
    public int Id;
    public Label PlayerId { get; set; }
    public Label State;
    public Label TurnCount; 
    private ProgressBar PlayerHealthBar; 
    private ProgressBar OpponentHealthBar;
    private MeshInstance Button; 
    
    public override void _Ready()
    {
        Control gui = GetNode<Control>("GUI");
        PlayerId = gui.GetNode<Label>("ID");
        PlayerId.Text = Id.ToString();
        State = gui.GetNode<Label>("State");
        TurnCount = gui.GetNode<Label>("TurnCount");
        PlayerHealthBar = gui.GetNode<ProgressBar>("PlayerHealth/PanelContainer/ProgressBar");
        PlayerHealthBar = gui.GetNode<ProgressBar>("Rival/PanelContainer/ProgressBar");
        Button = GetNode<MeshInstance>("Table/Button");
        GetNode<Area>("Table/Button/Area").Connect("input_event", this, "OnButtonPressed");
    }

    public void OnGameUpdated(Room room, States state)
    {
        State.Text = state.ToString();
        SpatialMaterial mat = (SpatialMaterial) Button.GetSurfaceMaterial(0);
        mat.AlbedoColor = state == States.IdleTurnPlayer ? Colors.Aqua : Colors.Red;
    }
    
    public void AddTurn()
    {
        // This only works when we're ending, it doesn't increase when we're beginning
        // turnCount += 1;
        // TurnCount.Text = turnCount.ToString();
    }

    private void OnButtonPressed(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
    {
        if (input is InputEventMouseButton {Doubleclick: true})
        {
            EndTurnPressed?.Invoke();
        }
    }
}
