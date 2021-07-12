using Godot;
using CardGame;
using CardGame.Client;
using JetBrains.Annotations;

[UsedImplicitly]
public class ChessClockButton : MeshInstance
{
    private SpatialMaterial Material { get; set; }
    public States State
    {
        set => Material.AlbedoColor = value == States.IdleTurnPlayer ? Colors.Aqua : Colors.Red;
    }
    
    public override void _Ready()
    {
        Material = (SpatialMaterial) GetSurfaceMaterial(0);
        GetNode<Area>("Area").Connect("input_event", this, "OnButtonPressed");
    }
    
    private void OnButtonPressed(Node camera, InputEvent input, Vector3 clickPos, Vector3 clickNormal, int shapeIdx)
    {
        if (input is not InputEventMouseButton {Doubleclick: true}) return;
        Player player = (Player) GetParent().GetParent<Room>().GetPlayer(true);
        Material.AlbedoColor = Colors.Red;
        player.EndTurn();
    }
}
