using System.Globalization;
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
		public Location CurrentLocation { get; set; }
		public Zone CurrentZone { get; set; }
		
		public int Id { get; set;}
		public string Title { get; set; }
		public CardStates CardState { get; set; }
		public Factions Faction { get; set; }

		public CardTypes CardType
		{
			get => _cardType;
			set
			{
				_cardType = value;
				_powerDisplay.Visible = value == CardTypes.Unit;
			}
		}

		public Texture Art
		{
			set
			{
				_face.AlbedoTexture = value;
				_face.EmissionTexture = value;
			}
		}

		public int Power 
		{ 
			get => _power;
			set
			{
				_power = value;
				if (CardType != CardTypes.Unit) return;
				string power = value.ToString(CultureInfo.InvariantCulture);
				for (int i = 0; i < 4; i++)
				{
					_powerDisplay.GetChild<Sprite3D>(i).Texture =
						GD.Load<Texture>($"res://Client/Assets/Numbers/Impact/{power[i]}.png");
				}
			}
		}
		public string Text { get; set; }

		private CardTypes _cardType;
		private SpatialMaterial _face;
		private Spatial _powerDisplay;
		private int _power;
		
		public Card() { }
		
		public override void _Ready() 
		{ 
			_face = (SpatialMaterial) GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
			_powerDisplay = GetNode<Spatial>("Power");
			GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent)); 
		}

		public void Move(Room room, Zone destination)
		{
			Zone origin = CurrentZone;
			origin.Remove(this);
			destination.Add(this);
			destination.Update(room);
			origin.Update(room);
		}
		
		public void LookAt(Vector3 position) { base.LookAt(position, Vector3.Up); }
		
		public void OnInputEvent(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
		{
			if (input is not InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Doubleclick: true}) return;
			CardPressed?.Invoke(this);
		}
	}
}
