using Godot;
using System;
using System.Collections.Generic;
using System.Security.Policy;

public class LocationTest : Spatial
{
    [Export()] private Vector3 Origin { get; set; } = new Vector3();
    [Export()] private Vector3 Offset { get; set; } = new Vector3();
    private List<Sprite3D> Sprites { get; set; } = new List<Sprite3D>();
    private List<Location> Locations { get; set; } = new List<Location>();
    private Tween Tween { get; set; } = new();
    
    public override void _Ready()
    {
        AddChild(Tween);
        GD.Print($"{Offset} is offset");
        for (int i = 0; i < 10; i++)
        {
            AddCard();
        }
        
       // UpdateLocations();
        
    }

    public override void _Input(InputEvent input)
    {
        if (input is InputEventKey && input.IsPressed())
        {
            UpdateLocations();
        }
    }


    private void AddCard()
    {
        Sprite3D sprite3D = new Sprite3D() {Texture = GD.Load<Texture>("res://icon.png")};
        Sprites.Add(sprite3D);
        sprite3D.Name = Sprites.Count.ToString();
        AddChild(sprite3D);
        Location location = new(Origin, Offset, sprite3D, Locations.Count);
        Locations.Add(location);
        
    }

    private class Location
    {
        public Vector3 Origin { get; set; }
        public Vector3 Translation { get; set; } // actually an offset
        public Sprite3D Sprite3D;
        public int Index = 0;

        public Location(Vector3 origin, Vector3 translation, Sprite3D sprite3D, int index)
        {
            Origin = origin;
            Translation = translation;
            Sprite3D = sprite3D;
            Index = index;
        }
    }

    private async void UpdateLocations()
    {
        const float duration = 0.3f;
        foreach (Location location in Locations)
        {
            // What about 0?
            // If I'm correct, everything here should move to the left
            Vector3 destination = location.Translation * (location.Index - Locations.Count / 2);
            Tween.InterpolateProperty(location.Sprite3D, "translation", location.Sprite3D.Translation, destination, duration);
            Tween.Start();
            await ToSignal(Tween, "tween_all_completed");
            Tween.RemoveAll();
        }
    }
    
}
