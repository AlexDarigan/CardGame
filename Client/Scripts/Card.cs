using System;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
    public class Card : Object
    {
        public delegate void Pressed(Card pressed);
        public event Pressed CardPressed;
        private readonly SpatialMaterial _face;
        private readonly Spatial _view;
        public int Id { get; }
        public string Title { get; }
        public CardType CardType { get; }
        public int Power { get; }
        public string Text { get; }
        public CardState CardState = CardState.None;
        public Participant Controller { get; set; }
        public Location CurrentLocation { get; set; }
        private Card() { }
        
        public Card(CardInfo info, Spatial view, int id)
        {
            Id = id;
            _view = view;
            _face = (SpatialMaterial) _view.GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
            (CardType, Title, Art, Text, Power) = info;
            _view.GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent));
            _view.GetNode<Spatial>("Power").Visible = CardType == CardType.Unit;
        }

        private Texture Art
        {
            set
            {
                _face.AlbedoTexture = value;
                _face.EmissionTexture = value;
            }
        }

        public Vector3 Translation
        {
            get => _view.Translation;
            set => _view.Translation = value;
        }

        public Vector3 RotationDegrees
        {
            get => _view.RotationDegrees;
            set => _view.RotationDegrees = value;
        }

        public void LookAt(Vector3 position)
        {
            _view.LookAt(position, Vector3.Up);
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

        public override void _Notification(int what)
        {
            if (what == NotificationPredelete) { _view.Free(); }
            base._Notification(what);
        }
    }
}