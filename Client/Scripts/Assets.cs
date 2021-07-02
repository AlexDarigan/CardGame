using Godot;

namespace CardGame.Client
{
    public static class Assets
    {
        public const string Library = "Client/Scripts/Library.json";
        public static Texture GetArt(string name) => (Texture) GD.Load($"Client/Assets/CardArt/{name}.png");
        public static Texture GetNumber(string name) => (Texture) GD.Load($"Client/Assets/Numbers/{name}.png");
    }


}