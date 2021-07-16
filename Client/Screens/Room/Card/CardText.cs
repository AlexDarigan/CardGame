using Godot;

namespace CardGame.Client
{
    public class CardText: CardProperty<string>
    {
        private Label Display { get; }
        protected override string Property { get; set; }

        public override string Value
        {
            get => Display.Text;
            set => Display.Text = value;
        }
        
        public CardText(Node card)
        {
            Display = card.GetNode<Label>("Text");
        }

        public void Show()
        {
            if (Display.Text == "") { return; }

            // Probably better to have this update with the card's unprojected 2D position rather..
            // ..than use the mouse but we'll keep with it for now
            Display.RectSize = Display.GetMinimumSize();
            Display.RectPosition = Display.GetGlobalMousePosition() + new Vector2(0, -25);
            Display.Show();
        }

        public void Hide() { Display.Hide(); }
    }
}