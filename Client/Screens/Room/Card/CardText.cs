using Godot;

namespace CardGame.Client
{
    public class CardText: CardProperty<string>
    {
        private Label Display { get; }
        protected override string Property { get; set; }

        public override string Value
        {
            get => Property;
            set
            {
                Property = value;
                Display.Text = value;
            }
        }
        
        public CardText(Node card)
        {
            Display = card.GetNode<Label>("Text");
        }

        public void Show()
        {
            if (Property == "") { return; }
            Text.CardText = Property;
        }

        public void Hide() { Display.Hide(); }
    }
}