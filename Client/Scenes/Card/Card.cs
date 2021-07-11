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

        public CardProperty<int> Id { get; }
        public CardProperty<string> Title { get; }
        public CardProperty<CardStates> CardState { get; }
        public CardProperty<Factions> Faction { get; }
        public CardProperty<CardTypes> CardType { get; }
        public CardProperty<Texture> Art { get; }
        public CardProperty<int> Power { get; }
        public CardProperty<string> Text { get; }
        
        public Card()
        {
            Id = new Id(this);
            Title = new Title(this);
            CardState = new CardState(this);
            Faction = new Faction(this);
            CardType = new CardType(this);
            Art = new Art(this);
            Power = new Power(this);
            Text = new Text(this);
        }
        
        public override void _Ready() { GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent)); }
        
        public void LookAt(Vector3 position) { base.LookAt(position, Vector3.Up); }
        
        public void OnInputEvent(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
        {
            if (input is not InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Doubleclick: true}) return;
            CardPressed?.Invoke(this);
            Console.WriteLine("Card Pressed");
        }
    }
}