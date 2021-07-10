using Godot;
using System;
using CardGame;
using CardGame.Client;

public class RoomView : Node
{
    public event Action PassPlayPressed;
    public event Action EndTurnPressed;
    
    //public int turnCount = 1;
    public int Id;
    public Label PlayerId { get; set; }
    public Label State;
    public Label TurnCount;
    private ProgressBar PlayerHealthBar;
    private ProgressBar OpponentHealthBar;
    private Label PlayerHealthLabel;
    private Label OpponentHealthLabel;
    private MeshInstance Button;
    private int _playerHealth = 8000;
    private int _opponentHealth = 8000;
    private Label LifeChange;

    private int PlayerHealth
    {
        get => _playerHealth;
        set
        {
            _playerHealth = value;
            PlayerHealthLabel.Text = value.ToString();
        }
    }

    private int OpponentHealth
    {
        get => _opponentHealth;
        set
        {
            _opponentHealth = value;
            OpponentHealthLabel.Text = value.ToString();
        }
    }

    
    public override void _Ready()
    {
        Console.WriteLine("Room View Created");
        //Control gui = GetNode<Control>("GUI");
        PlayerId = GetNode<Label>("GUI/ID");
        PlayerId.Text = Id.ToString();
        State = GetNode<Label>("GUI/State");
        TurnCount = GetNode<Label>("GUI/TurnCount");
        PlayerHealthBar = GetNode<ProgressBar>("GUI/PlayerHealth/PanelContainer/ProgressBar");
        OpponentHealthBar = GetNode<ProgressBar>("GUI/Rival/PanelContainer/ProgressBar");
        PlayerHealthLabel = GetNode<Label>("GUI/PlayerHealth/PanelContainer/Label");
        OpponentHealthLabel = GetNode<Label>("GUI/Rival/PanelContainer/Label");
        Button = GetNode<MeshInstance>("Table/Button");
        GetNode<Area>("Table/Button/Area").Connect("input_event", this, "OnButtonPressed");
        LifeChange = GetNode<Label>("GUI/LifeChange");
    }

    public void OnGameUpdated(Room room, States state)
    {
        State.Text = state.ToString();
        SpatialMaterial mat = (SpatialMaterial) Button.GetSurfaceMaterial(0);
        mat.AlbedoColor = state == States.IdleTurnPlayer ? Colors.Aqua : Colors.Red;
    }

    public void DisplayHealth(Participant player, Room room)
    {
        if (player is Player)
        {
            LifeChange.Text = ((int)PlayerHealthBar.Value - player.Health).ToString();
            room.Gfx.InterpolateCallback(LifeChange, 0.01f, "set_visible", true);
            room.Gfx.InterpolateCallback(LifeChange, 0.4f, "set_visible", false);
            room.Gfx.InterpolateProperty(PlayerHealthBar, "value", PlayerHealthBar.Value, player.Health, .5f, 
                Tween.TransitionType.Back, Tween.EaseType.In, .5F);
            room.Gfx.InterpolateProperty(this, nameof(PlayerHealth), PlayerHealth, player.Health, .5F,
            Tween.TransitionType.Back, Tween.EaseType.In, .5F);
           
        }
        else
        {
            LifeChange.Text = ((int)OpponentHealthBar.Value - player.Health).ToString();
            room.Gfx.InterpolateCallback(LifeChange, 0.01f, "set_visible", true);
            room.Gfx.InterpolateCallback(LifeChange, 0.4f, "set_visible", false);
            room.Gfx.InterpolateProperty(OpponentHealthBar, "value", OpponentHealthBar.Value, player.Health, .5f, 
                Tween.TransitionType.Back, Tween.EaseType.In, .5F);
            room.Gfx.InterpolateProperty(this, nameof(OpponentHealth), OpponentHealth, player.Health, .5F,
                Tween.TransitionType.Back, Tween.EaseType.In, .5F);
        }
    }

    public string SetPHealth()
    {
        return PlayerHealth.ToString();
    }
    
    public void AddTurn()
    {
        
    }

    private void OnButtonPressed(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
    {
        if (input is InputEventMouseButton {Doubleclick: true})
        {
            EndTurnPressed?.Invoke();
        }
    }
}
