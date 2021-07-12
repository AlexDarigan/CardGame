using Godot;

namespace CardGame.Client.Views
{
	public class Art: CardProperty<Texture>
	{
		private SpatialMaterial Face { get; set; }
		
		public Art(Card card)
		{
			Card = card;
			OnReady();
			async void OnReady()
			{
				await card.ToSignal(card, "ready");
				Face = (SpatialMaterial) card.GetNode<MeshInstance>("Face").GetSurfaceMaterial(0);
			}
		}

		public override void Set(Texture value)
		{
			Value = value;
			Face.AlbedoTexture = value;
			Face.EmissionTexture = value;
		}
		
		public override Texture Get() { return Value; }
	}
}
