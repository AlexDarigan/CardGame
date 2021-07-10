using Godot;
using CardGame.Client;

public class HealthBar : HBoxContainer
{
    private Label Change { get; set; }
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
   
    public override void _Ready()
    {
        Change = GetNode<Label>("Node/Change");
        Label = GetNode<Label>("PanelContainer/Label");
        Bar = GetNode<ProgressBar>("PanelContainer/ProgressBar");
    }

    public void DisplayHealth(Participant player, Room room)
    {
        Change.Text = (Health - player.Health).ToString();
        room.Gfx.InterpolateCallback(Change, 0.01f, "set_visible", true);
        room.Gfx.InterpolateCallback(Change, 0.4f, "set_visible", false);
        room.Gfx.InterpolateProperty(this, nameof(Health), Health, player.Health, .5F,
            Tween.TransitionType.Back, Tween.EaseType.In, .5F);
    }
}
