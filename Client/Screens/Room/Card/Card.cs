using System.Globalization;
using System.Runtime.InteropServices;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client
{
	
	[UsedImplicitly]
	public class Card: Spatial
	{
		public delegate void Pressed(Card pressed);
		public event Pressed CardPressed;
		
		public new Vector3 Translation { get => base.Translation; set => base.Translation = value; }
		public new Vector3 RotationDegrees { get => base.RotationDegrees; set => base.RotationDegrees = value; }

		public Participant Controller { get; set; }
		public Zone CurrentZone { get; set; }
		
		private Art _art;
		private Power _power;
		private CardType _cardType;
		
		public int Id { get; set;}
		public string Title { get; set; }
		public CardStates CardState { get; set; }
		public Factions Faction { get; set; }
		
		public Texture Art
		{
			get => _art.Value;
			set => _art.Value = value;
		}

		public int Power
		{
			get => _power.Value;
			set => _power.Value = value;
		}

		public string Text { get; set; }
		public CardTypes CardType { get => _cardType.Value; set => _cardType.Value = value; }
		public Location Location { get; set; }

		public Card() { }
		
		public override void _Ready()
		{
			_power = new Power(this);
			_art = new Art(this);
			_cardType = new CardType(this);
			GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent)); 
		}

		public void LookAt(Vector3 position) { base.LookAt(position, Vector3.Up); }
		
		public void OnInputEvent(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
		{
			if (input is not InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Doubleclick: true}) return;
			CardPressed?.Invoke(this);
		}
	}
}
