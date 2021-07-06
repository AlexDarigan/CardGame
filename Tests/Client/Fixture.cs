using System;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;


namespace CardGame.Client.Tests
{
    public class Fixture: WAT.Test
    {
        // Note To Self: Client tests do not work well in-editor due to their reliance on the server
        private static readonly PackedScene MainScene = GD.Load<PackedScene>("res://Global/Main.tscn");
        private Main _game;
        private Room _room1;
        private Room _room2;
        protected Participant P1;
        protected Participant P2;
        
        // Helper Methods
        // - BuildDeck (Put it on MainScene and then

        // ClientAction
        // PostDeploy
        // PostSet
        // PostActivate
        // PostAttackUnit
        // PostAttackPlayer
        // PostEndTurn
        
        // Operations (ClientSide?)
        // This one will require work.
        
        // BattleState
        // Win, Lose, Tie
        
        
        protected async Task StartGame(Array<SetCodes> deckList1 = null, Array<SetCodes> deckList2 = null)
        {
            _game?.Free();
            _game = (Main) MainScene.Instance();
            _game.DeckList1 = deckList1 ?? _game.DeckList1;
            _game.DeckList2 = deckList2 ?? _game.DeckList2;
            AddChild(_game);
            
            // Await the Main Scene Event to gather our data
            TestEventData data = await UntilEvent(_game, nameof(_game.GameBegun), 3.0);
            Players room = (Players) data.Arguments;
            
            _room1 = room.Room1;
            _room2 = room.Room2;
            P1 = room.Player1;
            P2 = room.Player2;
            
            // Await game state update so every player has a hand at least
            await Update();
        }
        
        protected static Array<SetCodes> BuildDeck(SetCodes setCode = SetCodes.NullCard)
        {
            Array<SetCodes> deckList = new();
            for (int i = 0; i < 40; i++) { deckList.Add(setCode); }
            return deckList;
        }
        
        protected async Task Queue(params Action[] arguments)
        {
            foreach (Action play in arguments)
            {
                play();
                await Update();
            }

            // I'm not entirely sure why this is required BUT worst case scenario is that it is just timed out
            await Update();
        }

        private async Task Update() { await UntilEvent(_game, nameof(_game.RoomsUpdated), 10); }
    }
}