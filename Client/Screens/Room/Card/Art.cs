using Godot;

namespace CardGame.Client
{
    public class Art: CardProperty<Texture>
    {
        private SpatialMaterial Face { get; }
        protected override Texture Property { get; set; }
        public override Texture Value
        {
            get => Property;
            set
            {
                Property = value;
                Face.AlbedoTexture = value;
                Face.EmissionTexture = value;
            }
        }
        
        public Art(Node card)
        {
            Face = (SpatialMaterial) card.GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
        }
    }
}