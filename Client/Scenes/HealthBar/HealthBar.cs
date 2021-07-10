using Godot;
using System;
using CardGame.Client;

public class HealthBar : HBoxContainer
{
    private Label Label { get; set; }
    private ProgressBar Bar { get; set; }
    private int _health = 8000;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            Bar.Value = value;
            Label.Text = value.ToString();
        }
    }
    public HealthBar(){}

    public override void _Ready()
    {
        Label = GetNode<Label>("PanelContainer/Label");
        Bar = GetNode<ProgressBar>("PanelContainer/ProgressBar");
    }

    public void DisplayHealth(Participant player, Room room)
    {
        // LifeChange.Text = ((int)OpponentHealthBar.Value - player.Health).ToString();
        // room.Gfx.InterpolateCallback(LifeChange, 0.01f, "set_visible", true);
        // room.Gfx.InterpolateCallback(LifeChange, 0.4f, "set_visible", false);
        // room.Gfx.InterpolateProperty(Bar, "value", Bar.Value, player.Health, .5f, 
        //     Tween.TransitionType.Back, Tween.EaseType.In, .5F);
        
        room.Gfx.InterpolateProperty(this, nameof(Health), Health, player.Health, .5F,
            Tween.TransitionType.Back, Tween.EaseType.In, .5F);
    }
}
