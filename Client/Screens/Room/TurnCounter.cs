using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Views
{
    [UsedImplicitly]
    public class TurnCounter : Label
    {
        private int _count = 1;

        private int Count
        {
            get => _count;
            set
            {
                _count = value;
                Text = value.ToString();
            }
        }

        public void NextTurn()
        {
            Count++;
        }

        public override void _Ready()
        {
            Text = Count.ToString();
        }
    }
}