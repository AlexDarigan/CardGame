using Godot;
using JetBrains.Annotations;

namespace CardGame.Client
{
    [UsedImplicitly]
    public class Text : Control
    {
        private int _turnCount = 0;
        private Label TurnCount { get; set; }
        private Label _state;
        private Label _id;
        private Label _card;
        private Label GameOver { get; set; }
        private Timer Timer { get; set; }
        
        public States State { set => _state.Text = value.ToString(); }
        public int Id { set => _id.Text = value.ToString(); }
        public static string CardText { get; set; }

        public override void _Ready()
        {
            TurnCount = GetNode<Label>("TurnCount");
            _id = GetNode<Label>("ID");
            _state = GetNode<Label>("State");
            _card = GetNode<Label>("Card");
            GameOver = GetNode<Label>("GameOver");
        }

        public void AddTurn()
        {
            _turnCount++;
            TurnCount.Text = _turnCount.ToString();
        }

        public void DisplayGameOver(bool gameLost)
        {
            GameOver.Text = gameLost ? "You Lose!" : "You Win!";
            GameOver.Visible = true;
        }

        public override void _Process(float delta)
        {
            _card.Text = CardText;
        }
    }
}