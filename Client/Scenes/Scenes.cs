using Godot;

namespace CardGame.Client
{
    static class Scenes
    {
        private static readonly PackedScene RoomScene = GD.Load<PackedScene>("res://Client/Scenes/Room.tscn");
        private static readonly PackedScene CardScene = GD.Load<PackedScene>("res://Client/Card/CardView.tscn");
        public static Spatial Room() { return (Spatial) RoomScene.Instance(); }
        public static Spatial Card() { return (Spatial) CardScene.Instance(); }
    }
}