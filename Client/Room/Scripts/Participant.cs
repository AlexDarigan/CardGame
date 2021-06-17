using Godot;
using System;

public class Participant : Spatial
{
	private Spatial Deck;
	private Spatial Discard;
	private Spatial Hand;
	private Spatial Units;
	private Spatial Support;
	
	public override void _Ready()
	{
		Deck = GetNode<Spatial>("Deck");
		Discard = GetNode<Spatial>("Discard");
		Hand = GetNode<Spatial>("Hand");
		Units = GetNode<Spatial>("Unit");
		Support = GetNode<Spatial>("Support");
	}
}
