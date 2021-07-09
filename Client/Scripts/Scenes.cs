using Godot;

namespace CardGame.Client
{
    public static class Scenes
    {
        private static readonly PackedScene RoomScene = GD.Load<PackedScene>("res://Client/Scenes/Room.tscn");
        private static readonly PackedScene CardScene = GD.Load<PackedScene>("res://Client/Scenes/Card.tscn");

        public static RoomView Room()
        {
            return (RoomView) RoomScene.Instance();
        }

        public static Spatial Card()
        {
            return (Spatial) CardScene.Instance();
        }
    }
}