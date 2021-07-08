using Godot;
using System;
using System.Reflection;
using CardGame.Client;

public class ManualTests : Node
{
    
    // MANUAL TESTS
    // WILL HEAR NETWORK COMPLAINING (USE MOCK SOMEHOW?)
    // REQUIRES BUILD DECK/START GAME/CLEAR GAME FUNCTIONS 
    // Network not required

    private Cards Cards1 { get; set; }
    private Cards Cards2 { get; set; }
    private Player Player1 { get; set; }
    private Player Player2 { get; set; }
    private Room Room1 { get; set; }
    private Room Room2 { get; set; }

    public override void _Ready()
    {
        Room1 = new Room(Scenes.Room(), "TestRoom", new MultiplayerAPI());
        Room2 = new Room(Scenes.Room(), "TestRoom", new MultiplayerAPI());
        AddChild(Room1);
        AddChild(Room2);
        Player1 = (Player) typeof(Room).GetProperty("Player", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Room1);
        Player2 = (Player) typeof(Room).GetProperty("Player", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Room2);
        Cards1 = (Cards) typeof(Room).GetProperty("Cards", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Room1);
        Cards2 = (Cards) typeof(Room).GetProperty("Cards", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Room2);
    }
    
}
