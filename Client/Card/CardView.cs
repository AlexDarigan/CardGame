using System;
using CardGame.Server;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
	public class CardView: Spatial { }
	
	public class Card: Object
	{
		[Signal] public delegate void OnCardEntered();
		[Signal] public delegate void OnCardExited();
		private readonly CardView View;
		private readonly SpatialMaterial _face;
		public readonly int Id;
		public string Title;
		public string Text;
		public int Power;
		public CardType CardType;
		public Texture Art { set { _face.AlbedoTexture = value; _face.EmissionTexture = value; } }
		public Vector3 Translation { get => View.Translation; set => View.Translation = value; }
		public Vector3 RotationDegrees { get => View.RotationDegrees; set => View.RotationDegrees = value; }

		public Card(CardInfo info, CardView view, int id)
		{
			Id = id;
			View = view;
			_face = (SpatialMaterial) view.GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
			(CardType, Title, Art, Text, Power) = info.GetData();
			View.GetNode<Area>("Area").Connect("mouse_entered", this, nameof(OnMouseEntered));
			View.GetNode<Area>("Area").Connect("mouse_exited", this, nameof(OnMouseEntered));
		}

		public void OnMouseEntered() => EmitSignal(nameof(OnCardEntered), this);
		public void OnMouseExited() => EmitSignal(nameof(OnCardExited), this);

	}
}
