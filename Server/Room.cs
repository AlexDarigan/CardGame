using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Godot;


namespace CardGame.Server
{
    public class Room: Node
    {
        private readonly Match _match = new Match();
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        private readonly Dictionary<int, Card> _cards = new Dictionary<int, Card>();
        private int _nextCardId = 0;

        public Room()
        {
            // I believe an empty constructor is required in Godot Classes that have non-empty constructor(s)
            // ..for the sake of some Godot callbacks
        }
        public Room(Player player1, Player player2)
        {
            player1.Opponent = player2;
            player2.Opponent = player1;
            _players[player1.Id] = player1;
            _players[player2.Id] = player2;
        }

        [Master]
        public void OnClientReady()
        {
            _players[CustomMultiplayer.GetRpcSenderId()].Ready = true;
            if (_players.Values.Any(player => !player.Ready))
            {
                return;
            }
            Start();
        }
        
        // Making this public for the sake of Tests
        public void Start()
        {
            SetDecks();
            DrawStartingHands();
        }

        private void SetDecks()
        {
            // We Set Decks here so we cards can be registered
            foreach (Player player in _players.Values)
            {
                foreach (SetCodes setCode in player.DeckList)
                {
                    Card card = new Card(_nextCardId, player);
                    Resource cardData = GD.Load<Resource>($"res://Server/Library/{setCode.ToString()}.tres");
                    
                    // Unfortunately Godot (as of writing) doesn't support custom resources properly in C# so we have
                    // to resort virtual get methods for each property. It might be worthwhile to create our custom
                    // inspector in future for this?
                    card.Title = (string) cardData.Get("Title");
                    card.SetCodes = (SetCodes) cardData.Get("SetCodes");
                    card.CardType = (CardType) cardData.Get("CardType");
                    card.Faction = (Faction) cardData.Get("Faction");
                    card.Power = (int) cardData.Get("Power");
                    _nextCardId++;
                    player.Deck.Add(card);
                }
            }
        }

        private void DrawStartingHands()
        {
            foreach (Player player in _players.Values)
            {
                for (int i = 0; i < 7; i++)
                {
                    _match.Draw(player);
                }
            }
        }
        
    }
}