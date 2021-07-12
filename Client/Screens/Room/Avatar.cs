using Godot;
using System;
using JetBrains.Annotations;

namespace CardGame.Client
{
    [UsedImplicitly]
    public class Avatar : MeshInstance
    {
        public event Action Pressed;

        public override void _Ready()
        {
            GetNode<Area>("Area").Connect("input_event", this, nameof(OnInputEvent));
        }

        public void OnInputEvent(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
        {
            if (input is InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Doubleclick: true})
                Pressed?.Invoke();
        }
    }
}
