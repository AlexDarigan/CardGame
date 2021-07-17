using Godot;

namespace System.Runtime.CompilerServices.Assets.Sounds
{
    public static class Sounds
    {
        public static readonly AudioStream Draw = GD.Load<AudioStream>("res://Client/Assets/Sounds/Card_Game_Movement_Deal_Single_02.wav");
        public static readonly AudioStream Deploy = GD.Load<AudioStream>("res://Client/Assets/Sounds/Card_Game_Play_Slam_01.wav");
        public static readonly AudioStream SetFaceDown = GD.Load<AudioStream>("res://Client/Assets/Sounds/Card_Game_Play_Hush_01.wav");
        public static readonly AudioStream Activate = GD.Load<AudioStream>("res://Client/Assets/Sounds/Card_Game_Play_Slam_Water_Rise_02.wav");
        public static readonly AudioStream Resolve = GD.Load<AudioStream>("res://Client/Assets/Sounds/Card_Game_Items_Cork_Pop_01.wav");
        
        // TODO:
        // Add Sounds To Rival
        // Add Damage Sound
        // Add Destroyed Sound
        // Add Victory/Defeat Sound & Stingers
    }
}