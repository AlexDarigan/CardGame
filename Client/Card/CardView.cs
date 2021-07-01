using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
	public class Card: Object
	{
		public delegate void Pressed(Card pressed);
		public event Pressed CardPressed;
		private readonly Spatial view;
		private readonly SpatialMaterial _face;
		public readonly int Id;
		private string _title;
		private string _text;
		private int _power;
		public CardState CardState = CardState.None;
		private Texture Art { set { _face.AlbedoTexture = value; _face.EmissionTexture = value; } }
		public Vector3 Translation { get => view.Translation; set => view.Translation = value; }
		public Vector3 RotationDegrees { get => view.RotationDegrees; set => view.RotationDegrees = value; }

		public Card(CardInfo info, Spatial view, int id)
		{
			Id = id;
			this.view = view;
			_face = (SpatialMaterial) view.GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
			CardType cardType;
			(cardType, _title, Art, _text, _power) = info;
			this.view.GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent));
			this.view.GetNode<Spatial>("Power").Visible = cardType == CardType.Unit;
		}

		public void Update(CardState state) =>	CardState = state;

		public void OnInputEvent(Node camera, InputEvent input, Vector3 click_pos, Vector3 click_normal, int shapeIdx)
		{
			if (input is InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Doubleclick: true}) { CardPressed?.Invoke(this); }
		}
	

	}
}
