using System;
using CardGame.Client.Views;
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

		public CardProperty<int> Id { get; set;}
		public CardProperty<string> Title { get; set; }
		public CardProperty<CardStates> CardState { get; set; }
		public CardProperty<Factions> Faction { get; set; }
		public CardProperty<CardTypes> CardType { get; set; }
		public CardProperty<Texture> Art { get; set; }
		public CardProperty<int> Power { get; set; }
		public CardProperty<string> Text { get; set; }
		
		public Card()
		{
		   
		}
		
		public override void _Ready() 
		{ 
			// Cast These To Relevant Types
			Id = new Id(this);
			Title = new Title(this);
			CardState = new CardState(this);
			Faction = new Faction(this);
			CardType = new CardType(this);
			Art = new Art(this);
			Power = new Power(this);
			Text = new Views.Text(this);
			GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent)); 
		}
		
		public void LookAt(Vector3 position) { base.LookAt(position, Vector3.Up); }
		
		public void OnInputEvent(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
		{
			if (input is not InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Doubleclick: true}) return;
			CardPressed?.Invoke(this);
			Console.WriteLine("Card Pressed");
		}
	}
}
