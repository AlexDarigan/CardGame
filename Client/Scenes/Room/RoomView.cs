using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Views
{
    [UsedImplicitly]
    public class RoomView : Node
    {
        private int Id { get; set; }
        private Label PlayerId { get; set; }
        public Label State;
        public TurnCounter TurnCounter;
        public HealthBar PlayerHealth;
        public HealthBar RivalHealth;
        public ChessClockButton ChessClockButton;
        public Label GameOver;
        public Heart PlayerHeart;
        public Heart RivalHeart;
        private Mouse Mouse { get; } = new Mouse();

        public void OnAttackDeclared()
        {
            Mouse.OnAttackDeclared();
        }

        public void OnAttackCancelled()
        {
            Mouse.OnAttackCancelled();
        }

        public override void _Ready()
        {
            AddChild(Mouse);
            PlayerId = GetNode<Label>("GUI/ID");
            State = GetNode<Label>("GUI/State");
            TurnCounter = GetNode<TurnCounter>("GUI/TurnCount");
            PlayerHealth = GetNode<HealthBar>("GUI/PlayerHealth");
            RivalHealth = GetNode<HealthBar>("GUI/RivalHealth");
            ChessClockButton = GetNode<ChessClockButton>("Table/ChessClockButton");
            GameOver = GetNode<Label>("GUI/GameOver");
            PlayerHeart = GetNode<Heart>("Table/PlayerHeart");
            RivalHeart = GetNode<Heart>("Table/RivalHeart");
            Id = GetParent().CustomMultiplayer.GetNetworkUniqueId();
            PlayerId.Text = Id.ToString();

            // We kept trying to do this in the constructor of the Room Object except neither of these existed within..
            // the tree at that time.
            Player player = (Player) GetParent<Room>().GetPlayer(true);
            RivalHeart.Pressed += player.OnRivalHeartPressed;

        }
        
    }
}