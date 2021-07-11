using Godot;

namespace CardGame.Client.Views
{
    public static class Scenes
    {
        private static readonly PackedScene RoomScene = GD.Load<PackedScene>("res://Client/Scenes/Room/Room.tscn");
        private static readonly PackedScene CardScene = GD.Load<PackedScene>("res://Client/Scenes/Card/Card.tscn");

        public static RoomView Room()
        {
            return (RoomView) RoomScene.Instance();
        }

        public static Card Card()
        {
            return (Card) CardScene.Instance();
        }
    }
}