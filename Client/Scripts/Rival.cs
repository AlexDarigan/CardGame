using Godot;

namespace CardGame.Client
{
    public class Rival: Participant
    {
        public Rival()
        {
            Deck = new Zone(new Vector3(10.5f, 0, -8.25f), new Vector3(0, .034f, 0), new Vector3(0, 180, 180));
            Discard = new Zone(new Vector3(10.5f, .5f, -4.5f), new Vector3(0, 0.04f, 0), new Vector3(0, 0, 0));
            Hand = new Zone(new Vector3(1.1f, 6, -7.5f), new Vector3(1.1f, 0, 0), new Vector3(33, 0, 180));
            Units = new Zone(new Vector3(0, .33f, -3), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 0));
            Supports = new Zone(new Vector3(0, .33f, -7), new Vector3(1.5f, 0, 0), new Vector3(0, 0, 180));
        }
    }
}