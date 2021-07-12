using Godot;
using JetBrains.Annotations;

namespace CardGame.Client
{
    [UsedImplicitly]
    public class Effects : Node
    {
        private Tween Tween { get; set; }
        private AudioStreamPlayer SoundEffects { get; set; }
        private AudioStreamPlayer BackgroundMusic { get; set; }
    
        public override void _Ready()
        {
            BackgroundMusic = GetNode<AudioStreamPlayer>("BGM");
            SoundEffects = GetNode<AudioStreamPlayer>("SFX");
            Tween = GetNode<Tween>("GFX");
        }

        public void InterpolateCallback(params object[] args)
        {
            Tween.Call("interpolate_callback", args);
        }

        public void InterpolateProperty(params object[] args)
        {
            Tween.Call("interpolate_property", args);
        }

        public void RemoveAll()
        {
            Tween.RemoveAll();
        }

        public void Start()
        {
            Tween.Start();
        }

        public SignalAwaiter Executed()
        {
            return ToSignal(Tween, "tween_all_completed");
        }
    }
}