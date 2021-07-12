using Godot;
using System;
using JetBrains.Annotations;

namespace CardGame.Client
{
    [UsedImplicitly]
    public class Table : Spatial
    {
        public Action PassPlayPressed;
        private SpatialMaterial PassPlay { get; set; }

        public States State { set => PassPlay.AlbedoColor = value == States.IdleTurnPlayer ? Colors.Aqua : Colors.Red; }

        public override void _Ready()
        {
            PassPlay = (SpatialMaterial) GetNode<MeshInstance>("PassPlayButton").GetSurfaceMaterial(0);
            GetNode<Area>("PassPlayButton/Area").Connect("input_event", this, "OnPassPlayPressed");
        }
        
        private void OnPassPlayPressed(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
        {
            if (input is not InputEventMouseButton {Doubleclick: true}) return;
            PassPlay.AlbedoColor = Colors.Red;
            PassPlayPressed();
        }
    }
}