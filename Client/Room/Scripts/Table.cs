using Godot;
using System;

public class Table : Spatial
{
   	// Our Physical View Of The Game
	public Participant Player { get; private set; }
	public Participant Opponent { get; private set; }
		
	public override void _Ready()
	{
		Player = GetNode<Participant>("Player");
		Opponent = GetNode<Participant>("Opponent");
	}

}
