using System;
using Godot;

namespace CardGame.Client
{
    public class Card : Spatial
    {
        public delegate void Pressed(Card pressed);
        public event Pressed CardPressed;
        private SpatialMaterial _face { get; set; }

        private int _id = -1;
        private CardType _cardType;
        private Spatial _powerNumbers;
        private Card() { }

        public int Id
        {
            get => _id;
            set => _id = _id < 0 ? value : _id;
        }
        
        public string Title { get; set; }

        public CardType CardType
        {
            get => _cardType;
            set
            {
                _cardType = CardType.Unit;
                _powerNumbers.Visible = value == CardType.Unit;
            }
        }

        public int Power { get; set; }
        public string Text { get; set; }
        public CardState CardState = CardState.None;
        public Participant Controller { get; set; }
        public Location CurrentLocation { get; set; }
        public Zone CurrentZone { get; set; }
       


        public override void _Ready()
        {
            _face = (SpatialMaterial) GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
            _powerNumbers = GetNode<Spatial>("Power");
            GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent));
        }
        
        public Texture Art
        {
            set
            {
                _face.AlbedoTexture = value;
                _face.EmissionTexture = value;
            }
        }

        public new Vector3 Translation
        {
            get => base.Translation;
            set => base.Translation = value;
        }

        public new Vector3 RotationDegrees
        {
            get => base.RotationDegrees;
            set => base.RotationDegrees = value;
        }

        public void LookAt(Vector3 position)
        {
            base.LookAt(position, Vector3.Up);
        }


        public void Update(CardState state)
        {
            CardState = state;
        }

        public void OnInputEvent(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
        {
            if (input is InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Doubleclick: true})
                CardPressed?.Invoke(this);
        }
    }
}