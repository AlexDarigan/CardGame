using Godot;

namespace CardGame.Client
{
	public class Card: Spatial
	{
		public int Id;
		public string Title;
		public string Text;
		public CardType CardType;
		private SpatialMaterial CardFace;
		public int Power;

		public Texture Art { set { CardFace.AlbedoTexture = value; CardFace.EmissionTexture = value; } }
		
		public override void _Ready()
		{
			CardFace = (SpatialMaterial) GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
		}

	}
}
