using Godot;

namespace CardGame.Client
{
    public class Card : Object
    {
        public delegate void Pressed(Card pressed);

        private readonly SpatialMaterial _face;
        private readonly Spatial _view;
        public readonly int Id;
        private int _power;
        private string _text;
        private string _title;
        public CardState CardState = CardState.None;

        public Card(CardInfo info, Spatial view, int id)
        {
            Id = id;
            _view = view;
            _face = (SpatialMaterial) _view.GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
            CardType cardType;
            (cardType, _title, Art, _text, _power) = info;
            _view.GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent));
            _view.GetNode<Spatial>("Power").Visible = cardType == CardType.Unit;
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

        public event Pressed CardPressed;

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