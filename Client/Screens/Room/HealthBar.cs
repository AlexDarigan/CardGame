using Godot;
using JetBrains.Annotations;

namespace CardGame.Client
{
	[UsedImplicitly]
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

		public void DisplayHealth(int newHealth, Participant player, Room room)
		{
			Change.Text = (Health - newHealth).ToString();
			Change.Visible = true;
			room.Effects.InterpolateCallback(Change, 0.4f, "set_visible", false);
			room.Effects.InterpolateProperty(this, nameof(Health), Health, _health, .5F,
				Tween.TransitionType.Linear, Tween.EaseType.In, .5F);
		}
	}
}
