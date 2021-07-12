using System;
using System.Globalization;
using Godot;

namespace CardGame.Client.Views
{
    public class Power: CardProperty<int>
    {
        private Spatial View { get; set; }
        
        public Power(Card card)
        {
            Card = card;
            OnReady();
            
            async void OnReady()
            {
                await card.ToSignal(card, "ready");
                View = card.GetNode<Spatial>("Power");
            }
        }

        public override void Set(int value)
        {
            Value = value;
            string power = value.ToString(CultureInfo.InvariantCulture);
            for (int i = 0; i < 4; i++)
            {
                View.GetChild<Sprite3D>(0).Texture =
                    GD.Load<Texture>($"res://Client/Assets/Numbers/Impact/{power[0]}.png");
            }
        }
        public override int Get() { return Value; }
    }
}


