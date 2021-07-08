using Godot;
using System;

public class PlayerOffsets : Spatial
{
    [Export()] public Vector3 Origin;
    [Export()] public Vector3 DeckTranslationOffSet = new Vector3();
    [Export()] public Vector3 DeckRotation = new Vector3();
    
    // Origin = X
    // Rotation = DR
    // OffSet
    
    // Location = X
    // Location.x/y/z * translation * offset
    // What if we've already moved locations (update offsets?)
}
