using Godot;

namespace CardGame.Client
{
    public class CardData: Resource
    {
        // SetCode is initialized on load
        public SetCodes SetCode;
        [Export()] public string Name = "Card";
        [Export()] public Texture Art = Assets.GetArt("NullCard");
        [Export(PropertyHint.Enum)] public CardType CardType = CardType.Null;
        [Export(PropertyHint.MultilineText)] public string Text = "";
        [Export()] public int Power;
        
        public void Deconstruct(out CardType cardType, out string title, out Texture art, out string text, out int power)
        {
            cardType = CardType;
            title = Name;
            art = Art;
            text = Text;
            power = Power;
        }
    }
}