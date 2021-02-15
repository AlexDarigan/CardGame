using Godot;

namespace CardGame.Server
{
    // Tool is required for our tests when we're using in-editor launch context
    [Tool]
    public class CardData: Resource
    {
        [Export()] public string Title = "NullCard";
        [Export()] public SetCodes SetCodes = SetCodes.NullCard;
        [Export()] public CardType CardType = CardType.Null;
        [Export()] public Faction Faction = Faction.Null;
        [Export()] public int Power = 0;

        
    }
}