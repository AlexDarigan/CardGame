using Godot;
using System.Globalization;

namespace CardGame.Client
{
	public class Power: CardProperty<int>
	{
		private Sprite3D[] Numbers { get; }
		protected override int Property { get; set; }
		public override int Value
		{
			get => Property;
			set
			{
				Property = value;
				string power = value.ToString(CultureInfo.InvariantCulture);
				for (int i = power.Length - 1; i > 0; i--)
				{
					
					Numbers[i].Texture = 
					GD.Load<Texture>($"res://Client/Assets/Numbers/Impact/{power[i]}.png");
				}
			}
		}
		
		public Power(Node card)
		{
			Numbers = new Sprite3D[4];
			Godot.Collections.Array x = card.GetNode<Spatial>("Power").GetChildren();
			for (int i = 0; i < 4; i++) { Numbers[i] = (Sprite3D) x[i]; }
		}
	}
}
