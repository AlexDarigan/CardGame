using System;
using CardGame.Server;
using Godot;
using JetBrains.Annotations;
using Object = Godot.Object;

namespace CardGame.Client
{
	public class CardView: Spatial { }
	
	public class Card: Object
	{
		[Signal] public delegate void OnCardEntered();
		[Signal] public delegate void OnCardExited();
		private readonly CardView _view;
		private readonly SpatialMaterial _face;
		public readonly int Id;
		private string _title;
		private string _text;
		private int _power;
		private CardType _cardType;
		public CardState CardState = CardState.None;
		private Texture Art { set { _face.AlbedoTexture = value; _face.EmissionTexture = value; } }
		public Vector3 Translation { get => _view.Translation; set => _view.Translation = value; }
		public Vector3 RotationDegrees { get => _view.RotationDegrees; set => _view.RotationDegrees = value; }

		public Card(CardInfo info, CardView view, int id)
		{
			Id = id;
			_view = view;
			_face = (SpatialMaterial) view.GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
			(_cardType, _title, Art, _text, _power) = info;
			_view.GetNode<Area>("Area").Connect("mouse_entered", this, nameof(OnMouseEntered));
			_view.GetNode<Area>("Area").Connect("mouse_exited", this, nameof(OnMouseEntered));
			_view.GetNode<Spatial>("Power").Visible = _cardType == CardType.Unit;
		}

		public void Update(CardState state) =>	CardState = state;
		public void OnMouseEntered() => EmitSignal(nameof(OnCardEntered), this);
		public void OnMouseExited() => EmitSignal(nameof(OnCardExited), this);

	}
}
